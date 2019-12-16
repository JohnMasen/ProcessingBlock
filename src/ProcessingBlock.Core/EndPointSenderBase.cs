using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProcessingBlock.Core
{
    public abstract class EndPointSenderBase<T> : IEndPointSender<T>
    {
        public WaitHandle BusyWaitHandle => mre;


        private StatusEnum statusEnum;

        private ManualResetEvent mre = new ManualResetEvent(false);
        public StatusEnum Status
        {
            get { return statusEnum; }
            private set 
            {
                switch (value)
                {
                    case StatusEnum.Idle:
                        mre.Set();
                        break;
                    case StatusEnum.Busy:
                        mre.Reset();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                statusEnum = value; 
            }
        }


        public async Task SendAsync(T value, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            Status = StatusEnum.Busy;
            try
            {
                await onSendAsync(value, token);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                Status = StatusEnum.Idle;
            }
        }

        protected abstract Task onSendAsync(T value, CancellationToken token);
        
    }
}
