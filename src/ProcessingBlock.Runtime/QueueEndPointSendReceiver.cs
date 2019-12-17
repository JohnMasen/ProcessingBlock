using ProcessingBlock.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProcessingBlock.Runtime
{
    public class QueueEndPointSendReceiver<T> : QueueEndPointReceiverBase<T>, IEndPointSender<T>
    {
        public Task SendAsync(T value, CancellationToken token)
        {
            add(value);
            return Task.CompletedTask;
        }

        public Task Close()
        {
            complete();
            return Task.CompletedTask;
        }
    }
}
