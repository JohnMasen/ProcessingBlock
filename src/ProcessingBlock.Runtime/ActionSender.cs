using ProcessingBlock.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProcessingBlock.Runtime
{
    public class ActionSender<T> : EndPointSenderBase<T>
    {
        private Func<T, CancellationToken,Task> Action;

        public ActionSender(Action<T> a)
        {
            Action = (x,token) =>
              {
                  a(x);
                  return Task.CompletedTask;
              };
        }

        public ActionSender(Func<T,CancellationToken,Task> a)
        {
            Action = a;
        }

        protected override Task onSendAsync(T value, CancellationToken token)
        {
            return Action(value,token);
        }
        
    }
}
