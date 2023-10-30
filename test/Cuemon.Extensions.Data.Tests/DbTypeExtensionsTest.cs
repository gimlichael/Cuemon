using System;
using System.Data;
using Cuemon.Extensions.Xunit;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.Extensions.Data
{
    public class DbTypeExtensionsTest : Test
    {
        public DbTypeExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void ToEquivalentType_ShouldMapDbTypeToBuiltInDotNetType()
        {
            Assert.Equal(typeof(byte), DbType.Byte.ToTypeEquivalent());
            Assert.Equal(typeof(sbyte), DbType.SByte.ToTypeEquivalent());
            Assert.Equal(typeof(byte[]), DbType.Binary.ToTypeEquivalent());
            Assert.Equal(typeof(bool), DbType.Boolean.ToTypeEquivalent());
            Assert.Equal(typeof(double), DbType.Currency.ToTypeEquivalent());
            Assert.Equal(typeof(double), DbType.Double.ToTypeEquivalent());
            Assert.Equal(typeof(DateTime), DbType.Date.ToTypeEquivalent());
            Assert.Equal(typeof(DateTime), DbType.DateTime.ToTypeEquivalent());
            Assert.Equal(typeof(DateTime), DbType.DateTime2.ToTypeEquivalent());
            Assert.Equal(typeof(DateTime), DbType.Time.ToTypeEquivalent());
            Assert.Equal(typeof(DateTimeOffset), DbType.DateTimeOffset.ToTypeEquivalent());
            Assert.Equal(typeof(Guid), DbType.Guid.ToTypeEquivalent());
            Assert.Equal(typeof(long), DbType.Int64.ToTypeEquivalent());
            Assert.Equal(typeof(int), DbType.Int32.ToTypeEquivalent());
            Assert.Equal(typeof(short), DbType.Int16.ToTypeEquivalent());
            Assert.Equal(typeof(object), DbType.Object.ToTypeEquivalent());
            Assert.Equal(typeof(float), DbType.Single.ToTypeEquivalent());
            Assert.Equal(typeof(ulong), DbType.UInt64.ToTypeEquivalent());
            Assert.Equal(typeof(uint), DbType.UInt32.ToTypeEquivalent());
            Assert.Equal(typeof(ushort), DbType.UInt16.ToTypeEquivalent());
            Assert.Equal(typeof(decimal), DbType.Decimal.ToTypeEquivalent());
            Assert.Equal(typeof(decimal), DbType.VarNumeric.ToTypeEquivalent());
            Assert.Equal(typeof(string), DbType.AnsiString.ToTypeEquivalent());
            Assert.Equal(typeof(string), DbType.AnsiStringFixedLength.ToTypeEquivalent());
            Assert.Equal(typeof(string), DbType.StringFixedLength.ToTypeEquivalent());
            Assert.Equal(typeof(string), DbType.String.ToTypeEquivalent());
            Assert.Equal(typeof(string), DbType.Xml.ToTypeEquivalent());
            
            var sut = Assert.Throws<ArgumentOutOfRangeException>(() => ((DbType)42).ToTypeEquivalent());
            Assert.Equal("dbType", sut.ParamName);
            Assert.StartsWith("DbType, '42', is not supported.", sut.Message);
        }
    }
}
