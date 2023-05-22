using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace Extend
{
    public static class Extensions
    {
        public static byte[] Serialize<T>(this T data)
        {
            var serializer = new DataContractJsonSerializer(typeof(T));

            using (var ms = new MemoryStream())
            {
                serializer.WriteObject(ms, data);
                return ms.ToArray();
            }
        }

        public static T Deserialize<T>(this byte[] data)
        {
            var serializer = new DataContractJsonSerializer(typeof(T));

            using (var ms = new MemoryStream(data))
            {
                return (T)serializer.ReadObject(ms);
            }
        }
    }
}
