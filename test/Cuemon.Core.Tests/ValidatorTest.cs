using System;
using System.Collections.Generic;
using System.IO;
using Cuemon.Assets;
#if NET48_OR_GREATER
using Cuemon.Extensions;
#endif
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
		public void ThrowIfObjectDisposed_ByTypeShouldThrowObjectDisposedException()
		{
			var sut = Assert.Throws<ObjectDisposedException>(() =>
			{
				Validator.ThrowIfDisposed(true, GetType());
			});

			Assert.Equal("""
                         Cannot access a disposed object.
                         Object name: 'Cuemon.ValidatorTest'.
                         """.ReplaceLineEndings(), sut.Message);

			sut = Assert.Throws<ObjectDisposedException>(() =>
			{
				Validator.ThrowIfDisposed(true, null);
			});

			Assert.Equal("""
                         Cannot access a disposed object.
                         """, sut.Message);

			var dic = new Dictionary<string, object>();

			sut = Assert.Throws<ObjectDisposedException>(() =>
			{
				Validator.ThrowIfDisposed(true, dic.GetType());
			});

			Assert.Equal("""
                         Cannot access a disposed object.
                         Object name: 'System.Collections.Generic.Dictionary<System.String,System.Object>'.
                         """.ReplaceLineEndings(), sut.Message);

			Validator.ThrowIfDisposed(false, GetType());
		}

		[Fact]
		public void ThrowIfObjectDisposed_ByObjectShouldThrowObjectDisposedException()
		{
			var sut = Assert.Throws<ObjectDisposedException>(() =>
			{
				Validator.ThrowIfDisposed(true, this);
			});

			Assert.Equal("""
                         Cannot access a disposed object.
                         Object name: 'Cuemon.ValidatorTest'.
                         """.ReplaceLineEndings(), sut.Message);

			sut = Assert.Throws<ObjectDisposedException>(() =>
			{
				Validator.ThrowIfDisposed(true, null);
			});

			Assert.Equal("""
                         Cannot access a disposed object.
                         """, sut.Message);

			var dic = new Dictionary<string, object>();

			sut = Assert.Throws<ObjectDisposedException>(() =>
			{
				Validator.ThrowIfDisposed(true, dic);
			});

			Assert.Equal("""
                         Cannot access a disposed object.
                         Object name: 'System.Collections.Generic.Dictionary<System.String,System.Object>'.
                         """.ReplaceLineEndings(), sut.Message);

			Validator.ThrowIfDisposed(false, this);
		}

		[Fact]
		public void ThrowIfObjectInDistress_ShouldThrowInvalidOperationException()
		{
			var sut = Assert.Throws<InvalidOperationException>(() =>
			{
				Validator.ThrowIfInvalidState(1 == 1);
			});

			Assert.Equal("Operation is not valid due to the current state of the object. (Expression '1 == 1')", sut.Message);
		}

		[Fact]
		public void ThrowIfNull_Decorator_ShouldInitFakeOptionsToDefault()
		{
			var sut = Decorator.Enclose(new ValidatableOptions());

			Validator.ThrowIfNull(sut, out var options, "paramName");
			Assert.Equal(sut.Inner, options);
		}

		[Fact]
		public void ThrowIfNull_Decorator_ShouldThrowArgumentNullException()
		{
			var sut = (Decorator<ValidatableOptions>)null;

			var result = Assert.Throws<ArgumentNullException>(() =>
			{
				Validator.ThrowIfNull(sut, out var options, "paramName");
				Assert.Null(options);
			});

			Assert.StartsWith("Value cannot be null", result.Message);
			Assert.Contains("paramName", result.Message);
			Assert.DoesNotContain("sut", result.Message);

			sut = Decorator.Enclose<ValidatableOptions>(null, false);

			result = Assert.Throws<ArgumentNullException>(() =>
			{
				Validator.ThrowIfNull(sut, out var options);
				Assert.Null(options);
			});

			Assert.StartsWith("Value cannot be null.", result.Message);
			Assert.Contains("sut", result.Message);
			Assert.DoesNotContain("paramName", result.Message);
		}

		[Fact]
		public void ThrowIfInvalidConfigurator_ShouldThrowArgumentException_WithInnerNotImplementedException()
		{
			ValidatableOptions options = null;
			var result = Assert.Throws<ArgumentException>(() =>
			{
				Action<ValidatableOptions> setup = null;
				Validator.ThrowIfInvalidConfigurator(setup, out options);
			});
			Assert.Equivalent(new ValidatableOptions(), options, true);

			Assert.StartsWith("Delegate must configure the public read-write properties to be in a valid state.", result.Message);
			Assert.Contains("setup", result.Message);
			Assert.IsType<NotImplementedException>(result.InnerException);
		}

		[Fact]
		public void ThrowIfInvalidConfigurator_ShouldNotThrow_SinceNonValidatable()
		{
			Action<EssentialOptions> setup = null;
			Validator.ThrowIfInvalidConfigurator(setup, out var options);
			Assert.Equivalent(new EssentialOptions(), options, true);
		}

		[Fact]
		public void ThrowIfInvalidOptions_ShouldNotThrow_SinceNonValidatable()
		{
			Validator.ThrowIfInvalidOptions(new EssentialOptions(), "paramName");
		}

		[Fact]
		public void ThrowIfInvalidOptions_ShouldThrowArgumentNullException()
		{
			Assert.Throws<ArgumentNullException>(() =>
			{
				Validator.ThrowIfInvalidOptions((ValidatableOptions)null, "paramName");
			});

			Assert.Throws<ArgumentNullException>(() =>
			{
				Validator.ThrowIfInvalidOptions((EssentialOptions)null, "paramName");
			});
		}

		[Fact]
		public void ThrowIfInvalidOptions_ShouldThrowArgumentException_WithInnerNotImplementedException()
		{
			var result = Assert.Throws<ArgumentException>(() =>
			{
				Validator.ThrowIfInvalidOptions(new ValidatableOptions(), "paramName");
			});

			Assert.StartsWith($"{nameof(ValidatableOptions)} are not in a valid state.", result.Message);
			Assert.Contains("paramName", result.Message);
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

			Assert.StartsWith("Specified arguments x and y are equal to one another.", sut.Message);
			Assert.Contains("paramName", sut.Message);
			Assert.EndsWith("Actual value was 1 == 1.", sut.Message);
		}

		[Fact]
		public void ThrowIfEqual_ShouldThrowArgumentOutOfRangeException_Message()
		{
			var sut = Assert.Throws<ArgumentOutOfRangeException>(() =>
			{
				Validator.ThrowIfEqual(1, 1, "marapName", message: "customMessage");
			});

			Assert.StartsWith("customMessage", sut.Message);
			Assert.Contains("marapName", sut.Message);
			Assert.EndsWith("Actual value was 1 == 1.", sut.Message);
		}

		[Fact]
		public void ThrowIfNotEqual_ShouldThrowArgumentOutOfRangeException()
		{
			var sut = Assert.Throws<ArgumentOutOfRangeException>(() =>
			{
				Validator.ThrowIfNotEqual(1, 2, "paramName");
			});

			Assert.StartsWith("Specified arguments x and y are not equal to one another.", sut.Message);
			Assert.Contains("paramName", sut.Message);
			Assert.EndsWith("Actual value was 1 != 2.", sut.Message);
		}

		[Fact]
		public void ThrowIfNotEqual_ShouldThrowArgumentOutOfRangeException_Message()
		{
			var sut = Assert.Throws<ArgumentOutOfRangeException>(() =>
			{
				Validator.ThrowIfNotEqual(1, 2, "marapName", message: "customMessage");
			});

			Assert.StartsWith("customMessage", sut.Message);
			Assert.Contains("marapName", sut.Message);
			Assert.EndsWith("Actual value was 1 != 2.", sut.Message);
		}

		[Fact]
		public void ThrowIfFalse_ShouldThrowArgumentException()
		{
			var sut = Assert.Throws<ArgumentException>(() =>
			{
				Validator.ThrowIfFalse(false, "paramName");
			});
#if NET48_OR_GREATER
            Assert.Equal("Value is not in a valid state. (Expression 'false')\r\nParameter name: paramName".ReplaceLineEndings(), sut.Message);
#else
			Assert.Equal("Value is not in a valid state. (Expression 'false') (Parameter 'paramName')", sut.Message);
#endif
		}

		[Fact]
		public void ThrowIfFalse_PredicateShouldThrowArgumentException()
		{
			var sut = Assert.Throws<ArgumentException>(() =>
			{
				Validator.ThrowIfFalse(() => false, "paramName", "Value is false ;-)");
			});
#if NET48_OR_GREATER
            Assert.Equal("Value is false ;-) (Expression '() => false')\r\nParameter name: paramName".ReplaceLineEndings(), sut.Message);
#else
			Assert.Equal("Value is false ;-) (Expression '() => false') (Parameter 'paramName')", sut.Message);
#endif
		}

		[Fact]
		public void ThrowIfTrue_ShouldThrowArgumentException()
		{
			var sut = Assert.Throws<ArgumentException>(() =>
			{
				Validator.ThrowIfTrue(true, "paramName");
			});
			TestOutput.WriteLine(sut.ToString());
#if NET48_OR_GREATER
            Assert.Equal("Value is not in a valid state. (Expression 'true')\r\nParameter name: paramName".ReplaceLineEndings(), sut.Message);
#else
			Assert.Equal("Value is not in a valid state. (Expression 'true') (Parameter 'paramName')", sut.Message);
#endif
		}

		[Fact]
		public void ThrowIfTrue_PredicateShouldThrowArgumentException()
		{
			var sut = Assert.Throws<ArgumentException>(() =>
			{
				Validator.ThrowIfTrue(() => true, "paramName", "Value is true ;-)");
			});
#if NET48_OR_GREATER
            Assert.Equal("Value is true ;-) (Expression '() => true')\r\nParameter name: paramName".ReplaceLineEndings(), sut.Message);
#else
			Assert.Equal("Value is true ;-) (Expression '() => true') (Parameter 'paramName')", sut.Message);
#endif

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

			Assert.StartsWith("Value must be a number.", sut.Message);
			Assert.Contains("paramName", sut.Message);
		}

		[Fact]
		public void ThrowIfNotNumber_ShouldThrowArgumentException_Message()
		{
			var sut = Assert.Throws<ArgumentException>(() =>
			{
				Validator.ThrowIfNotNumber("22cads", "marapName", message: "customMessage");
			});

			Assert.StartsWith("customMessage", sut.Message);
			Assert.Contains("marapName", sut.Message);
		}

		[Theory]
		[InlineData(null)]
		public void ThrowIfNull_ShouldThrowArgumentNullException(string value)
		{
			var sut = Assert.Throws<ArgumentNullException>(() =>
			{
				Validator.ThrowIfNull(value);
			});

			Assert.StartsWith("Value cannot be null.", sut.Message);
			Assert.Contains("value", sut.Message);

			TestOutput.WriteLine(sut.ToString());

			sut = Assert.Throws<ArgumentNullException>(() =>
			{
				Validator.ThrowIfNull(value, "paramName");
			});

			Assert.StartsWith("Value cannot be null.", sut.Message);
			Assert.Contains("paramName", sut.Message);
		}

		[Theory]
		[InlineData(null)]
		public void ThrowIfNullOrEmpty_ShouldThrowArgumentNullException(string value)
		{
			var sut = Assert.Throws<ArgumentNullException>(() =>
			{
				Validator.ThrowIfNullOrEmpty(value);
			});

			Assert.StartsWith("Value cannot be null.", sut.Message);
			Assert.Contains("value", sut.Message);

			sut = Assert.Throws<ArgumentNullException>(() =>
			{
				Validator.ThrowIfNullOrEmpty(value);
			});

			Assert.StartsWith("Value cannot be null.", sut.Message);
			Assert.Contains("value", sut.Message);

			sut = Assert.Throws<ArgumentNullException>(() =>
			{
				Validator.ThrowIfNullOrEmpty(value, nameof(value), "message");
			});

			Assert.StartsWith("message", sut.Message);
			Assert.Contains("value", sut.Message);
		}

		[Theory]
		[InlineData("")]
		public void ThrowIfNullOrEmpty_ShouldThrowArgumentException(string value)
		{
			var sut = Assert.Throws<ArgumentException>(() =>
			{
				Validator.ThrowIfNullOrEmpty(value);
			});

			Assert.StartsWith("Value cannot be empty.", sut.Message);
			Assert.Contains("value", sut.Message);

			sut = Assert.Throws<ArgumentException>(() =>
			{
				Validator.ThrowIfNullOrEmpty("", nameof(value));
			});


			Assert.StartsWith("Value cannot be empty.", sut.Message);
			Assert.Contains("value", sut.Message);

			sut = Assert.Throws<ArgumentException>(() =>
			{
				Validator.ThrowIfNullOrEmpty("", nameof(value), "message");
			});

			Assert.StartsWith("message", sut.Message);
			Assert.Contains("value", sut.Message);
		}

		[Theory]
		[InlineData(null)]
		public void ThrowIfNullOrWhitespace_ShouldThrowArgumentNullException(string value)
		{
			var sut = Assert.Throws<ArgumentNullException>(() =>
			{
				Validator.ThrowIfNullOrWhitespace(value);
			});

			Assert.StartsWith("Value cannot be null.", sut.Message);
			Assert.Contains("value", sut.Message);

			sut = Assert.Throws<ArgumentNullException>(() =>
			{
				Validator.ThrowIfNullOrWhitespace(value);
			});

			Assert.StartsWith("Value cannot be null.", sut.Message);
			Assert.Contains("value", sut.Message);
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

			Assert.StartsWith(Condition.TernaryIf(value.Length == 0, () => "Value cannot be empty.", () => "Value cannot consist only of white-space characters."), sut.Message);
			Assert.Contains("value", sut.Message);

			sut = Assert.Throws<ArgumentException>(() =>
			{
				Validator.ThrowIfNullOrWhitespace(value);
			});

			Assert.StartsWith(Condition.TernaryIf(value.Length == 0, () => "Value cannot be empty.", () => "Value cannot consist only of white-space characters."), sut.Message);
			Assert.Contains("value", sut.Message);

			sut = Assert.Throws<ArgumentException>(() =>
			{
				Validator.ThrowIfNullOrWhitespace(value, nameof(value), "message");
			});

			Assert.StartsWith("message", sut.Message);
			Assert.Contains("value", sut.Message);
		}

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void ThrowIfNullOrWhitespace_ShouldThrowArgumentException_WithCustomMessage(string value)
        {
            var expected = "Value cannot be null, empty or consist only of white-space characters.";

            if (value == null)
            {
                var sut = Assert.Throws<ArgumentNullException>(() =>
                {
                    Validator.ThrowIfNullOrWhitespace(value, message: expected);
                });
                Assert.StartsWith(expected, sut.Message);
            }
            else
            {
                var sut = Assert.Throws<ArgumentException>(() =>
                {
                    Validator.ThrowIfNullOrWhitespace(value, message: expected);
                });
                Assert.StartsWith(expected, sut.Message);
            }
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

            Assert.Throws<ArgumentException>(() =>
            {
                Validator.ThrowIfUri("/blog", "paramName", UriKind.Relative);
            });

            Assert.Throws<ArgumentException>(() =>
            {
                Validator.ThrowIfUri("/blog", "paramName", UriKind.RelativeOrAbsolute);
            });
		}

		[Fact]
		public void ThrowIfNotUri_ShouldThrowArgumentException()
		{
			Assert.Throws<ArgumentException>(() =>
			{
				Validator.ThrowIfNotUri("www.cuemon.net", "paramName");
			});

            Assert.Throws<ArgumentException>(() =>
            {
                Validator.ThrowIfNotUri("blog:that", "paramName", UriKind.Relative);
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
			var sut = Assert.Throws<ArgumentOutOfRangeException>(() =>
			{
				Validator.ThrowIfNotBase64String("DJ BOBO", "paramName");
			});

			Assert.Equal("paramName", sut.ParamName);
			Assert.Equal("DJ BOBO", sut.ActualValue);
			Assert.StartsWith("Value must consist only of base-64 digits.", sut.Message);

			sut = Assert.Throws<ArgumentOutOfRangeException>(() =>
			{
				Validator.ThrowIfNotBase64String(sut.ParamName);
			});

			Assert.Equal("sut.ParamName", sut.ParamName);
			Assert.Equal("paramName", sut.ActualValue);
			Assert.StartsWith("Value must consist only of base-64 digits.", sut.Message);
		}
	}
}