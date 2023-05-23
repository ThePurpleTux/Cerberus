using Models.Tasks;

using Extend;

using System;
using System.IO;
using System.Text;

namespace Tasks.Commands
{
    public class ChangeDirectory : CerberusCommand
    {
        public override string Name => "cd";

        public override MythicTaskResult Execute(MythicTask task)
        {
            var taskArgs = Encoding.UTF8.GetBytes(task.parameters).Deserialize<CdParam>();

            if (taskArgs is null || string.IsNullOrWhiteSpace(taskArgs.path))
            {
                taskArgs.path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            }

            MythicTaskResult result;

            try
            {
                Directory.SetCurrentDirectory(taskArgs.path);

                result = new MythicTaskResult
                {
                    task_id = task.id,
                    user_output = Directory.GetCurrentDirectory(),
                    completed = true,
                    status = "success"
                };
            }
            catch(Exception ex)
            {
                result = new MythicTaskResult
                {
                    task_id = task.id,
                    user_output = ex.Message,
                    completed = true,
                    status = "Error",
                    error = ex.Message
                };
                //return ex.Message;
            }

            return result;
            //return Directory.GetCurrentDirectory();
        }
    }

    public class CdParam
    {
        public string path { get; set; }
    }
}
