using System;
using System.Text;
using Cuemon.Extensions.Xunit;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.Integrity
{
    public class HashFactoryTest : Test
    {
        public HashFactoryTest(ITestOutputHelper output = null) : base(output)
        {
        }

        [Fact]
        public void CreateCrc_Crc64_ShouldBeValidHashResult()
        {
            var h = HashFactory.CreateCrc(CyclicRedundancyCheckAlgorithm.Crc64);
            Assert.Equal("5CA18585B92C58B9", h.ComputeHash(Alphanumeric.LettersAndNumbers).ToHexadecimalString().ToUpper());
        }

        [Fact]
        public void CreateCrc_Crc64GoIso_ShouldBeValidHashResult()
        {
            var h = HashFactory.CreateCrc(CyclicRedundancyCheckAlgorithm.Crc64GoIso);
            Assert.Equal("8400E49282258B59", h.ComputeHash(Alphanumeric.LettersAndNumbers).ToHexadecimalString().ToUpper());
        }

        [Fact]
        public void CreateCrc_Crc64We_ShouldBeValidHashResult()
        {
            var h = HashFactory.CreateCrc(CyclicRedundancyCheckAlgorithm.Crc64We);
            Assert.Equal("A36DA8F71E78B6FB", h.ComputeHash(Alphanumeric.LettersAndNumbers).ToHexadecimalString().ToUpper());
        }

        [Fact]
        public void CreateCrc_Crc64Xz_ShouldBeValidHashResult()
        {
            var h = HashFactory.CreateCrc(CyclicRedundancyCheckAlgorithm.Crc64Xz);
            Assert.Equal("0305BFE116B75626", h.ComputeHash(Alphanumeric.LettersAndNumbers).ToHexadecimalString().ToUpper());
        }

        [Fact]
        public void CreateCrc_Crc32_ShouldBeValidHashResult()
        {
            var h = HashFactory.CreateCrc();
            Assert.Equal("1fc2e6d2", h.ComputeHash(Alphanumeric.LettersAndNumbers, o => o.Encoding = Encoding.ASCII).ToHexadecimalString());
            Assert.Equal("a684c7c6", h.ComputeHash(Alphanumeric.Numbers).ToHexadecimalString());
            Assert.Equal("aa11d1c3", h.ComputeHash("what-a-feeling-#-¤-%-!-cover-from-dj-bobo-128379539285784289529893278278173981247983251311").ToHexadecimalString());

            h = HashFactory.CreateCrc32(0x4C11DB7, 0xFFFFFFFF, 0xFFFFFFFF, o =>
            {
                o.ReflectInput = false;
                o.ReflectOutput = true;
            });
            Assert.Equal("11842e05", h.ComputeHash(Alphanumeric.LettersAndNumbers, o => o.Encoding = Encoding.ASCII).ToHexadecimalString());
            Assert.Equal("07270d69", h.ComputeHash(Alphanumeric.Numbers).ToHexadecimalString());
            Assert.Equal("4475e1b8", h.ComputeHash("what-a-feeling-#-¤-%-!-cover-from-dj-bobo-128379539285784289529893278278173981247983251311").ToHexadecimalString());

            h = HashFactory.CreateCrc32(0x4C11DB7, 0xFFFFFFFF, 0xFFFFFFFF, o =>
            {
                o.ReflectInput = true;
                o.ReflectOutput = false;
            });
            Assert.Equal("4b6743f8", h.ComputeHash(Alphanumeric.LettersAndNumbers, o => o.Encoding = Encoding.ASCII).ToHexadecimalString());
            Assert.Equal("63e32165", h.ComputeHash(Alphanumeric.Numbers).ToHexadecimalString());
            Assert.Equal("c38b8855", h.ComputeHash("what-a-feeling-#-¤-%-!-cover-from-dj-bobo-128379539285784289529893278278173981247983251311").ToHexadecimalString());
        }

        [Fact]
        public void CreateCrc_Crc32Bzip2_ShouldBeValidHashResult()
        {
            var h = HashFactory.CreateCrc(CyclicRedundancyCheckAlgorithm.Crc32Bzip2);
            Assert.Equal("a0742188", h.ComputeHash(Alphanumeric.LettersAndNumbers, o => o.Encoding = Encoding.ASCII).ToHexadecimalString());
            Assert.Equal("96b0e4e0", h.ComputeHash(Alphanumeric.Numbers).ToHexadecimalString());
            Assert.Equal("1d87ae22", h.ComputeHash("what-a-feeling-#-¤-%-!-cover-from-dj-bobo-128379539285784289529893278278173981247983251311").ToHexadecimalString());
        }

        [Fact]
        public void CreateCrc_Crc32C_ShouldBeValidHashResult()
        {
            var h = HashFactory.CreateCrc(CyclicRedundancyCheckAlgorithm.Crc32C);
            Assert.Equal("A245D57D", h.ComputeHash(Alphanumeric.LettersAndNumbers).ToHexadecimalString().ToUpper());
        }

        [Fact]
        public void CreateCrc_Crc32D_ShouldBeValidHashResult()
        {
            var h = HashFactory.CreateCrc(CyclicRedundancyCheckAlgorithm.Crc32D);
            Assert.Equal("17578C36", h.ComputeHash(Alphanumeric.LettersAndNumbers).ToHexadecimalString().ToUpper());
        }

        [Fact]
        public void CreateCrc_Crc32Mpeg2_ShouldBeValidHashResult()
        {
            var h = HashFactory.CreateCrc(CyclicRedundancyCheckAlgorithm.Crc32Mpeg2);
            Assert.Equal("5F8BDE77", h.ComputeHash(Alphanumeric.LettersAndNumbers).ToHexadecimalString().ToUpper());
        }

        [Fact]
        public void CreateCrc_Crc32Posix_ShouldBeValidHashResult()
        {
            var h = HashFactory.CreateCrc(CyclicRedundancyCheckAlgorithm.Crc32Posix);
            Assert.Equal("D96EF8CF", h.ComputeHash(Alphanumeric.LettersAndNumbers).ToHexadecimalString().ToUpper());
        }

        [Fact]
        public void CreateCrc_Crc32Q_ShouldBeValidHashResult()
        {
            var h = HashFactory.CreateCrc(CyclicRedundancyCheckAlgorithm.Crc32Q);
            Assert.Equal("071381D3", h.ComputeHash(Alphanumeric.LettersAndNumbers).ToHexadecimalString().ToUpper());
        }

        [Fact]
        public void CreateCrc_Crc32Jamcrc_ShouldBeValidHashResult()
        {
            var h = HashFactory.CreateCrc(CyclicRedundancyCheckAlgorithm.Crc32Jamcrc);
            Assert.Equal("E03D192D", h.ComputeHash(Alphanumeric.LettersAndNumbers).ToHexadecimalString().ToUpper());
        }

        [Fact]
        public void CreateCrc_Crc32Xfer_ShouldBeValidHashResult()
        {
            var h = HashFactory.CreateCrc(CyclicRedundancyCheckAlgorithm.Crc32Xfer);
            Assert.Equal("E0334EE2", h.ComputeHash(Alphanumeric.LettersAndNumbers).ToHexadecimalString().ToUpper());
        }

        [Fact]
        public void CreateCrc_Crc32Autosar_ShouldBeValidHashResult()
        {
            var h = HashFactory.CreateCrc(CyclicRedundancyCheckAlgorithm.Crc32Autosar);
            Assert.Equal("ADD7278E", h.ComputeHash(Alphanumeric.LettersAndNumbers).ToHexadecimalString().ToUpper());
        }

        [Fact]
        public void CreateCrc_Crc32CdRomEdc_ShouldBeValidHashResult()
        {
            var h = HashFactory.CreateCrc(CyclicRedundancyCheckAlgorithm.Crc32CdRomEdc);
            Assert.Equal("D9B8B5E9", h.ComputeHash(Alphanumeric.LettersAndNumbers).ToHexadecimalString().ToUpper());
        }

        [Fact]
        public void CreateHmacCryptoMd5_ShouldBeValidHashResult()
        {
            var h = HashFactory.CreateHmacCryptoMd5(Decorator.Enclose("unittest").ToByteArray());
            Assert.Equal("d0ee1decd115feac4608976f28e19e10", h.ComputeHash(Alphanumeric.LettersAndNumbers).ToHexadecimalString());
            Assert.Equal("34bdd10e5c71eb6860294d19f2db8233", h.ComputeHash(Alphanumeric.Numbers).ToHexadecimalString());
            Assert.Equal("70adffcc026ac757db66de1bbb005e04", h.ComputeHash("what-a-feeling-#-¤-%-!-cover-from-dj-bobo-128379539285784289529893278278173981247983251311").ToHexadecimalString());
        }

        [Fact]
        public void CreateHmacCryptoSha1_ShouldBeValidHashResult()
        {
            var h = HashFactory.CreateHmacCryptoSha1(Decorator.Enclose("unittest").ToByteArray());
            Assert.Equal("0c7d9d4461c66d2d6eafef7211689abd20b28b39", h.ComputeHash(Alphanumeric.LettersAndNumbers).ToHexadecimalString());
            Assert.Equal("62ea258e7e4d0522fe5e2b06e1ab600051c542c4", h.ComputeHash(Alphanumeric.Numbers).ToHexadecimalString());
            Assert.Equal("bae9d2e09c9b5f65f9a7a3c5a55748b180ee65c1", h.ComputeHash("what-a-feeling-#-¤-%-!-cover-from-dj-bobo-128379539285784289529893278278173981247983251311").ToHexadecimalString());
        }

        [Fact]
        public void CreateHmacCryptoSha256_ShouldBeValidHashResult()
        {
            var h = HashFactory.CreateHmacCryptoSha256(Decorator.Enclose("unittest").ToByteArray());
            Assert.Equal("c06272f221bcbfc6783c059e85d7ecdb412e80f2c2b704c27885d64d984d95e3", h.ComputeHash(Alphanumeric.LettersAndNumbers).ToHexadecimalString());
            Assert.Equal("1fd4ff25f71fa49435bc6996bbbaeeb9abe2f47ed3256b8a9f44b8eea49a23b5", h.ComputeHash(Alphanumeric.Numbers).ToHexadecimalString());
            Assert.Equal("38367544d02183414967e4669cd298ca6d644db6c6bfa903afae497c6ab300fa", h.ComputeHash("what-a-feeling-#-¤-%-!-cover-from-dj-bobo-128379539285784289529893278278173981247983251311").ToHexadecimalString());
        }

        [Fact]
        public void CreateHmacCryptoSha384_ShouldBeValidHashResult()
        {
            var h = HashFactory.CreateHmacCryptoSha384(Decorator.Enclose("unittest").ToByteArray());
            Assert.Equal("87d90ce62441fd39f81d327fb4d3a9902d80a0d8312a961fdfc9b1bcf756afd77f0f0596d30a868a3cda1c49abf1a3ee", h.ComputeHash(Alphanumeric.LettersAndNumbers).ToHexadecimalString());
            Assert.Equal("7a90151a628de76431d11cf01200a7213f0dc491f7677aafbcd232e512521ca1ac3771d2b6204fa1ab6b54cf085ba2d3", h.ComputeHash(Alphanumeric.Numbers).ToHexadecimalString());
            Assert.Equal("3b00f1f207819422761a22b918c69a3e16ae0d5268c01ad331311184b6a809b927b324f75d8cb8f1cee0f849382558bd", h.ComputeHash("what-a-feeling-#-¤-%-!-cover-from-dj-bobo-128379539285784289529893278278173981247983251311").ToHexadecimalString());
        }

        [Fact]
        public void CreateHmacCryptoSha512_ShouldBeValidHashResult()
        {
            var h = HashFactory.CreateHmacCryptoSha512(Decorator.Enclose("unittest").ToByteArray());
            Assert.Equal("db0087dde1ad907c11037243bf700a9f08430899edfc19fe1fadb7d4e6846182fa0100762cf42c64828f1bfe41493275b98f5c0c1a80300656e0d97f9f1d892e", h.ComputeHash(Alphanumeric.LettersAndNumbers).ToHexadecimalString());
            Assert.Equal("7b674e1138c2ee91828fcf5318d356e72a9c9e4e533234181640b0d1728f305db656bb7e57c95e101c0efbed9290454ef0c37f514c8dfb85f003c483bf0f236a", h.ComputeHash(Alphanumeric.Numbers).ToHexadecimalString());
            Assert.Equal("33520bc89652184cf5a17600a78a513db44a0b7eb1fb001525ab0e77c94afcb45b7878b259cd37896ddbbc5dff76d2cbb8f57eaf44c4f4aac77695fe8fe28992", h.ComputeHash("what-a-feeling-#-¤-%-!-cover-from-dj-bobo-128379539285784289529893278278173981247983251311").ToHexadecimalString());
        }
        
        [Fact]
        public void CreateCryptoSha512_ShouldBeValidHashResult()
        {
            var h = HashFactory.CreateCryptoSha512();
            Assert.Equal("1e07be23c26a86ea37ea810c8ec7809352515a970e9253c26f536cfc7a9996c45c8370583e0a78fa4a90041d71a4ceab7423f19c71b9d5a3e01249f0bebd5894", h.ComputeHash(Alphanumeric.LettersAndNumbers).ToHexadecimalString());
            Assert.Equal("bb96c2fc40d2d54617d6f276febe571f623a8dadf0b734855299b0e107fda32cf6b69f2da32b36445d73690b93cbd0f7bfc20e0f7f28553d2a4428f23b716e90", h.ComputeHash(Alphanumeric.Numbers).ToHexadecimalString());
            Assert.Equal("0b6cbac838dfe7f47ea1bd0df00ec282fdf45510c92161072ccfb84035390c4da743d9c3b954eaa1b0f86fc9861b23cc6c8667ab232c11c686432ebb5c8c3f27", h.ComputeHash(Guid.Empty.ToByteArray()).ToHexadecimalString());
        }

        [Fact]
        public void CreateCryptoSha384_ShouldBeValidHashResult()
        {
            var h = HashFactory.CreateCryptoSha384();
            Assert.Equal("1761336e3f7cbfe51deb137f026f89e01a448e3b1fafa64039c1464ee8732f11a5341a6f41e0c202294736ed64db1a84", h.ComputeHash(Alphanumeric.LettersAndNumbers).ToHexadecimalString());
            Assert.Equal("90ae531f24e48697904a4d0286f354c50a350ebb6c2b9efcb22f71c96ceaeffc11c6095e9ca0df0ec30bf685dcf2e5e5", h.ComputeHash(Alphanumeric.Numbers).ToHexadecimalString());
            Assert.Equal("7210af19145ec2a8e250a7fe8e9eeeac1301e524daab82366c36be614dc35402a289101e48cad61c45337f2f32c14fdc", h.ComputeHash(VerticalDirection.Up).ToHexadecimalString());
        }

        [Fact]
        public void CreateCryptoSha256_ShouldBeValidHashResult()
        {
            var h = HashFactory.CreateCryptoSha256();
            Assert.Equal("db4bfcbd4da0cd85a60c3c37d3fbd8805c77f15fc6b1fdfe614ee0a7c8fdb4c0", h.ComputeHash(Alphanumeric.LettersAndNumbers).ToHexadecimalString());
            Assert.Equal("db4bfcbd4da0cd85a60c3c37d3fbd8805c77f15fc6b1fdfe614ee0a7c8fdb4c0", h.ComputeHash(Decorator.Enclose(Alphanumeric.LettersAndNumbers).ToStream()).ToHexadecimalString());
            Assert.Equal("84d89877f0d4041efb6bf91a16f0248f2fd573e6af05c19f96bedb9f882f7882", h.ComputeHash(Alphanumeric.Numbers).ToHexadecimalString());
            Assert.Equal("53ab3a50f51855beeae9721ab68656312c7f105b9b34bbfa97875dbfda72dbc6", h.ComputeHash(DateTime.UnixEpoch).ToHexadecimalString());
            Assert.Equal("1f1a24c833be74a0f4f99007aa70a51e2456e41f745a5628721ea2b8e1c07641", h.ComputeHash(213, "fdfsfsf", 9999).ToHexadecimalString());
        }

        [Fact]
        public void CreateCryptoSha1_ShouldBeValidHashResult()
        {
            var h = HashFactory.CreateCryptoSha1();
            Assert.Equal("761c457bf73b14d27e9e9265c46f4b4dda11f940", h.ComputeHash(Alphanumeric.LettersAndNumbers).ToHexadecimalString());
            Assert.Equal("761c457bf73b14d27e9e9265c46f4b4dda11f940", h.ComputeHash(Decorator.Enclose(Alphanumeric.LettersAndNumbers).ToStream()).ToHexadecimalString());
            Assert.Equal("87acec17cd9dcd20a716cc2cf67417b71c8a7016", h.ComputeHash(Alphanumeric.Numbers).ToHexadecimalString());
            Assert.Equal("485bad9954c874513b8ff7b6d5ea459ac65dd075", h.ComputeHash(decimal.MinValue).ToHexadecimalString());
            Assert.Equal("ec9a4348d9ffcb19403f5b90e9eefe4cedcd6ee1", h.ComputeHash(43402934324).ToHexadecimalString());
        }

        [Fact]
        public void CreateCryptoMd5_LittleEndian_ShouldBeValidHashResult()
        {
            var h = HashFactory.CreateCryptoMd5(o => o.ByteOrder = Endianness.LittleEndian);
            Assert.Equal("193112cee1d1a35660f02e95a28bfea2", h.ComputeHash(32131535).ToHexadecimalString());
            Assert.Equal("d174ab98d277d9f5a5611c2c9f419d9f", h.ComputeHash(Alphanumeric.LettersAndNumbers).ToHexadecimalString());
            Assert.Equal("d174ab98d277d9f5a5611c2c9f419d9f", h.ComputeHash(Decorator.Enclose(Alphanumeric.LettersAndNumbers).ToStream()).ToHexadecimalString());
            Assert.Equal("781e5e245d69b566979b86e28d23f2c7", h.ComputeHash(Alphanumeric.Numbers).ToHexadecimalString());
            Assert.Equal("612f309087565745ca61c53fcaf6fa7d", h.ComputeHash(212).ToHexadecimalString());
        }

        [Fact]
        public void CreateCryptoMd5_BigEndian_ShouldBeValidHashResult()
        {
            var h = HashFactory.CreateCryptoMd5(o => o.ByteOrder = Endianness.BigEndian);
            Assert.Equal("d52654efa276b2ab12ca067dccaf953f", h.ComputeHash(32131535).ToHexadecimalString());
            Assert.Equal("d174ab98d277d9f5a5611c2c9f419d9f", h.ComputeHash(Alphanumeric.LettersAndNumbers).ToHexadecimalString());
            Assert.Equal("d174ab98d277d9f5a5611c2c9f419d9f", h.ComputeHash(Decorator.Enclose(Alphanumeric.LettersAndNumbers).ToStream()).ToHexadecimalString());
            Assert.Equal("781e5e245d69b566979b86e28d23f2c7", h.ComputeHash(Alphanumeric.Numbers).ToHexadecimalString());
            Assert.Equal("fb7b8d4f62e2be708ceca1114b439d5d", h.ComputeHash(212).ToHexadecimalString());
        }

        [Fact]
        public void CreateFnv32_Fnv1_ShouldBeValidHashResult()
        {
            var h = HashFactory.CreateFnv32(o => o.Algorithm = FowlerNollVoAlgorithm.Fnv1);
            Assert.Equal("6792412c", h.ComputeHash(Alphanumeric.Numbers).ToHexadecimalString());
            Assert.Equal("050c5d1f", h.ComputeHash(byte.MinValue).ToHexadecimalString());
        }

        [Fact]
        public void CreateFnv64_Fnv1_ShouldBeValidHashResult()
        {
            var h = HashFactory.CreateFnv64(o => o.Algorithm = FowlerNollVoAlgorithm.Fnv1);
            Assert.Equal("93466e18b44cc858", h.ComputeHash(Alphanumeric.LettersAndNumbers).ToHexadecimalString());
            Assert.Equal("c3f080735df30b0c", h.ComputeHash(Alphanumeric.Numbers).ToHexadecimalString());
            Assert.Equal("af63bd4c8601b7df", h.ComputeHash(byte.MinValue).ToHexadecimalString());
        }

        [Fact]
        public void CreateFnv128_Fnv1_ShouldBeValidHashResult()
        {
            var h = HashFactory.CreateFnv128(o => o.Algorithm = FowlerNollVoAlgorithm.Fnv1);
            Assert.Equal("a9c6ce956469c801ebd62b0bbf9d6bf0", h.ComputeHash(Alphanumeric.LettersAndNumbers).ToHexadecimalString());
            Assert.Equal("eea7dd269d8d5af5f47ac00d5eb7f714", h.ComputeHash(Alphanumeric.Numbers).ToHexadecimalString());
            Assert.Equal("d228cb69101a8caf78912b704e4a147f", h.ComputeHash(byte.MinValue).ToHexadecimalString());
        }

        [Fact]
        public void CreateFnv256_Fnv1_ShouldBeValidHashResult()
        {
            var h = HashFactory.CreateFnv256(o => o.Algorithm = FowlerNollVoAlgorithm.Fnv1);
            Assert.Equal("3ea64656b211bff487a9b62f18ba64392c7068ddf241baf2c41a47514a0ffc48", h.ComputeHash(Alphanumeric.LettersAndNumbers).ToHexadecimalString());
            Assert.Equal("063cfa6dd23b329ac23cc7d1120d59e6a397a0c32d3ba388af79d1d72628225c", h.ComputeHash(Alphanumeric.Numbers).ToHexadecimalString());
            Assert.Equal("63323fb0f35303ec28dc561d0a33bdfa4de6a99b7266494f6183b2716811387f", h.ComputeHash(byte.MinValue).ToHexadecimalString());
        }

        [Fact]
        public void CreateFnv512_Fnv1_ShouldBeValidHashResult()
        {
            var h = HashFactory.CreateFnv512(o => o.Algorithm = FowlerNollVoAlgorithm.Fnv1);
            Assert.Equal("593826b252ba64905c4832380622bd2c4e177cf0fd7cb9cd72f2da0638ec7fc82beaa91153430a76042dffa611435e70bd754d44aa59e2c93ccb0b926efc7464", h.ComputeHash(Alphanumeric.LettersAndNumbers).ToHexadecimalString());
            Assert.Equal("fa7eb91efb6464118a735adc66e062a777fa3da6e3d7d3e73728da570c1fafc3d06e4dd9534a9fd4a52c438bd21169834ae60d20834d4f408192203a729b3ef0", h.ComputeHash(Alphanumeric.Numbers).ToHexadecimalString());
            Assert.Equal("e43a992dc8fc5ad7de493e3d696d6f85d64326ec28000000000000000011986f90c2532caf5be7d88291baa894a395225328b196bd6a8a643fe12cd87b282bbf", h.ComputeHash(byte.MinValue).ToHexadecimalString());
        }

        [Fact]
        public void CreateFnv1024_Fnv1_ShouldBeValidHashResult()
        {
            var h = HashFactory.CreateFnv1024(o => o.Algorithm = FowlerNollVoAlgorithm.Fnv1);
            Assert.Equal("70e427242b62d481df8f97b5a7c389f5f6df3457fda072841eb0ac24759648a39784a0ab922c4730b68efa7d0980de290e79de582d88c97e17c953592b9b70ce6dca5ccd19cd93182254abfe9ed6face84979b6793e44e46621ad88c76744b1296ed3934a03e443ce593f1d3dd137dcba2ac2c5edb2cc9c7353111c2327224ca", h.ComputeHash(Alphanumeric.LettersAndNumbers).ToHexadecimalString());
            Assert.Equal("c801f8e08ae91b180b98dd7d9f65ceb687ca86358c6905f60a7d1014c182b04f441590d012afb5871d0f57000000000000000000000000000000000000000000000000000000000000000000000000000000018045149ade1c79abe3b709a406f7d9205169bec59b126140bcb96f9d5d3e2ea91e0b2b52fa8d2d0d70ecdaeab2", h.ComputeHash(Alphanumeric.Numbers).ToHexadecimalString());
            Assert.Equal("98d7c19fbce653df221b9f717d3490ff95ca87fdaef30d1b823372f85b24a372f50e380000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000007685cd81a491dbccc21ad06648d09a5c8cf5a78482054e91470b33dde77252caef66597", h.ComputeHash(byte.MinValue).ToHexadecimalString());
        }

        [Fact]
        public void CreateFnv32_Fnv1a_ShouldBeValidHashResult()
        {
            var h = HashFactory.CreateFnv32(o => o.Algorithm = FowlerNollVoAlgorithm.Fnv1a);
            Assert.Equal("9b2bce4e", h.ComputeHash(Decorator.Enclose(Alphanumeric.LettersAndNumbers).ToStream()).ToHexadecimalString());
            Assert.Equal("f9808ff2", h.ComputeHash(Alphanumeric.Numbers).ToHexadecimalString());
            Assert.Equal("050c5d1f", h.ComputeHash(byte.MinValue).ToHexadecimalString());
        }

        [Fact]
        public void CreateFnv64_Fnv1a_ShouldBeValidHashResult()
        {
            var h = HashFactory.CreateFnv64(o => o.Algorithm = FowlerNollVoAlgorithm.Fnv1a);
            Assert.Equal("c35365f271d8c80e", h.ComputeHash(Decorator.Enclose(Alphanumeric.LettersAndNumbers).ToStream()).ToHexadecimalString());
            Assert.Equal("50c0aafd8b4330b2", h.ComputeHash(Alphanumeric.Numbers).ToHexadecimalString());
            Assert.Equal("af63bd4c8601b7df", h.ComputeHash(byte.MinValue).ToHexadecimalString());
        }

        [Fact]
        public void CreateFnv128_Fnv1a_ShouldBeValidHashResult()
        {
            var h = HashFactory.CreateFnv128(o => o.Algorithm = FowlerNollVoAlgorithm.Fnv1a);
            Assert.Equal("849d079e7ac91227b14ecbd5246bb93e", h.ComputeHash(Decorator.Enclose(Alphanumeric.LettersAndNumbers).ToStream()).ToHexadecimalString());
            Assert.Equal("0b7df68bb60da90266201c9330963d52", h.ComputeHash(Alphanumeric.Numbers).ToHexadecimalString());
            Assert.Equal("d228cb69101a8caf78912b704e4a147f", h.ComputeHash(byte.MinValue).ToHexadecimalString());
        }

        [Fact]
        public void CreateFnv256_Fnv1a_ShouldBeValidHashResult()
        {
            var h = HashFactory.CreateFnv256(o => o.Algorithm = FowlerNollVoAlgorithm.Fnv1a);
            Assert.Equal("16ce73ab46b29992f1d12aff42c3f70fb4d178dfc96513ef1ea856751d21d40e", h.ComputeHash(Decorator.Enclose(Alphanumeric.LettersAndNumbers).ToStream()).ToHexadecimalString());
            Assert.Equal("e2bdd7f76e05160bc86d1ed1120d59e6a397a0c3284fbcafaaa8566d5f79ebd2", h.ComputeHash(Alphanumeric.Numbers).ToHexadecimalString());
            Assert.Equal("63323fb0f35303ec28dc561d0a33bdfa4de6a99b7266494f6183b2716811387f", h.ComputeHash(byte.MinValue).ToHexadecimalString());
        }
        
        [Fact]
        public void CreateFnv512_Fnv1a_ShouldBeValidHashResult()
        {
            var h = HashFactory.CreateFnv512(o => o.Algorithm = FowlerNollVoAlgorithm.Fnv1a);
            Assert.Equal("d67158511cb86ff6a655a5cc30b6957faa2d4a113d88d7d4782641fbb3f1b034fa859ea95e767c2b93479b4e38977c409dab6cce192179923d0d3ffc10a7a076", h.ComputeHash(Decorator.Enclose(Alphanumeric.LettersAndNumbers).ToStream()).ToHexadecimalString());
            Assert.Equal("fa7eb91efb6464118a73639a14a22d3284c5433232d7d3e73728da570c1fafc3d06e4dd9534a9fd4a52c438bd21169834ae60d2084794a3e4e815976ca1e01f2", h.ComputeHash(Alphanumeric.Numbers).ToHexadecimalString());
            Assert.Equal("e43a992dc8fc5ad7de493e3d696d6f85d64326ec28000000000000000011986f90c2532caf5be7d88291baa894a395225328b196bd6a8a643fe12cd87b282bbf", h.ComputeHash(byte.MinValue).ToHexadecimalString());
        }

        [Fact]
        public void CreateFnv1024_Fnv1a_ShouldBeValidHashResult()
        {
            var h = HashFactory.CreateFnv1024(o => o.Algorithm = FowlerNollVoAlgorithm.Fnv1a);
            Assert.Equal("199c0ace56c5c33d8bce6f7cf4bc4b555e0fc3ae8d37c4b7384678a34d96ae8192825ae6bcda63dbb9e3417d0980de290e79de582d88c97e17c9535950c35f4f16d311bc66d1ac2892d59f7b0697257eba9fc1e3accbc85729218306b34996eedf99292c814e8a75f41ddc5a5b5177b6e60c0211ad8d8f78395c7c2d2c483e7e", h.ComputeHash(Decorator.Enclose(Alphanumeric.LettersAndNumbers).ToStream()).ToHexadecimalString());
            Assert.Equal("c801f8e08ae91b180b98dd7d9f65ceb687ca86358c6905f60a7d1014c182b04ee2ab1bd0066e9857a7f7de000000000000000000000000000000000000000000000000000000000000000000000000000000018045149ade1c79abe3b709a406f7d9205169bec59b126140bcb96f9d5d3e2ea91dfc0f40af8e7e3f25d14c3186", h.ComputeHash(Alphanumeric.Numbers).ToHexadecimalString());
            Assert.Equal("98d7c19fbce653df221b9f717d3490ff95ca87fdaef30d1b823372f85b24a372f50e380000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000007685cd81a491dbccc21ad06648d09a5c8cf5a78482054e91470b33dde77252caef66597", h.ComputeHash(byte.MinValue).ToHexadecimalString());
        }
    }
}