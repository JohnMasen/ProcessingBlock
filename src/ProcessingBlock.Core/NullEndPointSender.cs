using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProcessingBlock.Core
{
    public class NullEndPointSender<T> : IEndPointSender<T>
    {
        public WaitHandle BusyWaitHandle { get; } = new ManualResetEvent(true);

        public StatusEnum Status { get; } = StatusEnum.Idle;

        public Task Init()
        {
            return Task.CompletedTask;
        }

        public Task SendAsync(T value, CancellationToken token)
        {
            return Task.CompletedTask;
        }

        public Task Close()
        {
            return Task.CompletedTask;
        }
    }
}
