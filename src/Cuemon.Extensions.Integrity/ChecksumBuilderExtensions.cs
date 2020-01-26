using System.Collections.Generic;
using System.Linq;
using Cuemon.Integrity;

namespace Cuemon.Extensions.Integrity
{
    /// <summary>
    /// Extension methods for the <see cref="ChecksumBuilder"/> class.
    /// </summary>
    public static class ChecksumBuilderExtensions
    {
        /// <summary>
        /// Combines the <paramref name="additionalChecksum"/> to the representation of this instance.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="ChecksumBuilder"/>.</typeparam>
        /// <param name="cb">An instance of a <see cref="ChecksumBuilder"/>.</param>
        /// <param name="additionalChecksum">A <see cref="double"/> array that contains zero or more checksum of the additional data this instance must represent.</param>
        /// <returns>An updated instance the specified <paramref name="cb"/> of <typeparamref name="T"/>.</returns>
        public static T CombineWith<T>(this T cb, params double[] additionalChecksum) where T : ChecksumBuilder
        {
            return CombineWith(cb, additionalChecksum?.SelectMany(x => Convertible.GetBytes(x)).ToArray());
        }

        /// <summary>
        /// Combines the <paramref name="additionalChecksum"/> to the representation of this instance.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="ChecksumBuilder"/>.</typeparam>
        /// <param name="cb">An instance of a <see cref="ChecksumBuilder"/>.</param>
        /// <param name="additionalChecksum">An <see cref="short"/> array that contains zero or more checksum of the additional data this instance must represent.</param>
        /// <returns>An updated instance the specified <paramref name="cb"/> of <typeparamref name="T"/>.</returns>
        public static T CombineWith<T>(this T cb, params short[] additionalChecksum) where T : ChecksumBuilder
        {
            return CombineWith(cb, additionalChecksum?.SelectMany(x => Convertible.GetBytes(x)).ToArray());
        }

        /// <summary>
        /// Combines the <paramref name="additionalChecksum"/> to the representation of this instance.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="ChecksumBuilder"/>.</typeparam>
        /// <param name="cb">An instance of a <see cref="ChecksumBuilder"/>.</param>
        /// <param name="additionalChecksum">A <see cref="string"/> array that contains zero or more checksum of the additional data this instance must represent.</param>
        /// <returns>An updated instance the specified <paramref name="cb"/> of <typeparamref name="T"/>.</returns>
        public static T CombineWith<T>(this T cb, params string[] additionalChecksum) where T : ChecksumBuilder
        {
            List<long> result = new List<long>();
            for (int i = 0; i < additionalChecksum.Length; i++)
            {
                result.Add(Generate.HashCode64(additionalChecksum[i]));
            }
            return CombineWith(cb, result.ToArray());
        }

        /// <summary>
        /// Combines the <paramref name="additionalChecksum"/> to the representation of this instance.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="ChecksumBuilder"/>.</typeparam>
        /// <param name="cb">An instance of a <see cref="ChecksumBuilder"/>.</param>
        /// <param name="additionalChecksum">An <see cref="int"/> array that contains zero or more checksum of the additional data this instance must represent.</param>
        /// <returns>An updated instance the specified <paramref name="cb"/> of <typeparamref name="T"/>.</returns>
        public static T CombineWith<T>(this T cb, params int[] additionalChecksum) where T : ChecksumBuilder
        {
            return CombineWith(cb, additionalChecksum?.SelectMany(x => Convertible.GetBytes(x)).ToArray());
        }

        /// <summary>
        /// Combines the <paramref name="additionalChecksum"/> to the representation of this instance.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="ChecksumBuilder"/>.</typeparam>
        /// <param name="cb">An instance of a <see cref="ChecksumBuilder"/>.</param>
        /// <param name="additionalChecksum">An <see cref="long"/> array that contains zero or more checksum of the additional data this instance must represent.</param>
        /// <returns>An updated instance the specified <paramref name="cb"/> of <typeparamref name="T"/>.</returns>
        public static T CombineWith<T>(this T cb, params long[] additionalChecksum) where T : ChecksumBuilder
        {
            return CombineWith(cb, additionalChecksum?.SelectMany(x => Convertible.GetBytes(x)).ToArray());
        }

