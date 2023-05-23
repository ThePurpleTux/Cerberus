using Models.Tasks;

namespace Tasks.Commands
{
    public class RevToSelf : CerberusCommand
    {
        public override string Name => "rev2self";

        public override MythicTaskResult Execute(MythicTask task)
        {
            MythicTaskResult result;

            if (Native.Advapi.RevertToSelf())
            {
                result = new MythicTaskResult
                {
                    task_id = task.id,
                    user_output = "Reverted to self",
                    completed = true,
                    status = "success"
                };

                return result;
            }

            result = new MythicTaskResult
            {
                task_id = task.id,
                user_output = "Failed to revert",
                completed = true,
                status = "Error",
                error = "Failed to revert"
            };

            return result;
        }
    }
}
