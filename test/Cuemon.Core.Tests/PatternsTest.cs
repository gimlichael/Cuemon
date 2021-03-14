using System;
using Cuemon.Extensions.Xunit;
using Cuemon.Text;
using Cuemon.Threading;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon
{
    public class PatternsTest : Test
    {
        public PatternsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Configure_ShouldInitializeDefaultInstance()
        {
            Action<AsyncOptions> sut = null;
            var ao = new AsyncOptions();
            
            var options = Patterns.Configure(sut);

            Assert.NotNull(options);
            Assert.IsType<AsyncOptions>(options);
            Assert.Equal(ao.CancellationToken, options.CancellationToken);
        }

        [Fact]
        public void ConfigureExchange_ShouldSwapOptions_VerifyDefaultValues()
        {
            Action<AsyncEncodingOptions> sut1 = null;
            var sut2 = Patterns.ConfigureExchange<AsyncEncodingOptions, EncodingOptions>(sut1);

            var o1 = Patterns.Configure(sut1);
            var o2 = Patterns.Configure(sut2);

            Assert.NotNull(o1);
            Assert.NotNull(o2);
            Assert.IsType<AsyncEncodingOptions>(o1);
            Assert.IsType<EncodingOptions>(o2);
            Assert.Equal(o1.Encoding, o2.Encoding);
            Assert.Equal(o1.Preamble, o2.Preamble);
        }
    }
}