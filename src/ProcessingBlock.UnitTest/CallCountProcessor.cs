using ProcessingBlock.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProcessingBlock.UnitTest
{
    class CallCountProcessor<T> : SimpleProcessorBase<T, T>
    {
        public int CallCount { get; private set; } = 0;
        protected override Task<T> ProcessSingle(T para)
        {
            CallCount++;
            return Task.FromResult(para);
        }
    }
}
