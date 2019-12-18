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

        public static IProcessor<T, TResult> Chain<T,TResult>(this IEndPointSenderHost<T> source, IProcessor<T,TResult> target)
        {
            ProcessorManager.Default.Chain(source, target);
            return target;
        }

        public static void WithStartValue<T>(this IEndPointReceiverHost<T>  receiverHost, IEnumerable<T> sourceList)
        {
            ManualReceiver<T> r= new ManualReceiver<T>();
            receiverHost.Receiver = r;
            foreach (var item in sourceList)
            {
                r.Add(item);
            }
        }
    }
}
