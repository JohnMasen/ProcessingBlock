using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProcessingBlock.Core;
using ProcessingBlock.Runtime;
using System;
using System.Linq;
using System.Threading;

namespace ProcessingBlock.UnitTest
{
    [TestClass]
    public class BasicTest
    {
        [TestMethod]
        public void SingleCall()
        {
            FunctionProcessor<int,int> processor = new FunctionProcessor<int,int>(doAdd);
            //ResultCollector<int> sender = new ResultCollector<int>();
            //TestSender<int> sender = new TestSender<int>();
            //processor.Sender = sender;
            var collector = processor.SetResultCollector();
            TestReceiver<int> receiver = new TestReceiver<int>();
            processor.Receiver = receiver;
            
            int[] values,targetvalues;
            values = new int[10];
            targetvalues = new int[10];
            for (int i = 0; i < 10; i++)
            {
                values[i] = i;
                targetvalues[i] = doAdd(i);
            }
            for (int i = 0; i < 10; i++)
            {
                receiver.Add(values[i]);
            }
            
            processor.Start();
            receiver.Complete();
            processor.WaitUnitlShutdown();//not necessary, just check the threads won't deadlock
            //WaitHandle.WaitAll(new WaitHandle[] { processor.BusyWaitHandle, sender.BusyWaitHandle, receiver.BusyWaitHandle },1000);
            Assert.IsTrue(collector.Collect().SequenceEqual(targetvalues));
        }
        [TestMethod]
        public void SingleChainCall()
        {
            FunctionProcessor<int, int> p1 = new FunctionProcessor<int, int>(doAdd);
            TestReceiver<int> receiver = new TestReceiver<int>();
            p1.Receiver = receiver;

            CallCountProcessor<int> p2 = new CallCountProcessor<int>();
            p2.Sender = new NullEndPointSender<int>();
            ProcessorManager.Default.Chain(p1, p2);
            int[] values;
            values = new int[10];
            for (int i = 0; i < 10; i++)
            {
                receiver.Add(i);
            }
            
            p1.Start();
            p2.Start();
            receiver.Complete();
            p1.WaitUnitlShutdown();
            p2.WaitUnitlShutdown();
            //WaitHandle.WaitAll(new WaitHandle[] { p1.BusyWaitHandle, p1.Sender.BusyWaitHandle, p1.Receiver.BusyWaitHandle,p2.BusyWaitHandle,p2. }, 1000);
            Assert.AreEqual(p2.CallCount, 10);

        }

        [TestMethod]
        public void ChainAndCollector()
        {
            int[] value = getTestValue();
            FunctionProcessor<int,int> p = new FunctionProcessor<int, int>(doAdd);
            p.WithStartValue(value);
            var p2 = new FunctionProcessor<int, int>(doAdd);
            var result=p
                .Chain(p2)
                .SetResultCollector();
            p.Start();
            p2.Start();
            Assert.IsTrue( result.Collect().SequenceEqual(getTestValue(10, 3)));
        }

        private  int doAdd(int value)
        {
            return value + 1;
        }

        private int[] getTestValue(int count=10,int fillWith=1)
        {
            int[] result = new int[count];
            for (int i = 0; i < count; i++)
            {
                result[i] = fillWith;
            }
            return result;
        }
    }
}
