using ProcessingBlock.Core;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace ProcessingBlock.Runtime
{
    public static class ProcessorHelper
    {
        public static ResultCollector<T> SetResultCollector<T>(this IEndPointSenderHost<T>  senderHost)
        {
            ResultCollector<T> collector = new ResultCollector<T>();
            senderHost.Sender = collector;
            return collector;
        }
    }
}
