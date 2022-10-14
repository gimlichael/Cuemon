using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using Cuemon.Extensions.IO;
using Cuemon.Extensions.Xunit;
using Newtonsoft.Json.Serialization;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.Extensions.Newtonsoft.Json.Formatters
{
    public class JsonFormatterTest : Test
    {
        public JsonFormatterTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void DeserializeObject_ShouldBeEquivalentToOriginal_BindingFlags()
        {
            var sut1 = BindingFlags.DeclaredOnly;

            TestOutput.WriteLine(sut1.ToString());

            var json = JsonFormatter.SerializeObject(sut1);

            TestOutput.WriteLine(json.ToEncodedString(o => o.LeaveOpen = true));

            var sut2 = JsonFormatter.DeserializeObject<BindingFlags>(json);

            Assert.Equal(sut1, sut2);
        }

        [Fact]
        public void DeserializeObject_ShouldBeEquivalentToOriginal_UriScheme()
        {
            var sut1 = UriScheme.Https;

            TestOutput.WriteLine(sut1.ToString());

            var json = JsonFormatter.SerializeObject(sut1);

            TestOutput.WriteLine(json.ToEncodedString(o => o.LeaveOpen = true));

            var sut2 = JsonFormatter.DeserializeObject<UriScheme>(json);

            Assert.Equal(sut1, sut2);
        }

        [Fact]
        public void DeserializeObject_ShouldBeEquivalentToOriginal_TimeSpan()
        {
            var sut1 = TimeSpan.Parse("01:12:05");

            TestOutput.WriteLine(sut1.ToString());

            var json = JsonFormatter.SerializeObject(sut1);

            TestOutput.WriteLine(json.ToEncodedString(o => o.LeaveOpen = true));

            var sut2 = JsonFormatter.DeserializeObject<TimeSpan>(json);

            Assert.Equal(sut1, sut2);
        }

        [Fact]
        public void SerializeObject_ShouldBeEquivalentToOriginal_String()
        {
            var sut1 = "\"01:12:05\"";

            TestOutput.WriteLine(sut1);

            var timeSpan = JsonFormatter.DeserializeObject<TimeSpan>(sut1.ToStream());

            TestOutput.WriteLine(timeSpan.ToString());

            var sut2 = JsonFormatter.SerializeObject(timeSpan);

            Assert.Equal(sut1, sut2.ToEncodedString());
        }

        [Fact]
        public void Deserialize_ShouldBeEquivalentToOriginal_DateTime()
        {
            var sut = DateTime.Parse("2022-06-26T22:39:14.3512950Z").ToUniversalTime();
            TestOutput.WriteLine(sut.ToString("O"));

            var formatter = new JsonFormatter(o => o.Settings.DateFormatString = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffffffK");
            var serializedStream = formatter.Serialize(sut);

            var sutAsIso8601String = serializedStream.ToEncodedString(o => o.LeaveOpen = true);

            TestOutput.WriteLine(sutAsIso8601String); // note: trailing zeros stay true the formatter specification and do not omit anything

            var deserializedDt = formatter.Deserialize<DateTime>(serializedStream);

            TestOutput.WriteLine(deserializedDt.ToString("O"));

            Assert.Equal(@$"""{sut.ToString("O")}""", sutAsIso8601String);
            Assert.Equal(sut, deserializedDt);
        }

        [Fact]
        public void Serialize_ShouldSerializeUsingExceptionConverter_WithPascalCase()
        {
            try
            {
                throw new OutOfMemoryException("First", new AggregateException(new AccessViolationException("I1"), new AbandonedMutexException("I2"), new ArithmeticException("I3")));
            }
            catch (Exception e)
            {
                e.Data.Add("Cuemon", "XmlFormatterTest");
                var f = new JsonFormatter(o =>
                {
                    o.Settings.ContractResolver = new DefaultContractResolver();
                    o.IncludeExceptionStackTrace = true;
                });
                var r = f.Serialize(e);

                TestOutput.WriteLine(r.ToEncodedString(o => o.LeaveOpen = true));

                var x = new StreamReader(r).ReadAllLines().ToList();
                Assert.Contains(e.Data.Keys.Cast<string>(), s => s.Equals("Cuemon"));
                Assert.Contains(e.Data.Values.Cast<string>(), s => s.Equals("XmlFormatterTest"));
                Assert.Equal("{", x[0]);
                Assert.Contains("\"Type\": \"System.OutOfMemoryException\"", x[1]);
                Assert.Contains("\"Source\": \"Cuemon.Extensions.Newtonsoft.Json.Tests\"", x[2]);
                Assert.Contains("\"Message\": \"First\"", x[3]);
                Assert.Contains("\"Stack\": [", x[4]);
                Assert.Contains("at Cuemon.Extensions.Newtonsoft.Json.Formatters.JsonFormatterTest", x[5]);
                Assert.Contains("\"Data\": {", x[7]);
                Assert.Contains("\"Cuemon\": \"XmlFormatterTest\"", x[8]);
                Assert.Contains("},", x[9]);
                Assert.Contains("\"Inner\": {", x[10]);
                Assert.Contains("\"Type\": \"System.AggregateException\",", x[11]);
                Assert.Contains("\"Type\": \"System.AccessViolationException\"", x[14]);
                Assert.Contains("\"Type\": \"System.Threading.AbandonedMutexException\"", x[17]);
                Assert.Contains("\"Type\": \"System.ArithmeticException\"", x[21]);

                r.Dispose();
            }
        }
    }
}