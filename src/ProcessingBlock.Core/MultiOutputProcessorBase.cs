using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProcessingBlock.Core
{
    public abstract class MultiOutputProcessorBase<TPara, TResult> : ProcessorBase<TPara, TResult>
    {
        protected override async Task Process(TPara para, IEndPointSender<TResult> resultHandler, CancellationToken token)
        {
            await foreach (var item in ProcessMultiOutput(para,token))
            {
                await resultHandler.SendAsync(item, token);
            }
        }

        protected abstract IAsyncEnumerable<TResult> ProcessMultiOutput(TPara para,CancellationToken token);
    }
}
