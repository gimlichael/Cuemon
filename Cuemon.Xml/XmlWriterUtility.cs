using System;
using System.IO;
using System.Xml;

namespace Cuemon.Xml
{
    /// <summary>
    /// This utility class is designed to make <see cref="XmlWriter"/> related operations easier to work with.
    /// </summary>
    public static class XmlWriterUtility
    {
        /// <summary>
        /// Creates and returns a XML stream by the specified delegate <paramref name="writer"/>.
        /// </summary>
        /// <param name="writer">The delegate that will create an in-memory XML stream.</param>
        /// <param name="setup">The <see cref="XmlWriterSettings"/> which need to be configured.</param>
        /// <returns>A <see cref="Stream"/> holding the XML created by the delegate <paramref name="writer"/>.</returns>
        public static Stream CreateStream(Action<XmlWriter> writer, Action<XmlWriterSettings> setup = null)
        {
            var options = Patterns.Configure(setup);
            return Disposable.SafeInvoke(() => new MemoryStream(), ms =>
            {
                using (var w = XmlWriter.Create(ms, options))
                {
                    writer(w);
                    w.Flush();
                }
                ms.Flush();
                ms.Position = 0;
                return ms;
            }, exception => throw new InvalidOperationException("There is an error in the XML document.", exception));
        }
    }
}