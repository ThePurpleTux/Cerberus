using Ghost.Models.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ghost.Commands
{
    public class RevToSelf : GhostCommand
    {
        public override string Name => "rev2self";

        public override string Execute(GhostTask task)
        {
            if (Native.Advapi.RevertToSelf())
            {
                return "Reverted to self";
            }

            return "Failed to revert";
        }
    }
}
