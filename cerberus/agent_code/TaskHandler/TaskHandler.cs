using Models.Tasks;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskHandler
{
    public class TaskHandler
    {
        protected static ConcurrentQueue<MythicTask> Inbound = new ConcurrentQueue<MythicTask>();
        protected static ConcurrentQueue<MythicTaskResult> Outbound = new ConcurrentQueue<MythicTaskResult>();

        public static void AddTaskToInbound(MythicTask task)
        {
            Inbound.Enqueue(task);
        }

        public static void AddTaskResultToOutbound(MythicTaskResult result)
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
    }
}
