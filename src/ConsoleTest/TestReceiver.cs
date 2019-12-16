using ProcessingBlock.Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleTest
{
    class TestReceiver<T> : IEndPointReceiver<T>
    {
        private BlockingCollection<T> q = new BlockingCollection<T>();
        public WaitHandle BusyWaitHandle { get; }= new ManualResetEvent(false);

        public StatusEnum Status { get; private set; } = StatusEnum.Idle;

        public void Run(T para)
        {
            q.Add(para);
        }

        public async IAsyncEnumerable<T> GetResultsAsync([EnumeratorCancellation] CancellationToken token)
        {
            token.Register(() =>
            {
                Status = StatusEnum.Idle;
                (BusyWaitHandle as ManualResetEvent).Set();
                q.CompleteAdding();
            });
            (BusyWaitHandle as ManualResetEvent).Reset();
            Status = StatusEnum.Busy;
            foreach (var item in q.GetConsumingEnumerable())
            {
                yield return item;
            }
            
        }
    }
}
