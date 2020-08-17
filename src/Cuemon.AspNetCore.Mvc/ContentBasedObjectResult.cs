using Cuemon.Data.Integrity;
using Cuemon.Security.Cryptography;

namespace Cuemon.AspNetCore.Mvc
{
    internal class ContentBasedObjectResult : CacheableObjectResult, IEntityDataIntegrity
    {
        internal ContentBasedObjectResult(object instance, byte[] checksum, bool isWeak = false) : base(instance)
        {
            Checksum = new HashResult(checksum);
            Validation = checksum == null || checksum.Length == 0 
                ? EntityDataIntegrityStrength.Unspecified 
                : isWeak
                    ? EntityDataIntegrityStrength.Weak
                    : EntityDataIntegrityStrength.Strong;
        }

        public EntityDataIntegrityStrength Validation { get; }

        public HashResult Checksum { get; }
    }

    internal class ContentBasedObjectResult<T> : ContentBasedObjectResult
    {
        internal ContentBasedObjectResult(T instance, byte[] checksum, bool isWeak = false) : base(instance, checksum, isWeak)
        {
        }
    }
}