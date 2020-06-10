using System;
using System.Collections.Generic;
using System.Text;

namespace Cuemon.Core.Tests.Assets
{
    public class ClassBase
    {
        public virtual int GetSomeNumber()
        {
            return int.MinValue;
        }

        public virtual Guid Id { get; } = Guid.Empty;
    }
}
