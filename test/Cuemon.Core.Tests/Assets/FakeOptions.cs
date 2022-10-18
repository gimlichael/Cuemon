using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
