using System;

namespace Cuemon.Core.Assets
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
