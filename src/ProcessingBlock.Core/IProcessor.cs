using System;
using System.Collections.Generic;
using System.Text;

namespace ProcessingBlock.Core
{
    public interface IProcessor<TPara,TResult>
    {
        public IEndPointSender<TResult> NextEndPoint { get; set; }
        public IEndPointReceiver<TPara> CurrentEndPoint { get; set; }

        void Start();
        void Stop();
    }
}
