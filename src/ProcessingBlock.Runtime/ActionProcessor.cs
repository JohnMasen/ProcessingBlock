using ProcessingBlock.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProcessingBlock.Runtime
{
    public abstract class ActionProcessor<TPara> : ProcessorBase<TPara, object>
    {

        private readonly Func<TPara, Task> a;

        public ActionProcessor(Action<TPara> action)
        {
            a = x=>
            {
                action(x);
                return Task.CompletedTask;
            };
        }

        public ActionProcessor(Func<TPara, Task> action)
        {
            a = action;
        }
        protected override Task Process(TPara para, IEndPointSender<object> resultHandler, CancellationToken token)
        {
            return a(para);
        }


    }
}
