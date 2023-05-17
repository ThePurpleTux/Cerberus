using Cerberus.Models.Tasks;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cerberus.Models.CommModule
{
    public abstract class CommModule
    {
        public abstract Task Start();
        public abstract void Stop();

        protected CerberusMetadata Metadata { get; set; }

        protected ConcurrentQueue<CerberusTask> Inbound = new ConcurrentQueue<CerberusTask>();
        protected ConcurrentQueue<CerberusTaskResult> Outbound = new ConcurrentQueue<CerberusTaskResult>();

        public virtual void Init(CerberusMetadata metadata)
        {
            Metadata= metadata;
        }

        public bool RecvData(out IEnumerable<CerberusTask> tasks)
        {
            if (Inbound.IsEmpty)
            {
                tasks = null;
                return false;
            }

            var list = new List<CerberusTask>();

            while (Inbound.TryDequeue(out var task))
            {
                list.Add(task);
            }

            tasks = list;
            return true;
        }

        public void SendData(CerberusTaskResult result)
        {
            Outbound.Enqueue(result);
        }

        protected IEnumerable<CerberusTaskResult> GetOutbound()
        {
            var outbound = new List<CerberusTaskResult>();

            while (Outbound.TryDequeue(out var result))
            {
                outbound.Add(result);
            }

            return outbound;
        }
    }
}
