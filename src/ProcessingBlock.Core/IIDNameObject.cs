using System;
using System.Collections.Generic;
using System.Text;

namespace ProcessingBlock.Core
{
    public interface IIDNameObject
    {
        string Name { get; }
        Guid ID { get; }
    }
}
