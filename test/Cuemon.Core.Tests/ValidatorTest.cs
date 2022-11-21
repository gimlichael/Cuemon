using System;
using System.Collections.Generic;
using System.IO;
using Cuemon.Assets;
using Cuemon.Extensions.Xunit;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon
{
    public class ValidatorTest : Test
    {
        public ValidatorTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void ThrowIfNull_Decorator_ShouldInitFakeOptionsToDefault()
        {
            var sut = Decorator.Enclose(new FakeOptions());

            Validator.ThrowIfNull(sut, "paramName", out var options);
            Assert.Equal(sut.Inner, options);
        }

        [Fact]
        public void ThrowIfNull_Decorator_ShouldThrowArgumentNullException()
        {
            var sut = (Decorator<FakeOptions>)null;

            var result = Assert.Throws<ArgumentNullException>(() =>
            {
                Validator.ThrowIfNull(sut, "paramName", out var options);
                Assert.Null(options);
            });

            Assert.Equal("Decorator or Inner cannot be null. (Parameter 'paramName')", result.Message);

            sut = Decorator.Enclose<FakeOptions>(null, false);

            result = Assert.Throws<ArgumentNullException>(() =>
            {
                Validator.ThrowIfNull(sut, "paramName", out var options);
                Assert.Null(options);
            });

            Assert.Equal("Decorator or Inner cannot be null. (Parameter 'Inner')", result.Message);
        }

        [Fact]
        public void ThrowIfInvalidConfigurator_ShouldThrowArgumentException_WithInnerNotImplementedException()
        {
            var result = Assert.Throws<ArgumentException>(() =>
            {
                Action<FakeOptions> setup = null;
                Validator.ThrowIfInvalidConfigurator(setup, "paramName", out var options);
                Assert.Equal(new FakeOptions(), options);
            });

            Assert.Equal("Delegate must configure the public read-write properties to be in a valid state. (Parameter 'paramName')", result.Message);
            Assert.IsType<NotImplementedException>(result.InnerException);
        }

        [Fact]
        public void ThrowIfInvalidOptions_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                Validator.ThrowIfInvalidOptions((FakeOptions)null, "paramName");
            });
        }

        [Fact]
        public void ThrowIfInvalidOptions_ShouldThrowArgumentException_WithInnerNotImplementedException()
        {
            var result = Assert.Throws<ArgumentException>(() =>
            {
                Validator.ThrowIfInvalidOptions(new FakeOptions(), "paramName");
            });

            Assert.Equal("FakeOptions are not in a valid state. (Parameter 'paramName')", result.Message);
            Assert.IsType<NotImplementedException>(result.InnerException);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("cuemon")]
        public void CheckParameter_ShouldThrowArgumentNullExceptionOrReturnString(string value)
        {
            if (value == null)
            {
                Assert.Throws<ArgumentNullException>(() =>
                {
                    Validator.CheckParameter(value, () =>
                    {
                        Validator.ThrowIfNull(value);
                    });
                });
            }
            else
            {
                Assert.Equal("cuemon", Validator.CheckParameter(value, () =>
                {
                    Validator.ThrowIfNull(value);
                }));
            }
        }

        [Fact]
        public void ThrowIfContainsType_ShouldThrowArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Validator.ThrowIfContainsType(typeof(ArgumentNullException), "paramName", typeof(ArgumentException));
            });

            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Validator.ThrowIfContainsType(new ArgumentNullException(), "paramName", typeof(ArgumentException));
            });
        }

        [Fact]
        public void ThrowIfContainsType_ShouldThrowTypeArgumentOutOfRangeException()
        {
            Assert.Throws<TypeArgumentOutOfRangeException>(() =>
            {
                Validator.ThrowIfContainsType<ArgumentNullException>("typeParamName", typeof(ArgumentException));
            });
        }

        [Fact]
        public void ThrowIfNotContainsType_ShouldThrowArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Validator.ThrowIfNotContainsType(typeof(ArgumentNullException), "paramName", typeof(OutOfMemoryException));
            });

            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Validator.ThrowIfNotContainsType(new ArgumentNullException(), "paramName", typeof(OutOfMemoryException));
            });
        }

        [Fact]
        public void ThrowIfNotContainsInterface_ShouldThrowArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Validator.ThrowIfNotContainsInterface(typeof(Stream), "paramName", typeof(IConvertible));
            });
        }

        [Fact]
        public void ThrowIfNotContainsInterface_ShouldThrowTypeArgumentOutOfRangeException()
        {
            Assert.Throws<TypeArgumentOutOfRangeException>(() =>
            {
                Validator.ThrowIfNotContainsInterface<Stream>("paramName", typeof(IConvertible));
            });
        }

        [Fact]
        public void ThrowIfContainsInterface_ShouldThrowArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Validator.ThrowIfContainsInterface(typeof(string), "paramName", typeof(IConvertible));
            });
        }

        [Fact]
        public void ThrowIfContainsInterface_ShouldThrowTypeArgumentOutOfRangeException()
        {
            Assert.Throws<TypeArgumentOutOfRangeException>(() =>
            {
                Validator.ThrowIfContainsInterface<bool>("paramName", typeof(IConvertible));
            });
        }

        [Fact]
        public void ThrowIfNotContainsType_ShouldThrowTypeArgumentOutOfRangeException()
        {
            Assert.Throws<TypeArgumentOutOfRangeException>(() =>
            {
                Validator.ThrowIfNotContainsType<ArgumentNullException>("typeParamName", typeof(OutOfMemoryException));
            });
        }

        [Fact]
        public void ThrowIfEmailAddress_ShouldThrowArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Validator.ThrowIfEmailAddress("michael@wbpa.dk", "paramName");
            });
        }

        [Fact]
        public void ThrowIfNotEmailAddress_ShouldThrowArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Validator.ThrowIfNotEmailAddress("michael", "paramName");
            });
        }

        [Fact]
        public void ThrowIfEmpty_ShouldThrowArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Validator.ThrowIfEmpty("", "paramName");
            });
        }

        [Fact]
        public void ThrowIfSequenceEmpty_ShouldThrowArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                var list = new List<string>();
                Validator.ThrowIfSequenceEmpty(list, "paramName");
            });
        }

        [Fact]
        public void ThrowIfSequenceNullOrEmpty_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                List<string> list = null;
                Validator.ThrowIfSequenceNullOrEmpty(list, "paramName");
            });
        }

        [Fact]
        public void ThrowIfSequenceNullOrEmpty_ShouldThrowArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                var list = new List<string>();
                Validator.ThrowIfSequenceNullOrEmpty(list, "paramName");
            });
        }

        [Fact]
        public void ThrowIfEnum_ShouldThrowArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Validator.ThrowIfEnum<VerticalDirection>("Up", "paramName");
            });
        }

        [Fact]
        public void ThrowIfEnumType_ShouldThrowArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Validator.ThrowIfEnumType(typeof(VerticalDirection), "paramName");
            });
        }

        [Fact]
        public void ThrowIfNotEnumType_ShouldThrowArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Validator.ThrowIfNotEnumType(typeof(Stream), "paramName");
            });
        }

        [Fact]
        public void ThrowIfNotEnum_ShouldThrowArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Validator.ThrowIfNotEnum<VerticalDirection>("Ups", "paramName");
            });
        }

        [Fact]
        public void ThrowIfEnumType_ShouldThrowTypeArgumentException()
        {
            Assert.Throws<TypeArgumentException>(() =>
            {
                Validator.ThrowIfEnumType<VerticalDirection>("typeParamName");
            });
        }

        [Fact]
        public void ThrowIfNotEnumType_ShouldThrowTypeArgumentException()
        {
            Assert.Throws<TypeArgumentException>(() =>
            {
                Validator.ThrowIfNotEnumType<DateTime>("typeParamName");
            });
        }

        [Fact]
        public void ThrowIfEqual_ShouldThrowArgumentOutOfRangeException()
        {
            var sut = Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Validator.ThrowIfEqual(1, 1, "paramName");
            });

            Assert.Equal(@"Specified arguments x and y are equal to one another. (Parameter 'paramName')
Actual value was 1 == 1.", sut.Message);
        }

        [Fact]
        public void ThrowIfEqual_ShouldThrowArgumentOutOfRangeException_Message()
        {
            var sut = Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Validator.ThrowIfEqual(1, 1, "marapName", "customMessage");
            });

            Assert.Equal(@"customMessage (Parameter 'marapName')
Actual value was 1 == 1.", sut.Message);
        }

        [Fact]
        public void ThrowIfNotEqual_ShouldThrowArgumentOutOfRangeException()
        {
            var sut = Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Validator.ThrowIfNotEqual(1, 2, "paramName");
            });

            Assert.Equal(@"Specified arguments x and y are not equal to one another. (Parameter 'paramName')
Actual value was 1 != 2.", sut.Message);
        }

        [Fact]
        public void ThrowIfNotEqual_ShouldThrowArgumentOutOfRangeException_Message()
        {
            var sut = Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Validator.ThrowIfNotEqual(1, 2, "marapName", "customMessage");
            });

            Assert.Equal(@"customMessage (Parameter 'marapName')
Actual value was 1 != 2.", sut.Message);
        }

        [Fact]
        public void ThrowIfFalse_ShouldThrowArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Validator.ThrowIfFalse(false, "paramName");
            });
        }

        [Fact]
        public void ThrowIfFalse_PredicateShouldThrowArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Validator.ThrowIfFalse(() => false, "paramName", "Value is false ;-)");
            });
        }

        [Fact]
        public void ThrowIfTrue_ShouldThrowArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Validator.ThrowIfTrue(true, "paramName");
            });
        }

        [Fact]
        public void ThrowIfTrue_PredicateShouldThrowArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Validator.ThrowIfTrue(() => true, "paramName", "Value is true ;-)");
            });
        }

        [Fact]
        public void ThrowIfGreaterThan_ShouldThrowArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Validator.ThrowIfGreaterThan(2, 1, "paramName");
            });
        }

        [Fact]
        public void ThrowIfGreaterThanOrEqual_ShouldThrowArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Validator.ThrowIfGreaterThanOrEqual(2, 2, "paramName");
            });
        }

        [Fact]
        public void ThrowIfGuid_ShouldThrowArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Validator.ThrowIfGuid(Guid.Empty.ToString(), "paramName");
            });
        }

        [Fact]
        public void ThrowIfNotGuid_ShouldThrowArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Validator.ThrowIfNotGuid("not-a-guid", "paramName");
            });
        }

        [Fact]
        public void ThrowIfHex_ShouldThrowArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Validator.ThrowIfHex("AAB0F3C1", "paramName");
            });
        }

        [Fact]
        public void ThrowIfNotHex_ShouldThrowArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Validator.ThrowIfNotHex("olkiujhy", "paramName");
            });
        }

        [Fact]
        public void ThrowIfLowerThan_ShouldThrowArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Validator.ThrowIfLowerThan(1, 2, "paramName");
            });
        }

        [Fact]
        public void ThrowIfLowerThanOrEqual_ShouldThrowArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Validator.ThrowIfLowerThanOrEqual(2, 2, "paramName");
            });
        }

        [Fact]
        public void ThrowIfNumber_ShouldThrowArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Validator.ThrowIfNumber("22", "paramName");
            });
        }

        [Fact]
        public void ThrowIfNotNumber_ShouldThrowArgumentException()
        {
            var sut = Assert.Throws<ArgumentException>(() =>
            {
                Validator.ThrowIfNotNumber("22cads", "paramName");
            });

            Assert.Equal("Value must be a number. (Parameter 'paramName')", sut.Message);
        }

        [Fact]
        public void ThrowIfNotNumber_ShouldThrowArgumentException_Message()
        {
            var sut = Assert.Throws<ArgumentException>(() =>
            {
                Validator.ThrowIfNotNumber("22cads", "marapName", "customMessage");
            });

            Assert.Equal("customMessage (Parameter 'marapName')", sut.Message);
        }

        [Theory]
        [InlineData(null)]
        public void ThrowIfNull_ShouldThrowArgumentNullException(string value)
        {
            var sut = Assert.Throws<ArgumentNullException>(() =>
            {
                Validator.ThrowIfNull(value);
            });

            Assert.Equal("Value cannot be null. (Parameter 'value')", sut.Message);

            sut = Assert.Throws<ArgumentNullException>(() =>
            {
                Validator.ThrowIfNull(value);
            });

            Assert.Equal("Value cannot be null. (Parameter 'value')", sut.Message);
        }

        [Theory]
        [InlineData(null)]
        public void ThrowIfNullOrEmpty_ShouldThrowArgumentNullException(string value)
        {
            var sut = Assert.Throws<ArgumentNullException>(() =>
            {
                Validator.ThrowIfNullOrEmpty(value);
            });

            Assert.Equal("Value cannot be null. (Parameter 'value')", sut.Message);

            sut = Assert.Throws<ArgumentNullException>(() =>
            {
                Validator.ThrowIfNullOrEmpty(value);
            });

            Assert.Equal("Value cannot be null. (Parameter 'value')", sut.Message);

            sut = Assert.Throws<ArgumentNullException>(() =>
            {
                Validator.ThrowIfNullOrEmpty(value, nameof(value), "message");
            });

            Assert.Equal("message (Parameter 'value')", sut.Message);
        }

        [Theory]
        [InlineData("")]
        public void ThrowIfNullOrEmpty_ShouldThrowArgumentException(string value)
        {
            var sut = Assert.Throws<ArgumentException>(() =>
            {
                Validator.ThrowIfNullOrEmpty(value);
            });

            Assert.Equal("Value cannot be empty. (Parameter 'value')", sut.Message);

            sut = Assert.Throws<ArgumentException>(() =>
            {
                Validator.ThrowIfNullOrEmpty("", nameof(value));
            });

            Assert.Equal("Value cannot be empty. (Parameter 'value')", sut.Message);

            sut = Assert.Throws<ArgumentException>(() =>
            {
                Validator.ThrowIfNullOrEmpty("", nameof(value), "message");
            });

            Assert.Equal("message (Parameter 'value')", sut.Message);
        }

        [Theory]
        [InlineData(null)]
        public void ThrowIfNullOrWhitespace_ShouldThrowArgumentNullException(string value)
        {
            var sut = Assert.Throws<ArgumentNullException>(() =>
            {
                Validator.ThrowIfNullOrWhitespace(value);
            });

            Assert.Equal("Value cannot be null. (Parameter 'value')", sut.Message);

            sut = Assert.Throws<ArgumentNullException>(() =>
            {
                Validator.ThrowIfNullOrWhitespace(value);
            });

            Assert.Equal("Value cannot be null. (Parameter 'value')", sut.Message);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void ThrowIfNullOrWhitespace_ShouldThrowArgumentException(string value)
        {
            var sut = Assert.Throws<ArgumentException>(() =>
            {
                Validator.ThrowIfNullOrWhitespace(value);
            });

            Assert.Equal(Condition.TernaryIf(value.Length == 0, ()=> "Value cannot be empty. (Parameter 'value')", () => "Value cannot consist only of white-space characters. (Parameter 'value')"), sut.Message);

            sut = Assert.Throws<ArgumentException>(() =>
            {
                Validator.ThrowIfNullOrWhitespace(value);
            });

            Assert.Equal(Condition.TernaryIf(value.Length == 0, ()=> "Value cannot be empty. (Parameter 'value')", () => "Value cannot consist only of white-space characters. (Parameter 'value')"), sut.Message);

            sut = Assert.Throws<ArgumentException>(() =>
            {
                Validator.ThrowIfNullOrWhitespace(value, nameof(value), "message");
            });

            Assert.Equal("message (Parameter 'value')", sut.Message);
        }

        [Fact]
        public void ThrowIfSame_ShouldThrowArgumentOutOfRangeException()
        {
            var t1 = TimeZoneInfo.Utc;
            var t2 = t1;
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Validator.ThrowIfSame(t1, t2, "paramName");
            });
        }

        [Fact]
        public void ThrowIfNotSame_ShouldThrowArgumentOutOfRangeException()
        {
            var t1 = TimeZoneInfo.Utc;
            var t2 = TimeZoneInfo.Local;
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Validator.ThrowIfNotSame(t1, t2, "paramName");
            });
        }

        [Fact]
        public void ThrowIfUri_ShouldThrowArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Validator.ThrowIfUri("https://www.cuemon.net/", "paramName");
            });
        }

        [Fact]
        public void ThrowIfNotUri_ShouldThrowArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Validator.ThrowIfNotUri("www.cuemon.net", "paramName");
            });
        }

        [Fact]
        public void ThrowIfWhiteSpace_ShouldThrowArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Validator.ThrowIfWhiteSpace(" ", "paramName");
            });
        }

        [Fact]
        public void ThrowIfNotBinaryDigits_ShouldThrowArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Validator.ThrowIfNotBinaryDigits("12345678", "paramName");
            });
        }

        [Fact]
        public void ThrowIfNotBase64String_ShouldThrowArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Validator.ThrowIfNotBase64String("DJ BOBO", "paramName");
            });
        }
    }
}