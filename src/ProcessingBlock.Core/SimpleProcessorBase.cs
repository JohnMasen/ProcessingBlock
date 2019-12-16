﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProcessingBlock.Core
{
    public abstract class SimpleProcessorBase<TPara, TResult> : ProcessorBase<TPara, TResult>
    {
        public SimpleProcessorBase(IEndPointReceiver<TPara> receiver, IEndPointSender<TResult> sender):base(receiver, sender)
        {

        }
        public SimpleProcessorBase()
        {

        }
        protected override async Task Process(TPara para, IEndPointSender<TResult> resultHandler, CancellationToken token)
        {
            await resultHandler.SendAsync(await ProcessSingle(para),token);
        }

        protected abstract Task<TResult> ProcessSingle(TPara para);
    }
}
