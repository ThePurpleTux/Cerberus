using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ghost.Models.Tasks
{
    public abstract class GhostCommand
    {
        public abstract string Name { get; }
        public abstract string Execute(GhostTask task);
    }
}
