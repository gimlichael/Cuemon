using System;
using Cuemon.Configuration;

namespace Cuemon.Assets
{
    public class FakeOptions : IValidatableParameterObject
    {
        public bool Proceed { get; set; }

        public void ValidateOptions()
        {
            throw new NotImplementedException();
        }
    }
}
