using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;
using Cuemon.Reflection;
using Cuemon.Xml;
using Cuemon.Xml.Serialization;

namespace Cuemon.Extensions.Xml.Serialization
{
    internal static class Infrastructure
    {
        internal static bool HasXmlIgnoreAttribute(this IHierarchy<object> node)
        {
            return node.HasMemberReference && node.MemberReference.HasAttributes(typeof(XmlIgnoreAttribute));
        }

        internal static bool IsNodeEnumerable(this IHierarchy<object> node)
        {
            return TypeUtility.IsEnumerable(node.InstanceType) && (node.InstanceType != typeof(string));
        }

        internal static XmlQualifiedEntity LookupXmlStartElement(this IHierarchy<object> node, XmlQualifiedEntity qualifiedRootEntity = null)
        {
            if (node == null) { throw new ArgumentNullException(nameof(node)); }
            if (qualifiedRootEntity != null && !qualifiedRootEntity.LocalName.IsNullOrWhiteSpace()) { return qualifiedRootEntity; }
            bool hasRootAttribute = TypeUtility.ContainsAttributeType(node.InstanceType, true, typeof(XmlRootAttribute));
            bool hasElementAttribute = node.HasMemberReference && TypeUtility.ContainsAttributeType(node.MemberReference, typeof(XmlElementAttribute));
            string rootOrElementName = XmlUtility.SanitizeElementName(node.HasMemberReference ? node.MemberReference.Name : StringConverter.FromType(node.InstanceType, false, true));
            string ns = null;

            if (hasRootAttribute || hasElementAttribute)
            {
                string elementName = null;
                if (hasRootAttribute)
                {
                    XmlRootAttribute rootAttribute = node.InstanceType.GetTypeInfo().GetCustomAttribute<XmlRootAttribute>(true);
                    elementName = rootAttribute.ElementName;
                    ns = rootAttribute.Namespace;
                }

                if (hasElementAttribute)
                {
                    XmlElementAttribute elementAttribute = node.MemberReference.GetCustomAttribute<XmlElementAttribute>();
                    elementName = elementAttribute.ElementName;
                    ns = elementAttribute.Namespace;
                }

                if (!string.IsNullOrEmpty(elementName))
                {
                    rootOrElementName = elementName;
                }
            }

            XmlQualifiedEntity instance = node.Instance as XmlQualifiedEntity;
            return instance ?? new XmlQualifiedEntity(XmlUtility.SanitizeElementName(rootOrElementName), ns);
        }

        internal static IEnumerable<IHierarchy<T>> OrderByXmlAttributes<T>(this IEnumerable<IHierarchy<T>> sequence)
        {
            var attributes = new List<IHierarchy<T>>();
            var rest = new List<IHierarchy<T>>();
            foreach (IHierarchy<T> value in sequence)
            {
                XmlAttributeAttribute attribute = value.MemberReference?.GetCustomAttribute<XmlAttributeAttribute>();
                if (attribute != null)
                {
                    attributes.Add(value);
                }
                else
                {
                    rest.Add(value);
                }
            }
            return attributes.Concat(rest);
        }
    }
}