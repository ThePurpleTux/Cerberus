using Cerberus.Models.Tasks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cerberus.Commands
{
    public class CreateDirectory : CerberusCommand
    {
        public override string Name => "mkdir";

        public override string Execute(MythicTask task)
        {
            var Arguments = task.parameters.Split(' ');
            string path;

            if (Arguments is null || Arguments.Length == 0)
            {
                path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            }
            else
            {
                path = Arguments[0];
            }

            var dirInfo = Directory.CreateDirectory(path);
            return $"{dirInfo.FullName} created";
        }
    }
}
