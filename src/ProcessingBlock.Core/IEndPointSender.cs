using System;
using System.Threading;
using System.Threading.Tasks;

namespace ProcessingBlock.Core
{
    public interface IEndPointSender<T>:IStatus
    {
        Task SendAsync(T value, CancellationToken token);
        Task Init();

        Task Close();
    }
}
