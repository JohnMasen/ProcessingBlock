using ProcessingBlock.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProcessingBlock.UnitTest
{
    class TestSender<T> :EndPointSenderBase<T>
    {
        protected override Task onSendAsync(T value, CancellationToken token)
        {
            Results.Add(value);
            return Task.CompletedTask;
        }

        public List<T> Results { get; private set; } = new List<T>();
    }
}
