using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            TestProcessor<int> processor = new TestProcessor<int>(doAdd);
            processor.NextEndPoint = new TestSender<int>();
            TestReceiver<int> receiver = new TestReceiver<int>();
            processor.CurrentEndPoint = receiver;
            processor.Start();
           

            for (int i = 0; i < 10; i++)
            {
                receiver.Run(i);
            }
            
            Console.ReadLine();
            processor.Stop();

        }

        private static int doAdd(int value)
        {
            return value + value;
        }
    }
}
