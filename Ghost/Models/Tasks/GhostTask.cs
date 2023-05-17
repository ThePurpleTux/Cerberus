using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Ghost.Models.Tasks
{
    [DataContract]
    public class GhostTask
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "command")]
        public string Command { get; set; }
        
        [DataMember(Name = "arguments")]
        public string[] Arguments { get; set; }

        [DataMember(Name = "file")]
        public string File { get; set; }

        public byte[] FileBytes
        {
            get
            {
                if (string.IsNullOrWhiteSpace(File)) return new byte[0];
                return Convert.FromBase64String(File);
            }
        }
    }
}
