using System;
using System.Collections.Generic;
using System.Linq;

namespace Cuemon.Data.Integrity
{
    /// <summary>
    /// Extension methods for the <see cref="ChecksumBuilder"/> class tailored to adhere the decorator pattern.
    /// </summary>
    /// <seealso cref="IDecorator{T}"/>
    /// <seealso cref="Decorator{T}"/>
    public static class ChecksumBuilderDecoratorExtensions
    {
        /// <summary>
        /// Combines the <paramref name="additionalChecksum"/> to the representation of the enclosed <see cref="ChecksumBuilder"/> of the <paramref name="decorator"/>.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="ChecksumBuilder"/>.</typeparam>
        /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
        /// <param name="additionalChecksum">A <see cref="double"/> array that contains zero or more checksum of the additional data the enclosed <see cref="ChecksumBuilder"/> of the <paramref name="decorator"/> must represent.</param>
        /// <returns>An updated instance of the enclosed <see cref="ChecksumBuilder"/> of the <paramref name="decorator"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="decorator"/> cannot be null.
        /// </exception>
        public static T CombineWith<T>(this IDecorator<T> decorator, params double[] additionalChecksum) where T : ChecksumBuilder
        {
            return CombineWith(decorator, additionalChecksum?.SelectMany(x => Convertible.GetBytes(x)).ToArray());
        }

        /// <summary>
        /// Combines the <paramref name="additionalChecksum"/> to the representation of this instance.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="ChecksumBuilder"/>.</typeparam>
        /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
        /// <param name="additionalChecksum">An <see cref="short"/> array that contains zero or more checksum of the additional data the enclosed <see cref="ChecksumBuilder"/> of the <paramref name="decorator"/> must represent.</param>
        /// <returns>An updated instance of the enclosed <see cref="ChecksumBuilder"/> of the <paramref name="decorator"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="decorator"/> cannot be null.
        /// </exception>
        public static T CombineWith<T>(this IDecorator<T> decorator, params short[] additionalChecksum) where T : ChecksumBuilder
        {
            return CombineWith(decorator, additionalChecksum?.SelectMany(x => Convertible.GetBytes(x)).ToArray());
        }

        /// <summary>
        /// Combines the <paramref name="additionalChecksum"/> to the representation of this instance.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="ChecksumBuilder"/>.</typeparam>
        /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
        /// <param name="additionalChecksum">A <see cref="string"/> array that contains zero or more checksum of the additional data the enclosed <see cref="ChecksumBuilder"/> of the <paramref name="decorator"/> must represent.</param>
        /// <returns>An updated instance of the enclosed <see cref="ChecksumBuilder"/> of the <paramref name="decorator"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="decorator"/> cannot be null.
        /// </exception>
        public static T CombineWith<T>(this IDecorator<T> decorator, params string[] additionalChecksum) where T : ChecksumBuilder
        {
            var result = new List<long>();
            for (int i = 0; i < additionalChecksum.Length; i++)
            {
                result.Add(Generate.HashCode64(additionalChecksum[i]));
            }
            return CombineWith(decorator, result.ToArray());
        }

        /// <summary>
        /// Combines the <paramref name="additionalChecksum"/> to the representation of this instance.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="ChecksumBuilder"/>.</typeparam>
        /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
        /// <param name="additionalChecksum">An <see cref="int"/> array that contains zero or more checksum of the additional data the enclosed <see cref="ChecksumBuilder"/> of the <paramref name="decorator"/> must represent.</param>
        /// <returns>An updated instance of the enclosed <see cref="ChecksumBuilder"/> of the <paramref name="decorator"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="decorator"/> cannot be null.
        /// </exception>
        public static T CombineWith<T>(this IDecorator<T> decorator, params int[] additionalChecksum) where T : ChecksumBuilder
        {
            return CombineWith(decorator, additionalChecksum?.SelectMany(x => Convertible.GetBytes(x)).ToArray());
        }

        /// <summary>
        /// Combines the <paramref name="additionalChecksum"/> to the representation of this instance.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="ChecksumBuilder"/>.</typeparam>
        /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
        /// <param name="additionalChecksum">An <see cref="long"/> array that contains zero or more checksum of the additional data the enclosed <see cref="ChecksumBuilder"/> of the <paramref name="decorator"/> must represent.</param>
        /// <returns>An updated instance of the enclosed <see cref="ChecksumBuilder"/> of the <paramref name="decorator"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="decorator"/> cannot be null.
        /// </exception>
        public static T CombineWith<T>(this IDecorator<T> decorator, params long[] additionalChecksum) where T : ChecksumBuilder
        {
            return CombineWith(decorator, additionalChecksum?.SelectMany(x => Convertible.GetBytes(x)).ToArray());
        }

