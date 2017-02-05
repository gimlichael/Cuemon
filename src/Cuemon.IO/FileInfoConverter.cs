using System;
using System.IO;

namespace Cuemon.IO
{
    /// <summary>
    /// This utility class is designed to make <see cref="FileInfo"/> related conversions easier to work with.
    /// </summary>
    public static class FileInfoConverter
    {
        /// <summary>
        /// Converts the specified <paramref name="fileName"/> to an instance of <typeparamref name="T"/> using the function delegate <paramref name="converter"/>.
        /// </summary>
        /// <typeparam name="T">The type of the converted object.</typeparam>
        /// <param name="fileName">The fully qualified name of the file.</param>
        /// <param name="converter">The function delegate that will handle the conversion.</param>
        /// <returns>An instance of <typeparamref name="T"/>.</returns>
        public static T Convert<T>(string fileName, Func<FileInfo, byte[], T> converter)
        {
            return Convert(fileName, 0, converter);
        }

        /// <summary>
        /// Converts the specified <paramref name="fileName"/> to an instance of <typeparamref name="T"/> using the function delegate <paramref name="converter"/>.
        /// </summary>
        /// <typeparam name="T">The type of the converted object.</typeparam>
        /// <param name="fileName">The fully qualified name of the file.</param>
        /// <param name="bytesToRead">The amount of bytes to read from the specified <paramref name="fileName"/>.</param>
        /// <param name="converter">The function delegate that will handle the conversion.</param>
        /// <returns>An instance of <typeparamref name="T"/>.</returns>
        public static T Convert<T>(string fileName, int bytesToRead, Func<FileInfo, byte[], T> converter)
        {
            FileInfo fi = new FileInfo(fileName);
            if (bytesToRead > 0)
            {
                long buffer = bytesToRead;
                if (fi.Length < buffer) { buffer = fi.Length; }

                byte[] checksumBytes = new byte[buffer];
                using (FileStream openFile = fi.OpenRead())
                {
                    openFile.Read(checksumBytes, 0, (int)buffer);
                }
                return converter(fi, checksumBytes);
            }
            return converter(fi, new byte[0]);
        }
    }
}