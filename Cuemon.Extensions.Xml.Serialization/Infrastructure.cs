using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;
using Cuemon.ComponentModel;
using Cuemon.Extensions.Reflection;
using Cuemon.Reflection;
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
            return TypeInsight.FromType(node.InstanceType).HasEnumerableContract() && (node.InstanceType != typeof(string));
        }

        internal static XmlQualifiedEntity LookupXmlStartElement(this IHierarchy<object> node, XmlQualifiedEntity qualifiedRootEntity = null)
        {
            if (node == null) { throw new ArgumentNullException(nameof(node)); }
            if (qualifiedRootEntity != null && !qualifiedRootEntity.LocalName.IsNullOrWhiteSpace()) { return qualifiedRootEntity; }
            var hasRootAttribute = TypeInsight.FromType(node.InstanceType).HasAttribute(typeof(XmlRootAttribute));
            var hasElementAttribute = node.HasMemberReference && MemberInsight.FromMember(node.MemberReference).HasAttribute(typeof(XmlElementAttribute));
            var rootOrElementName = node.HasMemberReference ? node.MemberReference.Name.SanitizeXmlElementName() : TypeInsight.FromType(node.InstanceType).ToHumanReadableString(o => o.ExcludeGenericArguments = true).SanitizeXmlElementName();
            string ns = null;

            if (hasRootAttribute || hasElementAttribute)
            {
                string elementName = null;
                if (hasRootAttribute)
                {
                    var rootAttribute = node.InstanceType.GetTypeInfo().GetCustomAttribute<XmlRootAttribute>(true);
                    elementName = rootAttribute.ElementName;
                    ns = rootAttribute.Namespace;
                }

                if (hasElementAttribute)
                {
                    var elementAttribute = node.MemberReference.GetCustomAttribute<XmlElementAttribute>();
                    elementName = elementAttribute.ElementName;
                    ns = elementAttribute.Namespace;
                }

                if (!string.IsNullOrEmpty(elementName))
                {
                    rootOrElementName = elementName;
                }
            }

            var instance = node.Instance as XmlQualifiedEntity;
            return instance ?? new XmlQualifiedEntity(rootOrElementName.SanitizeXmlElementName(), ns);
        }

        internal static IEnumerable<IHierarchy<T>> OrderByXmlAttributes<T>(this IEnumerable<IHierarchy<T>> sequence)
        {
            var attributes = new List<IHierarchy<T>>();
            var rest = new List<IHierarchy<T>>();
            foreach (var value in sequence)
            {
                var attribute = value.MemberReference?.GetCustomAttribute<XmlAttributeAttribute>();
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