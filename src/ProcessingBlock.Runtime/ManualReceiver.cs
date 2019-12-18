using ProcessingBlock.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProcessingBlock.Runtime
{
    public class ManualReceiver<T>:QueueEndPointReceiverBase<T>
    {
        public void Add(T value)
        {
            add(value);
        }

        public void Complete()
        {
            complete();
        }

    }
}
