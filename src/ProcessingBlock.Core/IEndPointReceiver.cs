using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace ProcessingBlock.Core
{
    public interface IEndPointReceiver<T>:IStatus
    {
        IAsyncEnumerable<T> GetResultsAsync(CancellationToken token);
    }
}
