using System;
using System.Collections.Generic;
using System.Text;

namespace ProcessingBlock.Core
{
    public interface IConfigable<TConfig>
    {
        void Config(TConfig config);
    }
}
