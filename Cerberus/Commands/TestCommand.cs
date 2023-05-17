using Ghost.Models.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ghost.Commands
{
    public class TestCommand : GhostCommand
    {
        public override string Name => "TestCommand";

        public override string Execute(GhostTask task)
        {
            return "Tasking Logic is Success";
        }
    }
}
