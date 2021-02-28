using System;
using System.Collections.Generic;
using System.IO;
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
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Validator.ThrowIfEqual(1, 1, "paramName");
            });
        }

        [Fact]
        public void ThrowIfNotEqual_ShouldThrowArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Validator.ThrowIfNotEqual(1, 2, "paramName");
            });
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
        public void ThrowIfTrue_ShouldThrowArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Validator.ThrowIfTrue(true, "paramName");
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
            Assert.Throws<ArgumentException>(() =>
            {
                Validator.ThrowIfNotNumber("22cads", "paramName");
            });
        }

        [Fact]
        public void ThrowIfNull_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                Validator.ThrowIfNull<string>(null, "paramName");
            });
        }

        [Fact]
        public void ThrowIfNullOrEmpty_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                Validator.ThrowIfNullOrEmpty(null, "paramName");
            });
        }

        [Fact]
        public void ThrowIfNullOrEmpty_ShouldThrowArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Validator.ThrowIfNullOrEmpty("", "paramName");
            });
        }

        [Fact]
        public void ThrowIfNullOrWhitespace_ShouldThrowArgumentNullException_And_ArgumentException()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                Validator.ThrowIfNullOrWhitespace(null, "paramName");
            });
        }

        [Fact]
        public void ThrowIfNullOrWhitespace_ShouldThrowArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Validator.ThrowIfNullOrWhitespace("", "paramName");
            });
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