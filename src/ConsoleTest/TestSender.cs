using ProcessingBlock.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleTest
{
    class TestSender<T> :EndPointSenderBase<T>
    {

        protected override Task onSendAsync(T value, CancellationToken token)
        {
            Console.WriteLine($"Result is {value}");
            return Task.CompletedTask;
        }
        
    }
}
