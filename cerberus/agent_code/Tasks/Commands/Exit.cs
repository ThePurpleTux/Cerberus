using Models.Tasks;

namespace Tasks.Commands
{
    public class Exit : CerberusCommand
    {
        public override string Name => "exit";

        public override MythicTaskResult Execute(MythicTask task)
        {
            var result = new MythicTaskResult
            {
                task_id = task.id,
                user_output = "Tasked agent to exit",
                completed = true,
                status = "success"
            };

            return result;
        }
    }
}
