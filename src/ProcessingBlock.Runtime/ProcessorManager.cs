using ProcessingBlock.Core;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ProcessingBlock.Runtime
{
    public class ProcessorManager
    {
        public static ProcessorManager Default { get; private set; } = new ProcessorManager();
        public List<IBinder> Binders { get; private set; } = new List<IBinder>();

        public void Chain<T>(IEndPointSenderHost<T> sender,
                             IEndPointReceiverHost<T> receiver)
        {
            bool bindComplete = false;
            foreach (var item in Binders)
            {
                try
                {
                    bindComplete = item.TryBind(sender, receiver);
                    if (bindComplete)
                    {
                        break;
                    }
                }
                catch (Exception)
                {
                }
            }
            if (!bindComplete)
            {
                bindDefault(sender, receiver);
            }

        }

        private void bindDefault<T>(IEndPointSenderHost<T> sender, IEndPointReceiverHost<T>receiver)
        {
            QueueEndPointSendReceiver<T> tmp = new QueueEndPointSendReceiver<T>();
            sender.Sender = tmp;
            receiver.Receiver = tmp;
        }

        

        //TODO: Chain should create new chain template instead of instance
    }
}
