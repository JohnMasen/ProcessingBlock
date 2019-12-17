using System;
using System.Collections.Generic;
using System.Text;

namespace ProcessingBlock.Core
{
    public interface IEndPointReceiverHost<T>:IIDNameObject
    {
        IEndPointReceiver<T> Receiver { get; set; }
    }
}
