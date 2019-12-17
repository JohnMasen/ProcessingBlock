using ProcessingBlock.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProcessingBlock.Runtime
{
    public interface IBinder
    {
        bool TryBind<T>(IEndPointSenderHost<T> sender, IEndPointReceiverHost<T> receiver);
    }
}
