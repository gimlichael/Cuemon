using System;
using Cuemon.Security;

namespace Cuemon.Data.Integrity
{
    /// <summary>
    /// Represents the metadata information normally associated with an entity/resource.
    /// Implements the <see cref="IEntityInfo" />
    /// </summary>
    /// <seealso cref="IEntityInfo" />
    public class EntityInfo : IEntityInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityInfo"/> class.
        /// </summary>
        /// <param name="created">A <see cref="DateTime" /> value for when data this instance represents was first created.</param>
        public EntityInfo(DateTime created) : this(created, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityInfo"/> class.
        /// </summary>
        /// <param name="created">A <see cref="DateTime" /> value for when data this instance represents was first created.</param>
        /// <param name="modified">A <see cref="DateTime" /> value for when data this instance represents was last modified.</param>
        public EntityInfo(DateTime created, DateTime? modified) : this(created, modified, null, EntityDataIntegrityValidation.Unspecified)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityInfo"/> class.
        /// </summary>
        /// <param name="created">A <see cref="DateTime" /> value for when data this instance represents was first created.</param>
        /// <param name="modified">A <see cref="DateTime" /> value for when data this instance represents was last modified.</param>
        /// <param name="checksum">A <see cref="T:byte[]"/> containing a checksum of the data this instance represents.</param>
        /// <param name="validation">A <see cref="EntityDataIntegrityValidation"/> enumeration value that indicates the validation strength of the specified <paramref name="checksum"/>. Default is <see cref="EntityDataIntegrityValidation.Weak"/>.</param>
        public EntityInfo(DateTime created, DateTime? modified, byte[] checksum, EntityDataIntegrityValidation validation = EntityDataIntegrityValidation.Weak)
        {
            Created = created.ToUniversalTime();
            Modified = modified?.ToUniversalTime();
            Checksum = new HashResult(checksum);
            Validation = validation;
        }

        public DateTime Created { get; }

        public DateTime? Modified { get; }

        public HashResult Checksum { get; }

        public EntityDataIntegrityValidation Validation { get; }
    }
}