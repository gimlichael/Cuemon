using System;
using Cuemon.Integrity;

namespace Cuemon.AspNetCore.Mvc
{
    internal class ContentTimeBasedObjectResult :  CacheableObjectResult, ICacheableEntity
    {
        internal ContentTimeBasedObjectResult(object instance, ICacheableTimestamp timestamp, ICacheableIntegrity integrity) : base(instance)
        {
            Created = timestamp.Created;
            Checksum = integrity.Checksum;
            Modified = timestamp.Modified;
            Validation = integrity.Validation;
        }

        public DateTime Created { get; set; }

        public DateTime? Modified { get; set; }

        public ChecksumStrength Validation { get; set; }

        public HashResult Checksum { get; set; }
    }

    internal class ContentTimeBasedObjectResult<T> : ContentTimeBasedObjectResult
    {
        internal ContentTimeBasedObjectResult(T instance, ICacheableTimestamp timestamp, ICacheableIntegrity integrity) : base(instance, timestamp, integrity)
        {
        }
    }
}