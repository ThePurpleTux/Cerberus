using Cerberus.Models.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Cerberus.Commands
{
    public class WhoAmI : CerberusCommand
    {
        public override string Name => "whoami";

        public override string Execute(CerberusTask task)
        {
            var identity = WindowsIdentity.GetCurrent();
            return identity.Name;
        }
    }
}
