using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Tasks
{
    public class PostTaskingRequest
    {
        public string action { get; set; }
        public MythicTaskResult[] responses { get; set; }
    }
}
