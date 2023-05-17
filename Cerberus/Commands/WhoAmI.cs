using Ghost.Models.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Ghost.Commands
{
    public class WhoAmI : GhostCommand
    {
        public override string Name => "whoami";

        public override string Execute(GhostTask task)
        {
            var identity = WindowsIdentity.GetCurrent();
            return identity.Name;
        }
    }
}
