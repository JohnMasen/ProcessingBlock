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
        private IEndPointSender<TResult> _sender;

        public IEndPointSender<TResult> Sender 
        {
            get { return _sender; }
            set
            {
                if (Status == StatusEnum.Busy)
                {
                    throw new InvalidOperationException("Cannot change NextEndPoint while processor is busy.");
                }
                _sender = value;
            }
        }

        private IEndPointReceiver<TPara> _receiver;

        public IEndPointReceiver<TPara> Receiver
        {
            get { return _receiver; }
            set
            {
                if (Status == StatusEnum.Busy)
                {
                    throw new InvalidOperationException("Cannot change CurrentEndPoint while processor is busy");
                }
                _receiver = value;
            }
        }
        public WaitHandle BusyWaitHandle { get; } = new ManualResetEvent(false);

        public StatusEnum Status { get; private set; }

        public Guid ID { get; } = Guid.NewGuid();

        public string Name { get; set; }
        #endregion



        public ProcessorBase()
        {
            Status = StatusEnum.Idle;
        }

        public void Start()
        {
            if (Status==StatusEnum.Busy)
            {
                return;
            }
            if (Receiver == null)
            {
                throw new InvalidOperationException("Receiver cannot be null");
            }
            try
            {
                Receiver.Init();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to init Receiver", ex);
            }
            if (Sender==null)
            {
                throw new InvalidOperationException("Sender cannot be null");
            }
            try
            {
                Sender.Init();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to init Sender",ex);
            }
            Status = StatusEnum.Busy;
            (BusyWaitHandle as ManualResetEvent).Reset();
            Task.Factory.StartNew(async () =>
            {
                try
                {
                    await foreach (var item in Receiver.GetResultsAsync(cts.Token))
                    {
                        await Process(item, Sender, cts.Token);
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
                    await Sender.Close();
                    Status = StatusEnum.Idle;
                    (BusyWaitHandle as ManualResetEvent).Set();
                }


            }, cts.Token);

        }

        protected abstract Task Process(TPara para, IEndPointSender<TResult> resultHandler, CancellationToken token);

        public void Stop()
        {
            if (Status==StatusEnum.Idle)
            {
                return;
            }
            cts.Cancel();
            WaitUnitlShutdown();
        }

        public void WaitUnitlShutdown()
        {
            BusyWaitHandle.WaitOne();
            Receiver.BusyWaitHandle.WaitOne();
            Sender.BusyWaitHandle.WaitOne();
            //WaitHandle.WaitAll(new WaitHandle[] { BusyWaitHandle, Receiver.BusyWaitHandle, Sender.BusyWaitHandle });
        }
    }
}
