using Models.Tasks;

using System.Security.Principal;

namespace Tasks.Commands
{
    public class WhoAmI : CerberusCommand
    {
        public override string Name => "whoami";

        public override MythicTaskResult Execute(MythicTask task)
        {
            var identity = WindowsIdentity.GetCurrent();

            var result = new MythicTaskResult
            {
                task_id = task.id,
                user_output = identity.Name,
                completed = true,
                status = "success"
            };

            identity.Dispose();

            return result;
        }
    }
}
