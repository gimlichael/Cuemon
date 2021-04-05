using System;
using System.Xml.Serialization;
using Xunit;

namespace Cuemon.Extensions.Reflection.Assets
{
    [CLSCompliant(true)]
    public class ClassWithAttributeDecorations
    {
        [ContextStatic]
        private int _value = int.MaxValue;

        [XmlElement]
        public int Value => _value;

        public int ValueAlternative { get; } = int.MaxValue;

        [Theory]
        public void Test()
        {

        }
    }
}