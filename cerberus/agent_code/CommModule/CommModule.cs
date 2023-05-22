using Models.Tasks;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommModuleBase
{
    public abstract class CommModule
    {
        public abstract Task Start();
        public abstract void Stop();
        // Make init checkin to mythic and become a callback
        public abstract Task InitCheckin();

        public CerberusMetadata Metadata { get; set; }

        protected ConcurrentQueue<MythicTask> Inbound = new ConcurrentQueue<MythicTask>();
        protected ConcurrentQueue<MythicTaskResult> Outbound = new ConcurrentQueue<MythicTaskResult>();

        public virtual void Init(CerberusMetadata metadata)
        {
            Metadata= metadata;
        }

        public bool RecvData(out IEnumerable<MythicTask> tasks)
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

        public void SendData(MythicTaskResult result)
        {
            Outbound.Enqueue(result);
        }

        protected IEnumerable<MythicTaskResult> GetOutbound()
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
