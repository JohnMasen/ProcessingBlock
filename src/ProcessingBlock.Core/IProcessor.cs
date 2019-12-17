using System;
using System.Collections.Generic;
using System.Text;

namespace ProcessingBlock.Core
{
    public interface IProcessor<TPara,TResult>:IIDNameObject,IEndPointReceiverHost<TPara>,IEndPointSenderHost<TResult>
    {

        void Start();
        void Stop();
    }
}
