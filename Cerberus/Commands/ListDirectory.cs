using Agent;
using Cerberus.Models.Tasks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Cerberus.Commands
{
    public class ListDirectory : CerberusCommand
    {
        public override string Name => "ls";

        public override string Execute(MythicTask task)
        {
            var results = new SharpSploitResultList<ListDirectoryResult>();
            var Arguments = Encoding.UTF8.GetBytes(task.parameters).Deserialize<LsParam>();

            if (Arguments is null || string.IsNullOrWhiteSpace(Arguments.path))
            {
                Arguments.path = Directory.GetCurrentDirectory();
            }

            var files = Directory.GetFiles(Arguments.path);

            foreach (var file in files)
            {
                var fileInfo = new FileInfo(file);
                results.Add(new ListDirectoryResult
                {
                    Name = fileInfo.FullName,
                    Length = fileInfo.Length
                });
            }

            var directories = Directory.GetDirectories(Arguments.path);

            foreach (var directory in directories)
            {
                var dirInfo = new DirectoryInfo(directory);
                results.Add(new ListDirectoryResult
                {
                    Name = dirInfo.Name,
                    Length = 0
                });
            }

            return results.ToString();
        }

        public sealed class ListDirectoryResult : SharpSploitResult
        {
            public string Name { get; set; }
            public long Length { get; set; }

            protected internal override IList<SharpSploitResultProperty> ResultProperties => new List<SharpSploitResultProperty>
            {
                new SharpSploitResultProperty{Name = nameof(Name), Value = Name},
                new SharpSploitResultProperty{Name = nameof(Length), Value = Length}
            };
        }

        public class LsParam
        {
            public string path { get; set; }
        }
    }
}