        /// <summary>
        /// Combines the <paramref name="additionalChecksum"/> to the representation of this instance.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="ChecksumBuilder"/>.</typeparam>
        /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
        /// <param name="additionalChecksum">A <see cref="float"/> array that contains zero or more checksum of the additional data the enclosed <see cref="ChecksumBuilder"/> of the <paramref name="decorator"/> must represent.</param>
        /// <returns>An updated instance of the enclosed <see cref="ChecksumBuilder"/> of the <paramref name="decorator"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="decorator"/> cannot be null.
        /// </exception>
        public static T CombineWith<T>(this IDecorator<T> decorator, params float[] additionalChecksum) where T : ChecksumBuilder
        {
            return CombineWith(decorator, additionalChecksum?.SelectMany(x => Convertible.GetBytes(x)).ToArray());
        }

        /// <summary>
        /// Combines the <paramref name="additionalChecksum"/> to the representation of this instance.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="ChecksumBuilder"/>.</typeparam>
        /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
        /// <param name="additionalChecksum">An <see cref="ushort"/> array that contains zero or more checksum of the additional data the enclosed <see cref="ChecksumBuilder"/> of the <paramref name="decorator"/> must represent.</param>
        /// <returns>An updated instance of the enclosed <see cref="ChecksumBuilder"/> of the <paramref name="decorator"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="decorator"/> cannot be null.
        /// </exception>
        public static T CombineWith<T>(this IDecorator<T> decorator, params ushort[] additionalChecksum) where T : ChecksumBuilder
        {
            return CombineWith(decorator, additionalChecksum?.SelectMany(x => Convertible.GetBytes(x)).ToArray());
        }

        /// <summary>
        /// Combines the <paramref name="additionalChecksum"/> to the representation of this instance.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="ChecksumBuilder"/>.</typeparam>
        /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
        /// <param name="additionalChecksum">An <see cref="uint"/> array that contains zero or more checksum of the additional data the enclosed <see cref="ChecksumBuilder"/> of the <paramref name="decorator"/> must represent.</param>
        /// <returns>An updated instance of the enclosed <see cref="ChecksumBuilder"/> of the <paramref name="decorator"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="decorator"/> cannot be null.
        /// </exception>
        public static T CombineWith<T>(this IDecorator<T> decorator, params uint[] additionalChecksum) where T : ChecksumBuilder
        {
            return CombineWith(decorator, additionalChecksum?.SelectMany(x => Convertible.GetBytes(x)).ToArray());
        }

        /// <summary>
        /// Combines the <paramref name="additionalChecksum"/> to the representation of this instance.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="ChecksumBuilder"/>.</typeparam>
        /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
        /// <param name="additionalChecksum">An <see cref="ulong"/> array that contains zero or more checksum of the additional data the enclosed <see cref="ChecksumBuilder"/> of the <paramref name="decorator"/> must represent.</param>
        /// <returns>An updated instance of the enclosed <see cref="ChecksumBuilder"/> of the <paramref name="decorator"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="decorator"/> cannot be null.
        /// </exception>
        public static T CombineWith<T>(this IDecorator<T> decorator, params ulong[] additionalChecksum) where T : ChecksumBuilder
        {
            return CombineWith(decorator, additionalChecksum?.SelectMany(x => Convertible.GetBytes(x)).ToArray());
        }

        /// <summary>
        /// Combines the <paramref name="additionalChecksum" /> to the representation of this instance.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="ChecksumBuilder"/>.</typeparam>
        /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
        /// <param name="additionalChecksum">An array of bytes containing a checksum of the additional data the enclosed <see cref="ChecksumBuilder"/> of the <paramref name="decorator"/> must represent.</param>
        /// <returns>An updated instance of the enclosed <see cref="ChecksumBuilder"/> of the <paramref name="decorator"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="decorator"/> cannot be null.
        /// </exception>
        public static T CombineWith<T>(this IDecorator<T> decorator, params byte[] additionalChecksum) where T : ChecksumBuilder
        {
            Validator.ThrowIfNull(decorator, nameof(decorator));
            if (additionalChecksum == null) { return decorator.Inner; }
            if (additionalChecksum.Length == 0) { return decorator.Inner; }
            decorator.Inner.AppendChecksum(additionalChecksum);
            return decorator.Inner;
        }
    }
}