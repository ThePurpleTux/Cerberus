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

        public override string Execute(MythicTask task)
        {
            var Arguments = Encoding.UTF8.GetBytes(task.parameters).Deserialize<MkdirParam>();

            if (Arguments is null || string.IsNullOrWhiteSpace(Arguments.path))
            {
                Arguments.path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            }

            var dirInfo = Directory.CreateDirectory(Arguments.path);

            return $"{dirInfo.FullName} created";
        }
    }

    public class MkdirParam
    {
        public string path { get; set; }
    }
}
