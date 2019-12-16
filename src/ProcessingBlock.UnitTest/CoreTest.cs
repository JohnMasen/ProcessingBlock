using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading;

namespace ProcessingBlock.UnitTest
{
    [TestClass]
    public class CoreTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            TestProcessor<int> processor = new TestProcessor<int>(doAdd);
            TestSender<int> sender = new TestSender<int>();
            processor.NextEndPoint = sender;
            TestReceiver<int> receiver = new TestReceiver<int>();
            processor.CurrentEndPoint = receiver;
            processor.Start();
            int[] values,targetvalues;
            values = new int[10];
            targetvalues = new int[10];
            for (int i = 0; i < 10; i++)
            {
                values[i] = i;
                targetvalues[i] = i + i;
            }

            for (int i = 0; i < 10; i++)
            {
                receiver.Run(values[i]);
            }
            receiver.Complete();
            WaitHandle.WaitAll(new WaitHandle[] { processor.BusyWaitHandle, sender.BusyWaitHandle, receiver.BusyWaitHandle },1000);
            Assert.IsTrue(sender.Results.SequenceEqual(targetvalues));
        }

        private  int doAdd(int value)
        {
            return value + value;
        }
    }
}
