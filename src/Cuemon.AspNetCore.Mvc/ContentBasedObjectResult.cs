using Cuemon.Integrity;

namespace Cuemon.AspNetCore.Mvc
{
    internal class ContentBasedObjectResult : CacheableObjectResult, ICacheableIntegrity
    {
        internal ContentBasedObjectResult(object instance, byte[] checksum, bool isWeak = false) : base(instance)
        {
            Checksum = new HashResult(checksum);
            Validation = checksum == null || checksum.Length == 0 
                ? ChecksumStrength.None 
                : isWeak
                    ? ChecksumStrength.Weak
                    : ChecksumStrength.Strong;
        }

        public ChecksumStrength Validation { get; }

        public HashResult Checksum { get; }
    }

    internal class ContentBasedObjectResult<T> : ContentBasedObjectResult
    {
        internal ContentBasedObjectResult(T instance, byte[] checksum, bool isWeak = false) : base(instance, checksum, isWeak)
        {
        }
    }
}