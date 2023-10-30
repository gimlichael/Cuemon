using System;
using System.Data;
using Cuemon.Extensions.Xunit;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.Data
{
    public class DbTypeDecoratorExtensionsTest : Test
    {
        public DbTypeDecoratorExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void ToEquivalentType_ShouldMapDbTypeToBuiltInDotNetType()
        {
            Assert.Equal(typeof(byte), Decorator.Enclose(DbType.Byte).ToTypeEquivalent());
            Assert.Equal(typeof(sbyte), Decorator.Enclose(DbType.SByte).ToTypeEquivalent());
            Assert.Equal(typeof(byte[]), Decorator.Enclose(DbType.Binary).ToTypeEquivalent());
            Assert.Equal(typeof(bool), Decorator.Enclose(DbType.Boolean).ToTypeEquivalent());
            Assert.Equal(typeof(double), Decorator.Enclose(DbType.Currency).ToTypeEquivalent());
            Assert.Equal(typeof(double), Decorator.Enclose(DbType.Double).ToTypeEquivalent());
            Assert.Equal(typeof(DateTime), Decorator.Enclose(DbType.Date).ToTypeEquivalent());
            Assert.Equal(typeof(DateTime), Decorator.Enclose(DbType.DateTime).ToTypeEquivalent());
            Assert.Equal(typeof(DateTime), Decorator.Enclose(DbType.DateTime2).ToTypeEquivalent());
            Assert.Equal(typeof(DateTime), Decorator.Enclose(DbType.Time).ToTypeEquivalent());
            Assert.Equal(typeof(DateTimeOffset), Decorator.Enclose(DbType.DateTimeOffset).ToTypeEquivalent());
            Assert.Equal(typeof(Guid), Decorator.Enclose(DbType.Guid).ToTypeEquivalent());
            Assert.Equal(typeof(long), Decorator.Enclose(DbType.Int64).ToTypeEquivalent());
            Assert.Equal(typeof(int), Decorator.Enclose(DbType.Int32).ToTypeEquivalent());
            Assert.Equal(typeof(short), Decorator.Enclose(DbType.Int16).ToTypeEquivalent());
            Assert.Equal(typeof(object), Decorator.Enclose(DbType.Object).ToTypeEquivalent());
            Assert.Equal(typeof(float), Decorator.Enclose(DbType.Single).ToTypeEquivalent());
            Assert.Equal(typeof(ulong), Decorator.Enclose(DbType.UInt64).ToTypeEquivalent());
            Assert.Equal(typeof(uint), Decorator.Enclose(DbType.UInt32).ToTypeEquivalent());
            Assert.Equal(typeof(ushort), Decorator.Enclose(DbType.UInt16).ToTypeEquivalent());
            Assert.Equal(typeof(decimal), Decorator.Enclose(DbType.Decimal).ToTypeEquivalent());
            Assert.Equal(typeof(decimal), Decorator.Enclose(DbType.VarNumeric).ToTypeEquivalent());
            Assert.Equal(typeof(string), Decorator.Enclose(DbType.AnsiString).ToTypeEquivalent());
            Assert.Equal(typeof(string), Decorator.Enclose(DbType.AnsiStringFixedLength).ToTypeEquivalent());
            Assert.Equal(typeof(string), Decorator.Enclose(DbType.StringFixedLength).ToTypeEquivalent());
            Assert.Equal(typeof(string), Decorator.Enclose(DbType.String).ToTypeEquivalent());
            Assert.Equal(typeof(string), Decorator.Enclose(DbType.Xml).ToTypeEquivalent());

            var sut = Assert.Throws<ArgumentOutOfRangeException>(() => Decorator.Enclose((DbType)42).ToTypeEquivalent());
            Assert.Equal("decorator", sut.ParamName);
            Assert.StartsWith("DbType, '42', is not supported.", sut.Message);
        }
    }
}
