using System;
using System.Collections.Generic;
using System.Text;

namespace ProcessingBlock.Runtime
{
    public abstract class ChainHostingBase
    {
        public bool IsStarted { get; private set; } = false;
        public ChainHostingBase Previous { get; protected set; }

        internal abstract void StartProcessor();
    }
}
