using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Managers
{
    public interface IManager
    {
        string Name { get; }
        void Init();
    }
}
