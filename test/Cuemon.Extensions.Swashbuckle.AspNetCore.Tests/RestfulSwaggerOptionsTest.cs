using System;
using Cuemon.Extensions.Xunit;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.Extensions.Swashbuckle.AspNetCore
{
    public class RestfulSwaggerOptionsTest : Test
    {
        public RestfulSwaggerOptionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void RestfulSwaggerOptions_OpenApiInfoIsNull_ShouldThrowInvalidOperationException()
        {
            var sut1 = new RestfulSwaggerOptions()
            {
                OpenApiInfo = null
            };
            var sut2 = Assert.Throws<InvalidOperationException>(() => sut1.ValidateOptions());
            var sut3 = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut1, nameof(sut1)));

            Assert.Equal("Operation is not valid due to the current state of the object. (Expression 'OpenApiInfo == null')", sut2.Message);
            Assert.Equal("RestfulSwaggerOptions are not in a valid state. (Parameter 'sut1')", sut3.Message);
            Assert.IsType<InvalidOperationException>(sut3.InnerException);
        }

        [Fact]
        public void RestfulSwaggerOptions_XmlDocumentationsIsNull_ShouldThrowInvalidOperationException()
        {
            var sut1 = new RestfulSwaggerOptions()
            {
                XmlDocumentations = null
            };
            var sut2 = Assert.Throws<InvalidOperationException>(() => sut1.ValidateOptions());
            var sut3 = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut1, nameof(sut1)));

            Assert.Equal("Operation is not valid due to the current state of the object. (Expression 'XmlDocumentations == null')", sut2.Message);
            Assert.Equal("RestfulSwaggerOptions are not in a valid state. (Parameter 'sut1')", sut3.Message);
            Assert.IsType<InvalidOperationException>(sut3.InnerException);
        }

        [Fact]
        public void RestfulSwaggerOptions_SettingsIsNull_ShouldThrowInvalidOperationException()
        {
            var sut1 = new RestfulSwaggerOptions()
            {
                Settings = null
            };
            var sut2 = Assert.Throws<InvalidOperationException>(() => sut1.ValidateOptions());
            var sut3 = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut1, nameof(sut1)));

            Assert.Equal("Operation is not valid due to the current state of the object. (Expression 'Settings == null')", sut2.Message);
            Assert.Equal("RestfulSwaggerOptions are not in a valid state. (Parameter 'sut1')", sut3.Message);
            Assert.IsType<InvalidOperationException>(sut3.InnerException);
        }

        [Fact]
        public void RestfulSwaggerOptions_ShouldHaveDefaultValues()
        {
            var sut = new RestfulSwaggerOptions();

            Assert.False(sut.IncludeControllerXmlComments);
            Assert.NotNull(sut.XmlDocumentations);
            Assert.NotNull(sut.OpenApiInfo);
            Assert.NotNull(sut.Settings);
        }
    }
}
