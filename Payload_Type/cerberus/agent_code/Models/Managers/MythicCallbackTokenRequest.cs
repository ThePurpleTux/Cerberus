using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Managers
{
    public class MythicCallbackTokenRequest
    {
        public string action { get; set; } // add or remove
        public int token_id { get; set; }
    }
}
