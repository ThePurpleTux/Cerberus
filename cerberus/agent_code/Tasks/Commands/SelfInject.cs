using Injection;
using Models.Tasks;
using Extend;

namespace Tasks.Commands
{
    public class SelfInject : CerberusCommand
    {
        public override string Name => "self-inject";

        public override MythicTaskResult Execute(MythicTask task)
        {
            MythicTaskResult result;

            var FileBytes = new byte[10];

            var injector = new SelfInjector();
            var success = injector.Inject(FileBytes);

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
