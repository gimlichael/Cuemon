using System;
using System.Linq;
using Cuemon.Extensions.IO;
using Cuemon.Extensions.Reflection;
using Cuemon.Extensions.Xunit;
using Cuemon.Reflection;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.Extensions.Xml
{
    public class StreamExtensionsTest : Test
    {
        public StreamExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void RemoveXmlNamespaceDeclarations_ShouldLoadAndClearAnyNamespaceDeclarations()
        {
            using (var file = typeof(StreamExtensionsTest).GetEmbeddedResources("Namespace.xml", ManifestResourceMatch.ContainsName).Values.Single())
            {
                var original = file.ToEncodedString(o => o.LeaveOpen = true);
                var sanitized = file.RemoveXmlNamespaceDeclarations().ToEncodedString();
                
                TestOutput.WriteLine(original);
                TestOutput.WriteLine("");
                TestOutput.WriteLine(sanitized);

                Assert.Contains("xmlns:h=\"http://www.w3.org/HTML/1998/html4\"", original);
                Assert.Contains("xmlns:xdc=\"http://www.xml.com/books\"", original);
                Assert.Contains("h:body", original);
                Assert.Contains("xdc:bookreview", original);
                Assert.True(original.Length > sanitized.Length, "original.Length > sanitized.Length");
                Assert.DoesNotContain("xmlns:h=\"http://www.w3.org/HTML/1998/html4\"", sanitized);
                Assert.DoesNotContain("xmlns:xdc=\"http://www.xml.com/books\"", sanitized);
                Assert.DoesNotContain("h:body", sanitized);
                Assert.DoesNotContain("xdc:bookreview", sanitized);
            }
        }
    }
}