using System;
using System.Linq;
using System.Threading;
using System.Xml;
using Cuemon.Extensions.Xunit;
using Cuemon.IO;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.Xml.Serialization.Formatters
{
    public class XmlFormatterTest : Test
    {
        public XmlFormatterTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Serialize_ShouldSerializeUsingExceptionConverter()
        {
            try
            {
                throw new OutOfMemoryException("First", new AggregateException(new AccessViolationException("I1"), new AbandonedMutexException("I2"), new ArithmeticException("I3")));
            }
            catch (Exception e)
            {
                e.Data.Add("Cuemon", "XmlFormatterTest");
                var f = new XmlFormatter(o =>
                {
                    o.IncludeExceptionStackTrace = true;
                    o.Settings.Writer.Indent = true;
                });
                var r = f.Serialize(e);
                var x = new XmlDocument();
                x.Load(r);

                Assert.Contains(e.Data.Keys.Cast<string>(), s => s.Equals("Cuemon"));
                Assert.Contains(e.Data.Values.Cast<string>(), s => s.Equals("XmlFormatterTest"));
                Assert.Equal("<?xml version=\"1.0\" encoding=\"utf-8\"?>", x.FirstChild.OuterXml);
                Assert.Contains("<OutOfMemoryException namespace=\"System\">", x.OuterXml);
                Assert.Contains("<Source>Cuemon.Xml.Tests</Source>", x.OuterXml);
                Assert.Contains("<Message>First</Message>", x.OuterXml);
                Assert.Contains("<Stack>", x.OuterXml);
                Assert.Contains("<Frame>at Cuemon.Xml.Serialization.Formatters.XmlFormatterTest", x.OuterXml);
                Assert.Contains("<Cuemon>XmlFormatterTest</Cuemon>", x.OuterXml);
                Assert.Contains("<AggregateException namespace=\"System\">", x.OuterXml);
                Assert.Contains("<AccessViolationException namespace=\"System\">", x.OuterXml);
                Assert.Contains("<AbandonedMutexException namespace=\"System.Threading\">", x.OuterXml);
                Assert.Contains("<ArithmeticException namespace=\"System\">", x.OuterXml);

                TestOutput.WriteLine(Decorator.Enclose(r).ToEncodedString());
                r.Dispose();
            }
        }
    }
}