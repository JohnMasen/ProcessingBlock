using ProcessingBlock.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProcessingBlock.UnitTest
{
    class AddFunctionProcessor : SimpleProcessorBase<int, int>
    {
        protected override Task<int> ProcessSingle(int para)
        {
            return Task.FromResult(para + 1);
        }
    }
}
