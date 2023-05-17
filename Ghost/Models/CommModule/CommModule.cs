using Ghost.Models.Tasks;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ghost.Models.CommModule
{
    public abstract class CommModule
    {
        public abstract Task Start();
        public abstract void Stop();

        protected GhostMetadata Metadata { get; set; }

        protected ConcurrentQueue<GhostTask> Inbound = new ConcurrentQueue<GhostTask>();
        protected ConcurrentQueue<GhostTaskResult> Outbound = new ConcurrentQueue<GhostTaskResult>();

        public virtual void Init(GhostMetadata metadata)
        {
            Metadata= metadata;
        }

        public bool RecvData(out IEnumerable<GhostTask> tasks)
        {
            if (Inbound.IsEmpty)
            {
                tasks = null;
                return false;
            }

            var list = new List<GhostTask>();

            while (Inbound.TryDequeue(out var task))
            {
                list.Add(task);
            }

            tasks = list;
            return true;
        }

        public void SendData(GhostTaskResult result)
        {
            Outbound.Enqueue(result);
        }

        protected IEnumerable<GhostTaskResult> GetOutbound()
        {
            var outbound = new List<GhostTaskResult>();

            while (Outbound.TryDequeue(out var result))
            {
                outbound.Add(result);
            }

            return outbound;
        }
    }
}
