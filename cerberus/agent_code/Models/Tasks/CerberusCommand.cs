using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Tasks
{
    public abstract class CerberusCommand
    {
        public abstract string Name { get; }
        public abstract MythicTaskResult Execute(MythicTask task);
    }
}
