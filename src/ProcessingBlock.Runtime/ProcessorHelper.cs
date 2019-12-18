using ProcessingBlock.Core;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace ProcessingBlock.Runtime
{
    public static class ProcessorHelper
    {

        public static ResultCollector<T> SetResultCollector<T>(this IEndPointSenderHost<T> senderHost)
        {
            ResultCollector<T> collector = new ResultCollector<T>();
            senderHost.Sender = collector;
            return collector;
        }

        public static ChainHosting<TPara, TResult, TNextResult> CreateChain<TPara, TResult,TNextResult>(this IProcessor<TPara, TResult> processor,IProcessor<TResult,TNextResult> nextProcessor)
        {
            return new ChainHosting<TPara, TPara, TResult>(null, processor, processor).Chain(nextProcessor);
        }

        public static void WithStartValue<T>(this IEndPointReceiverHost<T>  receiverHost, IEnumerable<T> sourceList)
        {
            ManualReceiver<T> r= new ManualReceiver<T>();
            receiverHost.Receiver = r;
            foreach (var item in sourceList)
            {
                r.Add(item);
            }
            r.Complete();
        }
    }
}
