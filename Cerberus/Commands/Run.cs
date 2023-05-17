using Cerberus.Models.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cerberus.Commands
{
    public class Run : CerberusCommand
    {
        public override string Name => "run";

        public override string Execute(CerberusTask task)
        {
            if (task.Arguments is null || task.Arguments.Length == 0)
                return "No arguments supplied. Must supply file to execute and any required arguments";

            var fileName = task.Arguments[0];
            var args = string.Join(" ", task.Arguments.Skip(1));

            return Internal.Execute.ExecuteCommand(fileName, args);
        }
    }
}
