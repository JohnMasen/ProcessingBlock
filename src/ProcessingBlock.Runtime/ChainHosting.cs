using ProcessingBlock.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ProcessingBlock.Runtime
{
    public class ChainHosting<TRootPara, TPara, TResult> : ChainHostingBase
    {
        private IProcessor<TPara, TResult> currentprocessor;
        private IEndPointReceiverHost<TRootPara> rootProcessor;

        internal ChainHosting(ChainHostingBase previous, IProcessor<TPara, TResult> processor, IEndPointReceiverHost<TRootPara> root)
        {
            Previous = previous;
            currentprocessor = processor;
            rootProcessor=root;
        }

        public ChainHosting<TRootPara, TResult, TNextResult> Chain<TNextResult>(IProcessor<TResult, TNextResult> processor)
        {
            ProcessorManager.Default.Chain(currentprocessor, processor);
            return new ChainHosting<TRootPara, TResult, TNextResult>(this, processor, rootProcessor);
        }

        internal override void StartProcessor()
        {
            Previous?.StartProcessor();
            currentprocessor.Start();
        }

        public void Start(IEndPointReceiver<TRootPara> receiver, IEndPointSender<TResult> sender)
        {
            if (!canStart)
            {
                throw new InvalidOperationException("at least one of the processors in the chain already started");
            }
            rootProcessor.Receiver = receiver;
            currentprocessor.Sender = sender;
            StartProcessor();
        }

        public (ManualReceiver<TRootPara> trigger, ResultCollector<TResult> resultCollector) Start()
        {
            ManualReceiver<TRootPara> trigger = new ManualReceiver<TRootPara>();
            ResultCollector<TResult> resultCollector = new ResultCollector<TResult>();
            Start(trigger, resultCollector);
            return (trigger, resultCollector);
        }

        private bool canStart => !IsStarted && (Previous == null ? true : !Previous.IsStarted);
    }
}
