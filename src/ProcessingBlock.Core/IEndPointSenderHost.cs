using System;
using System.Collections.Generic;
using System.Text;

namespace ProcessingBlock.Core
{
    public interface IEndPointSenderHost<T>:IIDNameObject
    {
        IEndPointSender<T> Sender { get; set; }
    }
}
