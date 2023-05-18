using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cerberus.Models.Tasks
{
    public class MythicTask
    {
        public string id { get; set; }
        public string command { get; set; }
        public string parameters { get; set; }
        public string result { get; set; }
        public bool completed { get; set; } = false;
        public bool started { get; set; } = false;
        public bool error { get; set; } = false;
    }
}
