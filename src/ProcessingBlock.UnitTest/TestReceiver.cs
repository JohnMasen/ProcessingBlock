using ProcessingBlock.Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProcessingBlock.UnitTest
{
    class TestReceiver<T>:QueueEndPointReceiverBase<T>
    {
        public void Add(T item)
        {
            add(item);
        }

        public void Complete()
        {
            complete();
        }
    }
}
