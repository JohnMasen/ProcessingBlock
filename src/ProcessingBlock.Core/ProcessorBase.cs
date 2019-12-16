using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProcessingBlock.Core
{
    public abstract class ProcessorBase<TPara, TResult> : IProcessor<TPara, TResult>, IStatus
    {
        private CancellationTokenSource cts = new CancellationTokenSource();

        #region Public properties
        private IEndPointSender<TResult> _next;

        public IEndPointSender<TResult> NextEndPoint
        {
            get { return _next; }
            set
            {
                if (Status == StatusEnum.Busy)
                {
                    throw new InvalidOperationException("Cannot change NextEndPoint while processor is busy.");
                }
                _next = value;
            }
        }

        private IEndPointReceiver<TPara> endPointReceiver;

        public IEndPointReceiver<TPara> CurrentEndPoint
        {
            get { return endPointReceiver; }
            set
            {
                if (Status == StatusEnum.Busy)
                {
                    throw new InvalidOperationException("Cannot change CurrentEndPoint while processor is busy");
                }
                endPointReceiver = value;
            }
        }
        public WaitHandle BusyWaitHandle { get; } = new ManualResetEvent(false);

        public StatusEnum Status { get; private set; }
        #endregion


        public ProcessorBase(IEndPointReceiver<TPara> receiver, IEndPointSender<TResult> sender)
        {
            Status = StatusEnum.Idle;
            NextEndPoint = sender;
            CurrentEndPoint = receiver;
        }
        public ProcessorBase()
        {
            Status = StatusEnum.Idle;
        }

        public void Start()
        {
            Status = StatusEnum.Busy;
            (BusyWaitHandle as ManualResetEvent).Reset();
            Task.Factory.StartNew(async () =>
            {
                try
                {
                    await foreach (var item in CurrentEndPoint.GetResultsAsync(cts.Token))
                    {
                        await Process(item, NextEndPoint, cts.Token);
                    }
                }
                catch (OperationCanceledException)
                {
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    Status = StatusEnum.Idle;
                    (BusyWaitHandle as ManualResetEvent).Set();
                }


            }, cts.Token);

        }

        protected abstract Task Process(TPara para, IEndPointSender<TResult> resultHandler, CancellationToken token);

        public void Stop()
        {
            cts.Cancel();
            //BusyWaitHandle.WaitOne();
            //CurrentEndPoint.BusyWaitHandle.WaitOne();
            //NextEndPoint.BusyWaitHandle.WaitOne();
            WaitHandle.WaitAll(new WaitHandle[] { BusyWaitHandle, CurrentEndPoint.BusyWaitHandle, NextEndPoint.BusyWaitHandle });
        }
    }
}
