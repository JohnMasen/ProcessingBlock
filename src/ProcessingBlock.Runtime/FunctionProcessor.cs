using ProcessingBlock.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProcessingBlock.Runtime
{
    public class FunctionProcessor<TPara, TResult> : SimpleProcessorBase<TPara, TResult>
    {
        private readonly Func<TPara, Task<TResult>> f;
        public FunctionProcessor(Func<TPara, Task<TResult>> asyncFunction)
        {
            f = asyncFunction;
        }

        public FunctionProcessor(Func<TPara, TResult> function)
        {
            f = x =>
            {
                return Task.FromResult(function(x));
            };
        }
        protected override Task<TResult> ProcessSingle(TPara para)
        {
            if (f==null)
            {
                throw new InvalidOperationException("function not initialized before calling");
            }
            return f(para);
        }
    }
}