        /// <summary>
        /// Combines the <paramref name="additionalChecksum"/> to the representation of this instance.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="ChecksumBuilder"/>.</typeparam>
        /// <param name="cb">An instance of a <see cref="ChecksumBuilder"/>.</param>
        /// <param name="additionalChecksum">A <see cref="float"/> array that contains zero or more checksum of the additional data this instance must represent.</param>
        /// <returns>An updated instance the specified <paramref name="cb"/> of <typeparamref name="T"/>.</returns>
        public static T CombineWith<T>(this T cb, params float[] additionalChecksum) where T : ChecksumBuilder
        {
            return CombineWith(cb, additionalChecksum?.SelectMany(x => Convertible.GetBytes(x)).ToArray());
        }

        /// <summary>
        /// Combines the <paramref name="additionalChecksum"/> to the representation of this instance.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="ChecksumBuilder"/>.</typeparam>
        /// <param name="cb">An instance of a <see cref="ChecksumBuilder"/>.</param>
        /// <param name="additionalChecksum">An <see cref="ushort"/> array that contains zero or more checksum of the additional data this instance must represent.</param>
        /// <returns>An updated instance the specified <paramref name="cb"/> of <typeparamref name="T"/>.</returns>
        public static T CombineWith<T>(this T cb, params ushort[] additionalChecksum) where T : ChecksumBuilder
        {
            return CombineWith(cb, additionalChecksum?.SelectMany(x => Convertible.GetBytes(x)).ToArray());
        }

        /// <summary>
        /// Combines the <paramref name="additionalChecksum"/> to the representation of this instance.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="ChecksumBuilder"/>.</typeparam>
        /// <param name="cb">An instance of a <see cref="ChecksumBuilder"/>.</param>
        /// <param name="additionalChecksum">An <see cref="uint"/> array that contains zero or more checksum of the additional data this instance must represent.</param>
        /// <returns>An updated instance the specified <paramref name="cb"/> of <typeparamref name="T"/>.</returns>
        public static T CombineWith<T>(this T cb, params uint[] additionalChecksum) where T : ChecksumBuilder
        {
            return CombineWith(cb, additionalChecksum?.SelectMany(x => Convertible.GetBytes(x)).ToArray());
        }

        /// <summary>
        /// Combines the <paramref name="additionalChecksum"/> to the representation of this instance.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="ChecksumBuilder"/>.</typeparam>
        /// <param name="cb">An instance of a <see cref="ChecksumBuilder"/>.</param>
        /// <param name="additionalChecksum">An <see cref="ulong"/> array that contains zero or more checksum of the additional data this instance must represent.</param>
        /// <returns>An updated instance the specified <paramref name="cb"/> of <typeparamref name="T"/>.</returns>
        public static T CombineWith<T>(this T cb, params ulong[] additionalChecksum) where T : ChecksumBuilder
        {
            return CombineWith(cb, additionalChecksum?.SelectMany(x => Convertible.GetBytes(x)).ToArray());
        }

        /// <summary>
        /// Combines the <paramref name="additionalChecksum" /> to the representation of this instance.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="ChecksumBuilder"/>.</typeparam>
        /// <param name="cb">An instance of a <see cref="ChecksumBuilder"/>.</param>
        /// <param name="additionalChecksum">An array of bytes containing a checksum of the additional data this instance must represent.</param>
        /// <returns>An updated instance the specified <paramref name="cb"/> of <typeparamref name="T"/>.</returns>
        public static T CombineWith<T>(this T cb, params byte[] additionalChecksum) where T : ChecksumBuilder
        {
            if (additionalChecksum == null) { return cb; }
            if (additionalChecksum.Length == 0) { return cb; }
            cb.AppendChecksum(additionalChecksum);
            return cb;
        }
    }
}