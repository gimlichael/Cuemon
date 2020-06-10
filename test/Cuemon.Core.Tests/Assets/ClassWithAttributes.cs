using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Cuemon.Core.Tests.Assets
{
    [DataContract]
    public class ClassWithAttributes
    {
        [DataMember]
        public int Id { get; set; }

        [Obsolete]
        public string Name { get; set; }
    }
}