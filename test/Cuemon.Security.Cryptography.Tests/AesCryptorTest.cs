using System;
using System.Linq;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Security.Cryptography
{
    public class AesCryptorTest : Test
    {
        private readonly byte[] _secretKey;
        private readonly byte[] _iv;

        public AesCryptorTest(ITestOutputHelper output) : base(output)
        {
            _secretKey = AesCryptor.GenerateKey();
            _iv = AesCryptor.GenerateInitializationVector();
        }

        [Fact]
        public void AesCryptor_ShouldEncryptAndDecrypt()
        {
            var cryptor = new AesCryptor(_secretKey, _iv);
            var secretMessage = Decorator.Enclose("This is my secret message that needs encryption!").ToByteArray();

            Assert.True(_secretKey.SequenceEqual(cryptor.Key));
            Assert.True(_iv.SequenceEqual(cryptor.InitializationVector));

            var enc = cryptor.Encrypt(secretMessage);
            TestOutput.WriteLine(Convert.ToBase64String(enc));

            var dec = cryptor.Decrypt(enc);
            TestOutput.WriteLine(Convert.ToBase64String(dec));

            Assert.True(dec.SequenceEqual(secretMessage));
        }
    }
}