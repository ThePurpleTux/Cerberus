using Cerberus.Models.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cerberus.Commands
{
    public class TestCommand : CerberusCommand
    {
        public override string Name => "TestCommand";

        public override string Execute(MythicTask task)
        {
            return "Tasking Logic is Success";
        }
    }
}
