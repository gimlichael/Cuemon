using System;
using Asp.Versioning;
using Codebelt.Extensions.Xunit;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.Extensions.Asp.Versioning
{
    public class RestfulApiVersioningOptionsTest : Test
    {
        public RestfulApiVersioningOptionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void RestfulApiVersioningOptions_ParameterNameIsWithWhitespaceOnly_ShouldThrowInvalidOperationException()
        {
            var sut1 = new RestfulApiVersioningOptions
            {
                ParameterName = " "
            };
            var sut2 = Assert.Throws<InvalidOperationException>(() => sut1.ValidateOptions());
            var sut3 = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut1));

            Assert.Equal("Operation is not valid due to the current state of the object. (Expression 'Condition.IsNull(ParameterName) || Condition.IsEmpty(ParameterName) || Condition.IsWhiteSpace(ParameterName)')", sut2.Message);
            Assert.Equal("RestfulApiVersioningOptions are not in a valid state. (Parameter 'sut1')", sut3.Message);
            Assert.IsType<InvalidOperationException>(sut3.InnerException);
        }

        [Fact]
        public void RestfulApiVersioningOptions_ParameterNameIsEmpty_ShouldThrowInvalidOperationException()
        {
            var sut1 = new RestfulApiVersioningOptions
            {
                ParameterName = string.Empty
            };
            var sut2 = Assert.Throws<InvalidOperationException>(() => sut1.ValidateOptions());
            var sut3 = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut1));

            Assert.Equal("Operation is not valid due to the current state of the object. (Expression 'Condition.IsNull(ParameterName) || Condition.IsEmpty(ParameterName) || Condition.IsWhiteSpace(ParameterName)')", sut2.Message);
            Assert.Equal("RestfulApiVersioningOptions are not in a valid state. (Parameter 'sut1')", sut3.Message);
            Assert.IsType<InvalidOperationException>(sut3.InnerException);
        }

        [Fact]
        public void RestfulApiVersioningOptions_ParameterNameIsNull_ShouldThrowInvalidOperationException()
        {
            var sut1 = new RestfulApiVersioningOptions
            {
                ParameterName = null
            };
            var sut2 = Assert.Throws<InvalidOperationException>(() => sut1.ValidateOptions());
            var sut3 = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut1));

            Assert.Equal("Operation is not valid due to the current state of the object. (Expression 'Condition.IsNull(ParameterName) || Condition.IsEmpty(ParameterName) || Condition.IsWhiteSpace(ParameterName)')", sut2.Message);
            Assert.Equal("RestfulApiVersioningOptions are not in a valid state. (Parameter 'sut1')", sut3.Message);
            Assert.IsType<InvalidOperationException>(sut3.InnerException);
        }

        [Fact]
        public void RestfulApiVersioningOptions_ValidAcceptHeadersIsNull_ShouldThrowInvalidOperationException()
        {
            var sut1 = new RestfulApiVersioningOptions
            {
                ValidAcceptHeaders = null
            };
            var sut2 = Assert.Throws<InvalidOperationException>(() => sut1.ValidateOptions());
            var sut3 = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut1));

            Assert.Equal("Operation is not valid due to the current state of the object. (Expression 'ValidAcceptHeaders == null')", sut2.Message);
            Assert.Equal("RestfulApiVersioningOptions are not in a valid state. (Parameter 'sut1')", sut3.Message);
            Assert.IsType<InvalidOperationException>(sut3.InnerException);
        }

        [Fact]
        public void RestfulApiVersioningOptions_ShouldHaveDefaultValues()
        {
            var sut = new RestfulApiVersioningOptions();

            Assert.NotNull(sut.ValidAcceptHeaders);
            Assert.True(typeof(CurrentImplementationApiVersionSelector) == sut.ApiVersionSelectorType, "typeof(CurrentImplementationApiVersionSelector) == sut.ApiVersionSelectorType");
            //Assert.True(typeof(RestfulProblemDetailsFactory) == sut.ProblemDetailsFactoryType, "typeof(RestfulProblemDetailsFactory) == sut.ProblemDetailsFactoryType");
            Assert.Equal(ApiVersion.Default, sut.DefaultApiVersion);
            Assert.Equal("v", sut.ParameterName);
            Assert.NotNull(sut.Conventions);
        }
    }
}
