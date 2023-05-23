using Models.Tasks;

using Extend;

using System;
using System.IO;
using System.Text;

namespace Tasks.Commands
{
    public class CreateDirectory : CerberusCommand
    {
        public override string Name => "mkdir";

        public override MythicTaskResult Execute(MythicTask task)
        {
            var Arguments = Encoding.UTF8.GetBytes(task.parameters).Deserialize<MkdirParam>();
            MythicTaskResult result;

            if (Arguments is null || string.IsNullOrWhiteSpace(Arguments.path))
            {
                Arguments.path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            }

            try
            {
                var dirInfo = Directory.CreateDirectory(Arguments.path);

                result = new MythicTaskResult
                {
                    task_id = task.id,
                    user_output = $"{dirInfo.FullName} created",
                    completed = true,
                    status = "success",
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
            }

            return result;
        }
    }

    public class MkdirParam
    {
        public string path { get; set; }
    }
}
