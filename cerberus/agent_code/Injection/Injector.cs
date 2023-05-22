using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Injection
{
    public abstract class Injector
    {
        public abstract bool Inject(byte[] shellcode, int pid = 0, string filePath = null);
    }
}
