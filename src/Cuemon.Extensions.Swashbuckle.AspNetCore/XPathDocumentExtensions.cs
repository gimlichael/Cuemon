using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.XPath;

namespace Cuemon.Extensions.Swashbuckle.AspNetCore
{
    public static class XPathDocumentExtensions
    {
        public static IList<XPathDocument> AddByType(this IList<XPathDocument> documents, Type type)
        {
            Validator.ThrowIfNull(type, nameof(type));
            return AddByAssembly(documents, type.Assembly);
        }

        public static IList<XPathDocument> AddByAssembly(this IList<XPathDocument> documents, Assembly assembly)
        {
            Validator.ThrowIfNull(assembly, nameof(assembly));
            if (assembly.IsDynamic || string.IsNullOrWhiteSpace(assembly.Location)) { return documents; }
            var path = Path.ChangeExtension(assembly.Location, ".xml");
            return AddByFilename(documents, path);
        }

        public static IList<XPathDocument> AddByFilename(this IList<XPathDocument> documents, string path)
        {
            Validator.ThrowIfNull(documents, nameof(documents));
            Validator.ThrowIfNullOrWhitespace(path, nameof(path));
            if (File.Exists(path))
            {
                documents.Add(new XPathDocument(path));
            }
            return documents;
        }
    }
}
