using Cerberus.Models.Tasks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cerberus.Commands
{
    public class PrintWorkingDirectory : CerberusCommand
    {
        public override string Name => "pwd";

        public override string Execute(CerberusTask task)
        {
            return Directory.GetCurrentDirectory();
        }
    }
}
