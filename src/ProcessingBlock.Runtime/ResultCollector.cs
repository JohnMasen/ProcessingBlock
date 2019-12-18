using ProcessingBlock.Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProcessingBlock.Runtime
{
    public class ResultCollector<T> : EndPointSenderBase<T>
    {
        private BlockingCollection<T> col = new BlockingCollection<T>();
        protected override Task onSendAsync(T value, CancellationToken token)
        {
            col.Add(value);
            return Task.CompletedTask;
        }
        public override Task Close()
        {
            col.CompleteAdding();
            return base.Close();
            
        }

        public Task<T> CollectOneAsync()
        {
            TaskCompletionSource<T> tcs = new TaskCompletionSource<T>();
            Task.Factory.StartNew(() =>
            {
                tcs.SetResult(col.Take());
            });
            return tcs.Task;
        }

        public T CollectOne()
        {
            return col.Take();
        }

        public IEnumerable<T> CollectUntilClose()
        {
            return col.GetConsumingEnumerable();
        }

    }
}
