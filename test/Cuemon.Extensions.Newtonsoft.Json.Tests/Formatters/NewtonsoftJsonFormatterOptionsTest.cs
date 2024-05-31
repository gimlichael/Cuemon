using System;
using System.Collections.Generic;
using System.Linq;
using Cuemon.Diagnostics;
using Cuemon.Extensions.Xunit;
using Cuemon.Reflection;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.Extensions.Newtonsoft.Json.Formatters
{
    public class NewtonsoftJsonFormatterOptionsTest : Test
    {
        public NewtonsoftJsonFormatterOptionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void JsonFormatterOptions_SettingsIsNull_ShouldThrowInvalidOperationException()
        {
            var sut1 = new NewtonsoftJsonFormatterOptions()
            {
                Settings = null
            };
            var sut2 = Assert.Throws<InvalidOperationException>(() => sut1.ValidateOptions());
            var sut3 = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut1, nameof(sut1)));

            Assert.Equal("Operation is not valid due to the current state of the object. (Expression 'Settings == null')", sut2.Message);
            Assert.StartsWith($"{nameof(NewtonsoftJsonFormatterOptions)} are not in a valid state.", sut3.Message);
            Assert.Contains("sut1", sut3.Message);
            Assert.IsType<InvalidOperationException>(sut3.InnerException);
        }

        [Fact]
        public void JsonFormatterOptions_SupportedMediaTypesIsNull_ShouldThrowInvalidOperationException()
        {
            var sut1 = new NewtonsoftJsonFormatterOptions()
            {
                SupportedMediaTypes = null
            };
            var sut2 = Assert.Throws<InvalidOperationException>(() => sut1.ValidateOptions());
            var sut3 = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut1, nameof(sut1)));

            Assert.Equal("Operation is not valid due to the current state of the object. (Expression 'SupportedMediaTypes == null')", sut2.Message);
            Assert.StartsWith($"{nameof(NewtonsoftJsonFormatterOptions)} are not in a valid state.", sut3.Message);
            Assert.Contains("sut1", sut3.Message);
            Assert.IsType<InvalidOperationException>(sut3.InnerException);
        }

        [Fact]
        public void JsonFormatterOptions_ShouldHaveDefaultValues()
        {
            var sut = new NewtonsoftJsonFormatterOptions();

            Assert.NotNull(sut.Settings);
            Assert.NotNull(sut.SupportedMediaTypes);
            Assert.Equal(FaultSensitivityDetails.None, sut.SensitivityDetails);
            Assert.False(sut.SynchronizeWithJsonConvert);
        }

        [Fact]
        public void DefaultConverters_ShouldHaveSameAmountOfDefaultConverters()
        {
            var defaultConverters = new List<JsonConverter>();
            NewtonsoftJsonFormatterOptions.DefaultConverters(defaultConverters);

            var x = new NewtonsoftJsonFormatterOptions();
            var y = new NewtonsoftJsonFormatterOptions();
            var bootstrapInvocationList = NewtonsoftJsonFormatterOptions.DefaultConverters.GetInvocationList().Length;

            x.GetType().GetMethod("RefreshWithConverterDependencies", MemberReflection.Everything).Invoke(x, new object[] { });
            y.GetType().GetMethod("RefreshWithConverterDependencies", MemberReflection.Everything).Invoke(y, new object[] { });

            Assert.Equal(4, defaultConverters.Count);
            Assert.Equal(1, bootstrapInvocationList);
            Assert.Equal(2, x.Settings.Converters.Count - defaultConverters.Count);
            Assert.Equal(2, y.Settings.Converters.Count - defaultConverters.Count);

            Assert.Equal(x.Settings.Converters.Count, y.Settings.Converters.Count);

            Assert.Equal(NewtonsoftJsonFormatterOptions.DefaultMediaType, x.SupportedMediaTypes.First());
        }
    }
}
