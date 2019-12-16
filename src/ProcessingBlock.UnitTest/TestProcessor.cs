using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProcessingBlock.UnitTest
{
    class TestProcessor<T> : ProcessingBlock.Core.SimpleProcessorBase<T, T>
    {

        Func<T, T> func;
        public TestProcessor(Func<T, T> callback)
        {
            func = callback;
        }
        protected override Task<T> ProcessSingle(T para)
        {
            return Task.FromResult(func(para));
        }
    }
}
