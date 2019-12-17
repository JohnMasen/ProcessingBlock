using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProcessingBlock.Core
{
    public interface IEndPointReceiver<T>:IStatus
    {
        Task Init();
        IAsyncEnumerable<T> GetResultsAsync(CancellationToken token);
    }
}
