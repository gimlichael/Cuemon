using System;
using System.Linq;
using System.Reflection;
using Cuemon.Extensions.IO;
using Cuemon.Extensions.Reflection.Assets;
using Cuemon.Extensions.Xunit;
using Cuemon.Reflection;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.Extensions.Reflection
{
    public class TypeExtensionsTest : Test
    {
        public TypeExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void GetEmbeddedResources_ShouldReadErTextFile()
        {
            var sut1 = this.GetType().GetEmbeddedResources("Cuemon.Extensions.Reflection.Assets.ER.txt", ManifestResourceMatch.Name);
            var sut2 = this.GetType().GetEmbeddedResources("xt", ManifestResourceMatch.ContainsExtension);
            var sut3 = this.GetType().GetEmbeddedResources(".txt", ManifestResourceMatch.Extension);
            var sut4 = this.GetType().GetEmbeddedResources("ER", ManifestResourceMatch.ContainsName);
            var sut5 = this.GetType().GetEmbeddedResources("Cuemon.Extensions.Reflection.Assets.ER.xxx", ManifestResourceMatch.Name);
            var sut6 = this.GetType().GetEmbeddedResources("xx", ManifestResourceMatch.ContainsExtension);
            var sut7 = this.GetType().GetEmbeddedResources(".xxx", ManifestResourceMatch.Extension);
            var sut8 = this.GetType().GetEmbeddedResources("XX", ManifestResourceMatch.ContainsName);

            Assert.True(sut1.Count == 1, "sut1.Count == 1");
            Assert.True(sut2.Count == 1, "sut2.Count == 1");
            Assert.True(sut3.Count == 1, "sut3.Count == 1");
            Assert.True(sut4.Count == 1, "sut4.Count == 1");
            Assert.True(sut1.Values.Single().ToEncodedString().Equals("Cuemon"));
            Assert.True(sut2.Values.Single().ToEncodedString().Equals("Cuemon"));
            Assert.True(sut3.Values.Single().ToEncodedString().Equals("Cuemon"));
            Assert.True(sut4.Values.Single().ToEncodedString().Equals("Cuemon"));

            Assert.True(sut5.Count == 0, "sut1.Count == 0");
            Assert.True(sut6.Count == 0, "sut2.Count == 0");
            Assert.True(sut7.Count == 0, "sut3.Count == 0");
            Assert.True(sut8.Count == 0, "sut4.Count == 0");
        }

        [Fact]
        public void GetRuntimePropertiesExceptOf_ShouldRetrieveRuntimePropertiesExceptTheSpecifiedExclusions()
        {
            var sut1 = new CustomException();
            var sut2 = sut1.GetType().GetRuntimePropertiesExceptOf<Exception>().ToList();
            var sut3 = sut1.GetType().GetRuntimePropertiesExceptOf<AggregateException>().ToList();
            var sut4 = sut1.GetType().GetRuntimeProperties().ToList();

            foreach (var pi in sut2)
            {
                TestOutput.WriteLine(pi.Name);
            }

            TestOutput.WriteLine(" --- ");

            foreach (var pi in sut3)
            {
                TestOutput.WriteLine(pi.Name);
            }

            TestOutput.WriteLine(" --- ");

            Assert.True(sut2.Count == 5, "sut2.Count == 5"); // with .net 6 ms decided to add two extra internals; InnerExceptionCount and InternalInnerExceptions (yup; ugly)
            Assert.True(sut3.Count == 2, "sut2.Count == 2");
            Assert.Equal(13, sut4.Count);

            foreach (var pi in sut4)
            {
                TestOutput.WriteLine(pi.Name);
            }

            Assert.Collection(sut2,
                pi => Assert.Equal("Code", pi.Name),
                pi => Assert.Equal("CodePhrase", pi.Name),
                pi => Assert.Equal("InnerExceptions", pi.Name),
                pi => Assert.Equal("InnerExceptionCount", pi.Name),
                pi => Assert.Equal("InternalInnerExceptions", pi.Name));

            Assert.Collection(sut3, 
                pi => Assert.Equal("Code", pi.Name),
                pi => Assert.Equal("CodePhrase", pi.Name));

            Assert.Collection(sut4,
                pi => Assert.Equal("Code", pi.Name),
                pi => Assert.Equal("CodePhrase", pi.Name),
                pi => Assert.Equal("InnerExceptions", pi.Name),
                pi => Assert.Equal("Message", pi.Name),
                pi => Assert.Equal("InnerExceptionCount", pi.Name),
                pi => Assert.Equal("InternalInnerExceptions", pi.Name),
                pi => Assert.Equal("TargetSite", pi.Name),
                pi => Assert.Equal("Data", pi.Name),
                pi => Assert.Equal("InnerException", pi.Name),
                pi => Assert.Equal("HelpLink", pi.Name),
                pi => Assert.Equal("Source", pi.Name),
                pi => Assert.Equal("HResult", pi.Name),
                pi => Assert.Equal("StackTrace", pi.Name));
        }

        [Fact]
        public void ToFullNameIncludingAssemblyName_ShouldWriteFullTypeNameIncludingAssemblyName()
        {
            var sut1 = this.GetType();
            var sut2 = sut1.ToFullNameIncludingAssemblyName();

            TestOutput.WriteLine(sut2);

            Assert.Equal("Cuemon.Extensions.Reflection.TypeExtensionsTest, Cuemon.Extensions.Reflection.Tests", sut2);
        }
    }
}