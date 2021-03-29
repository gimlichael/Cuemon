using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cuemon.Extensions.Xunit;
using Cuemon.Security.Cryptography;
using Microsoft.Extensions.Options;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.Extensions.AspNetCore.Configuration
{
    public class AssemblyCacheBustingTest : Test
    {
        public AssemblyCacheBustingTest(ITestOutputHelper output) : base(output)
        {
        }

        [Theory]
        [MemberData(nameof(GetAlgorithmOptions))]
        public void Ctor_ShouldUseDifferentAlgorithms(AssemblyCacheBustingOptions options)
        {
            var sut = new AssemblyCacheBusting(new OptionsWrapper<AssemblyCacheBustingOptions>(options));

            TestOutput.WriteLine(sut.Version);

            Condition.IsTrue(options.Algorithm == UnkeyedCryptoAlgorithm.Md5, () => Assert.Equal("bcb895f28f0b3d781dce1b1154896e1c", sut.Version));
            Condition.IsTrue(options.Algorithm == UnkeyedCryptoAlgorithm.Sha1, () => Assert.Equal("b491a390bd9438e7c5568be2852173111310b85d", sut.Version));
            Condition.IsTrue(options.Algorithm == UnkeyedCryptoAlgorithm.Sha256, () => Assert.Equal("4b123be85e851a9e552a99b2c3afd7babe3be2b23d298e9431c3304003393229", sut.Version));
            Condition.IsTrue(options.Algorithm == UnkeyedCryptoAlgorithm.Sha384, () => Assert.Equal("6059a2579c2e76992779b1f5fa3ff703c632aad698140b720bc667b0253b26ff26673d144d72b544eaa0f0414e7df76b", sut.Version));
            Condition.IsTrue(options.Algorithm == UnkeyedCryptoAlgorithm.Sha512, () => Assert.Equal("bb6e85208f404f2352b8b0dbf9b7c56a9f6be4ab0dab620b42dae3b07097394779a9f2f3d5c72064197fe7ed0eb302f56ada6415901a3ca08220a4a195cfe6f1", sut.Version));
        }

        [Theory]
        [MemberData(nameof(GetAlgorithmOptionsWithStrongIntegrityEnabled))]
        public void Ctor_ShouldUseDifferentAlgorithmsWithStrongIntegrity(AssemblyCacheBustingOptions options)
        {
            var sut1 = new AssemblyCacheBusting(new OptionsWrapper<AssemblyCacheBustingOptions>(options));
            var sut2 = File.ReadAllBytes(typeof(AssemblyCacheBustingTest).Assembly.Location).Concat(Convertible.GetBytes(Generate.HashCode64(typeof(AssemblyCacheBustingTest).Assembly.FullName))).ToArray();
            var sut3 = UnkeyedHashFactory.CreateCrypto(options.Algorithm).ComputeHash(sut2).ToHexadecimalString();
            
            TestOutput.WriteLine(sut1.Version);

            Assert.Equal(sut3, sut1.Version);
        }

        private static IEnumerable<object[]> GetAlgorithmOptions()
        {
            var parameters = new List<object[]>()
            {
                new object[]
                {
                    new AssemblyCacheBustingOptions()
                    {
                        Assembly = typeof(AssemblyCacheBustingTest).Assembly
                    }
                },
                new object[]
                {
                    new AssemblyCacheBustingOptions()
                    {
                        Assembly = typeof(AssemblyCacheBustingTest).Assembly,
                        Algorithm = UnkeyedCryptoAlgorithm.Sha1
                    }
                },
                new object[]
                {
                    new AssemblyCacheBustingOptions()
                    {
                        Assembly = typeof(AssemblyCacheBustingTest).Assembly,
                        Algorithm = UnkeyedCryptoAlgorithm.Sha256
                    }
                },
                new object[]
                {
                    new AssemblyCacheBustingOptions()
                    {
                        Assembly = typeof(AssemblyCacheBustingTest).Assembly,
                        Algorithm = UnkeyedCryptoAlgorithm.Sha384
                    }
                },
                new object[]
                {
                    new AssemblyCacheBustingOptions()
                    {
                        Assembly = typeof(AssemblyCacheBustingTest).Assembly,
                        Algorithm = UnkeyedCryptoAlgorithm.Sha512
                    }
                }
            };

            return parameters;
        }

        private static IEnumerable<object[]> GetAlgorithmOptionsWithStrongIntegrityEnabled()
        {
            var parameters = new List<object[]>()
            {
                new object[]
                {
                    new AssemblyCacheBustingOptions()
                    {
                        Assembly = typeof(AssemblyCacheBustingTest).Assembly,
                        ReadByteForByteChecksum = true
                    }
                },
                new object[]
                {
                    new AssemblyCacheBustingOptions()
                    {
                        Assembly = typeof(AssemblyCacheBustingTest).Assembly,
                        Algorithm = UnkeyedCryptoAlgorithm.Sha1,
                        ReadByteForByteChecksum = true
                    }
                },
                new object[]
                {
                    new AssemblyCacheBustingOptions()
                    {
                        Assembly = typeof(AssemblyCacheBustingTest).Assembly,
                        Algorithm = UnkeyedCryptoAlgorithm.Sha256,
                        ReadByteForByteChecksum = true
                    }
                },
                new object[]
                {
                    new AssemblyCacheBustingOptions()
                    {
                        Assembly = typeof(AssemblyCacheBustingTest).Assembly,
                        Algorithm = UnkeyedCryptoAlgorithm.Sha384,
                        ReadByteForByteChecksum = true
                    }
                },
                new object[]
                {
                    new AssemblyCacheBustingOptions()
                    {
                        Assembly = typeof(AssemblyCacheBustingTest).Assembly,
                        Algorithm = UnkeyedCryptoAlgorithm.Sha512,
                        ReadByteForByteChecksum = true
                    }
                }
            };

            return parameters;
        }
    }
}