using Ghost.Models.Tasks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ghost.Commands
{
    public class CreateDirectory : GhostCommand
    {
        public override string Name => "mkdir";

        public override string Execute(GhostTask task)
        {
            string path;

            if (task.Arguments is null || task.Arguments.Length == 0)
            {
                path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            }
            else
            {
                path = task.Arguments[0];
            }

            var dirInfo = Directory.CreateDirectory(path);
            return $"{dirInfo.FullName} created";
        }
    }
}
