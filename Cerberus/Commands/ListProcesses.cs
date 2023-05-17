using Agent;
using Ghost.Models.Tasks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Ghost.Commands
{
    public class ListProcesses : GhostCommand
    {
        public override string Name => "ps";

        public override string Execute(GhostTask task)
        {
            var results = new SharpSploitResultList<ListProcessesResult>();
            var processes = Process.GetProcesses();

            foreach ( var process in processes )
            {
                var result = new ListProcessesResult
                {
                    ProcessName = process.ProcessName,
                    ProcessId = process.Id,
                    SessionId = process.SessionId,
                };

                result.ProcessPath = GetProcessPath(process);
                result.Owner = GetProcessOwner(process);
                result.Arch = GetProcessArch(process);

                results.Add(result);
            }

            return results.ToString();
        }

        private string GetProcessArch(Process process)
        {
            try
            {
                var is64bitOS = Environment.Is64BitOperatingSystem;

                if (!is64bitOS)
                    return "x86";

                if (!Native.Kernel32.IsWow64Process(process.Handle, out var isWow64))
                    return "-";

                if (is64bitOS && isWow64)
                    return "x86";

                return "x64";
            }
            catch
            {
                return "-";
            }
        }

        private string GetProcessOwner(Process process)
        {
            var hToken = IntPtr.Zero;

            try
            {
                if (!Native.Advapi.OpenProcessToken(process.Handle, Native.Advapi.DesiredAccess.TOKEN_ALL_ACCESS, out hToken))
                    return "-";

                var identity = new WindowsIdentity(hToken);
                return identity.Name;
            }
            catch 
            { 
                return "-"; 
            }
            finally 
            { 
                Native.Kernel32.CloseHandle(hToken); 
            }
        }

        private string GetProcessPath(Process process)
        {
            try
            {
                return process.MainModule.FileName;
            }
            catch
            {
                return "-";
            }
        }
    }

    public sealed class ListProcessesResult : SharpSploitResult
    {
        public string ProcessName { get; set; }
        public string ProcessPath { get; set; }
        public string Owner { get; set; }
        public int ProcessId { get; set; }
        public int SessionId { get; set; }
        public string Arch { get; set; }

        protected internal override IList<SharpSploitResultProperty> ResultProperties => new List<SharpSploitResultProperty>
        {
            new SharpSploitResultProperty { Name = nameof(ProcessName), Value = ProcessName },
            new SharpSploitResultProperty { Name = nameof(ProcessPath), Value = ProcessPath },
            new SharpSploitResultProperty { Name = nameof(Owner), Value = Owner },
            new SharpSploitResultProperty { Name = "PID", Value = ProcessId },
            new SharpSploitResultProperty { Name = nameof(SessionId), Value = SessionId },
            new SharpSploitResultProperty { Name = nameof(Arch), Value = Arch },
        };
    }
}
