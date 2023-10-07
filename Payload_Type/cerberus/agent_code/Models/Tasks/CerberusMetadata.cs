using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Models.Tasks
{
    public class CerberusMetadata
    {
        public string action { get; set; }
        public string ip { get; set; }
        public string os { get; set; }
        public string user { get; set; }
        public string host { get; set; }
        public string domain { get; set; }
        public int pid { get; set; }
        public string uuid { get; set; }
        public string architecture { get; set; }
    }
}
