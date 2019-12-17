using ProcessingBlock.Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleTest
{
    class TestReceiver<T> :QueueEndPointReceiverBase<T>
    {


        public void Run(T para)
        {
            add(para);
        }

       
    }
}
