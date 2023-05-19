using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cerberus.Models.Tasks
{
    public class MythicTaskResult
    {
        public string task_id { get; set; }
        public string user_output { get; set; }
        public bool completed { get; set; }
    }
}
