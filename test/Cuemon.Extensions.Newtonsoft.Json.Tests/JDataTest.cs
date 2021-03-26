using System;
using System.Linq;
using System.Threading;
using Cuemon.Extensions.IO;
using Cuemon.Extensions.Newtonsoft.Json.Formatters;
using Cuemon.Extensions.Xunit;
using Newtonsoft.Json.Serialization;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.Extensions.Newtonsoft.Json
{
    public class JDataTest : Test
    {
        public JDataTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void ReadAll_PrimitiveValuesShouldBeCompliantWithRfc7159()
        {
            object[] primitives = { "null", 1, 2, 3, 4, 5, 6, 7, 8, 9, "\"cuemon\"", false, true, 100.4 };

            foreach (var p in primitives)
            {
                var f = new JsonFormatter();
                var r = f.Serialize(p);

                var x0 = JData.ReadAll(r, o => o.LeaveOpen = true);
            
                TestOutput.WriteLine(DelimitedString.Create(x0, o => o.Delimiter = Environment.NewLine));
                TestOutput.WriteLine(r.ToEncodedString());
            }
        }

        [Fact]
        public void ReadAll_ShouldReadSimpleObject()
        {
            var e = new ArgumentException("The amazing message of this exception.", "fakeArg");
            var f = new JsonFormatter();
            var r = f.Serialize(e);
            var x0 = JData.ReadAll(r, o => o.LeaveOpen = true);
            
            TestOutput.WriteLine(DelimitedString.Create(x0, o => o.Delimiter = Environment.NewLine));

            TestOutput.WriteLine(r.ToEncodedString());
        }

        [Fact]
        public void ReadAll_ShouldReadSimpleArray()
        {
            var a = Generate.RangeOf(50, i => i);
            var f = new JsonFormatter();
            var r = f.Serialize(a);
            var x0 = JData.ReadAll(r, o => o.LeaveOpen = true).ToList();

            for (var i = 0; i < 50; i++)
            {
                Assert.Equal(Convert.ChangeType(i, x0[i].Type), x0[i].Value); // should be int32 - but Newtonsoft resolves it as int64
            }

            TestOutput.WriteLine(r.ToEncodedString());
        }

        [Fact]
        public void ReadAll_ShouldHaveOuterAndNestedExceptionsBothHierarchyAndFlattened_WithPascalCase()
        {
            var e = new OutOfMemoryException("First", new AggregateException(new AccessViolationException("I1"), new AbandonedMutexException("I2"), new ArithmeticException("I3")));
            var f = new JsonFormatter(o =>
            {
                o.Settings.ContractResolver = new DefaultContractResolver();
            });
            var r = f.Serialize(e);
            var x0 = JData.ReadAll(r, o => o.LeaveOpen = true);
            var x1 = x0.Last(r => r.Children.Any()).Children;
            var x2 = x1.Last(r => r.Children.Any()).Children;
            var x3 = x2.Last(r => r.Children.Any()).Children;
            var x4 = x3.Last(r => r.Children.Any()).Children;
            var xFlat = x0.Flatten().ToList();

            TestOutput.WriteLine(DelimitedString.Create(xFlat, o => o.Delimiter = Environment.NewLine));
            TestOutput.WriteLine("");
            TestOutput.WriteLine(r.ToEncodedString());

            Assert.Equal(3, x0.Count());
            Assert.Equal(3, x1.Count);
            Assert.Equal(3, x2.Count);
            Assert.Equal(4, x3.Count);
            Assert.Equal(2, x4.Count);

            Assert.Equal("System.OutOfMemoryException", x0.Single(result => result.PropertyName == "Type").Value);
            Assert.Equal("First", x0.Single(result => result.PropertyName == "Message").Value);
            Assert.Equal("System.AggregateException", x1.Single(result => result.PropertyName == "Type").Value);
            Assert.Equal("One or more errors occurred. (I1) (I2) (I3)", x1.Single(result => result.PropertyName == "Message").Value);
            Assert.Equal("System.AccessViolationException", x2.Single(result => result.PropertyName == "Type").Value);
            Assert.Equal("I1", x2.Single(result => result.PropertyName == "Message").Value);
            Assert.Equal("System.Threading.AbandonedMutexException", x3.Single(result => result.PropertyName == "Type").Value);
            Assert.Equal("I2", x3.Single(result => result.PropertyName == "Message").Value);
            var x3jdr = x3.Single(result => result.PropertyName == "MutexIndex");
            Assert.Equal(Convert.ChangeType(-1, x3jdr.Type), x3jdr.Value); // should be int32 - but Newtonsoft resolves it as int64
            Assert.Equal("System.ArithmeticException", x4.Single(result => result.PropertyName == "Type").Value);
            Assert.Equal("I3", x4.Single(result => result.PropertyName == "Message").Value);

            Assert.Equal("System.OutOfMemoryException", xFlat[0].Value);
            Assert.Equal("First", xFlat[1].Value);
            Assert.Equal("System.AggregateException", xFlat[3].Value);
            Assert.Equal("One or more errors occurred. (I1) (I2) (I3)", xFlat[4].Value);
            Assert.Equal("System.AccessViolationException", xFlat[6].Value);
            Assert.Equal("I1", xFlat[7].Value);
            Assert.Equal("System.Threading.AbandonedMutexException", xFlat[9].Value);
            Assert.Equal("I2", xFlat[10].Value);
            Assert.Equal(Convert.ChangeType(-1, xFlat[11].Type), xFlat[11].Value); // should be int32 - but Newtonsoft resolves it as int64
            Assert.Equal("System.ArithmeticException", xFlat[13].Value);
            Assert.Equal("I3", xFlat[14].Value);
        }
    }
}