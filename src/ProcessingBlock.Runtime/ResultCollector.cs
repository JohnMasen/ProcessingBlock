using ProcessingBlock.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProcessingBlock.Runtime
{
    public class ResultCollector<T> : EndPointSenderBase<T>
    {
        private List<T> result = new List<T>();
        protected override Task onSendAsync(T value, CancellationToken token)
        {
            result.Add(value);
            return Task.CompletedTask;
        }
        public List<T> Collect()
        {
            BusyWaitHandle.WaitOne();
            return result;
        }
    }
}
