using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProcessingBlock.Core
{
    public class QueueEndPointReceiverBase<T> : IEndPointReceiver<T>
    {
        private BlockingCollection<T> queue = new BlockingCollection<T>();
        public WaitHandle BusyWaitHandle { get; } = new ManualResetEvent(false);

        public StatusEnum Status { get; private set; } = StatusEnum.Idle;

        protected void add(T para)
        {
            queue.Add(para);
        }
        protected void complete()
        {
            queue.CompleteAdding();
        }

        public async IAsyncEnumerable<T> GetResultsAsync([EnumeratorCancellation] CancellationToken token)
        {
            token.Register(() =>
            {
                Status = StatusEnum.Idle;
                (BusyWaitHandle as ManualResetEvent).Set();
                queue.CompleteAdding();
            });

            (BusyWaitHandle as ManualResetEvent).Reset();
            Status = StatusEnum.Busy;
            foreach (var item in queue.GetConsumingEnumerable())
            {
                yield return item;
            }
            //if token cancellation is triggered, the following code will NOT run
            Status = StatusEnum.Idle;
            (BusyWaitHandle as ManualResetEvent).Set();
            queue.CompleteAdding();

        }

        public virtual Task Init()
        {
            return Task.CompletedTask;
        }
    }
}
