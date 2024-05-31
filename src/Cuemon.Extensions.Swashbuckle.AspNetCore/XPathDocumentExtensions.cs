using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.XPath;

namespace Cuemon.Extensions.Swashbuckle.AspNetCore
{
    /// <summary>
    /// Extension methods for the <see cref="XPathDocument"/> class.
    /// </summary>
    public static class XPathDocumentExtensions
    {
        /// <summary>
        /// Adds the specified <paramref name="type"/> to the collection of <paramref name="documents"/>.
        /// </summary>
        /// <param name="documents">The collection of documents in XML format.</param>
        /// <param name="type">The type to locate XML documentation files by.</param>
        /// <returns>A reference to <paramref name="documents" /> so that additional calls can be chained.</returns>
        public static IList<XPathDocument> AddByType(this IList<XPathDocument> documents, Type type)
        {
            Validator.ThrowIfNull(type);
            return AddByAssembly(documents, type.Assembly);
        }

        /// <summary>
        /// Adds the specified <paramref name="assembly"/> to the collection of <paramref name="documents"/>.
        /// </summary>
        /// <param name="documents">The collection of documents in XML format.</param>
        /// <param name="assembly">The assembly to locate XML documentation files by.</param>
        /// <returns>A reference to <paramref name="documents" /> so that additional calls can be chained.</returns>
        public static IList<XPathDocument> AddByAssembly(this IList<XPathDocument> documents, Assembly assembly)
        {
            Validator.ThrowIfNull(assembly);
            if (assembly.IsDynamic || string.IsNullOrWhiteSpace(assembly.Location)) { return documents; }
            var path = Path.ChangeExtension(assembly.Location, ".xml");
            return AddByFilename(documents, path);
        }

        /// <summary>
        /// Adds the specified <paramref name="path"/> to the collection of <paramref name="documents"/>.
        /// </summary>
        /// <param name="documents">The collection of documents in XML format.</param>
        /// <param name="path">The path to locate XML documentation files by.</param>
        /// <returns>A reference to <paramref name="documents" /> so that additional calls can be chained.</returns>
        public static IList<XPathDocument> AddByFilename(this IList<XPathDocument> documents, string path)
        {
            Validator.ThrowIfNull(documents);
            Validator.ThrowIfNullOrWhitespace(path);
            if (File.Exists(path))
            {
                documents.Add(new XPathDocument(path));
            }
            return documents;
        }
    }
}
