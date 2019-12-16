using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ProcessingBlock.Core
{
    public enum StatusEnum
    {
        Idle,
        Busy
    }
    public interface IStatus
    {
        WaitHandle BusyWaitHandle { get; }
        public StatusEnum Status { get;  }
    }
}
