using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Tasks
{
    public class MythicTaskResult
    {
        public string task_id { get; set; }
        public string user_output { get; set; }
        public bool completed { get; set; }
        public string status { get; set; }
        public string error { get; set; } = null;
        public int token_id { get; set; } = 0;
    }
}
