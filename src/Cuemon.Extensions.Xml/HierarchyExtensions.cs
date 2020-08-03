using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;
using Cuemon.Extensions.Xml.Serialization;
using Cuemon.Reflection;

namespace Cuemon.Extensions.Xml
{
    /// <summary>
    /// Extension methods for the <see cref="IHierarchy{T}"/> interface.
    /// </summary>
    public static class HierarchyExtensions
    {
        /// <summary>
        /// Determines whether the <paramref name="hierarchy"/> implements <see cref="XmlIgnoreAttribute"/>.
        /// </summary>
        /// <param name="hierarchy">The <see cref="T:IHierarchy{object}"/> to extend.</param>
        /// <returns>
        /// 	<c>true</c> if the <see cref="T:IHierarchy{object}"/> implements <see cref="XmlIgnoreAttribute"/>; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="hierarchy"/> cannot be null.
        /// </exception>
        public static bool HasXmlIgnoreAttribute(this IHierarchy<object> hierarchy)
        {
            Validator.ThrowIfNull(hierarchy, nameof(hierarchy));
            return hierarchy.HasMemberReference && Decorator.Enclose(hierarchy.MemberReference).HasAttribute(typeof(XmlIgnoreAttribute));
        }

        /// <summary>
        /// Determines whether the <paramref name="hierarchy"/> implements either <see cref="IEnumerable"/> or <see cref="IEnumerable{T}"/> and is not a <see cref="string"/>.
        /// </summary>
        /// <param name="hierarchy">The <see cref="T:IHierarchy{object}"/> to extend.</param>
        /// <returns>
        /// 	<c>true</c> if the <see cref="T:IHierarchy{object}"/> implements either <see cref="IEnumerable"/> or <see cref="IEnumerable{T}"/> and is not a <see cref="string"/>; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="hierarchy"/> cannot be null.
        /// </exception>
        public static bool IsNodeEnumerable(this IHierarchy<object> hierarchy)
        {
            Validator.ThrowIfNull(hierarchy, nameof(hierarchy));
            return Decorator.Enclose(hierarchy.InstanceType).HasEnumerableImplementation() && (hierarchy.InstanceType != typeof(string));
        }

        /// <summary>
        /// Resolves an <see cref="XmlQualifiedEntity"/> from the specified <paramref name="qualifiedRootEntity"/> or from the <see cref="T:IHierarchy{object}"/>.
        /// </summary>
        /// <param name="hierarchy">The <see cref="T:IHierarchy{object}"/> to extend.</param>
        /// <param name="qualifiedRootEntity">The optional <see cref="XmlQualifiedEntity"/> that is part of the equation.</param>
        /// <returns>An <see cref="XmlQualifiedEntity"/> that is either from <paramref name="qualifiedRootEntity"/>, <see cref="XmlRootAttribute"/> or <see cref="XmlElementAttribute"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="hierarchy"/> cannot be null.
        /// </exception>
        public static XmlQualifiedEntity GetXmlRootOrElement(this IHierarchy<object> hierarchy, XmlQualifiedEntity qualifiedRootEntity = null)
        {
            Validator.ThrowIfNull(hierarchy, nameof(hierarchy));
            if (qualifiedRootEntity != null && !string.IsNullOrWhiteSpace(qualifiedRootEntity.LocalName)) { return qualifiedRootEntity; }
            var hasRootAttribute = Decorator.Enclose(hierarchy.InstanceType).HasAttribute(typeof(XmlRootAttribute));
            var hasElementAttribute = hierarchy.HasMemberReference && Decorator.Enclose(hierarchy.MemberReference).HasAttribute(typeof(XmlElementAttribute));
            var rootOrElementName = hierarchy.HasMemberReference 
                ? hierarchy.MemberReference.Name.SanitizeXmlElementName() 
                : Decorator.Enclose(hierarchy.InstanceType).ToFriendlyName(o => o.ExcludeGenericArguments = true).SanitizeXmlElementName();
            string ns = null;

            if (hasRootAttribute || hasElementAttribute)
            {
                string elementName = null;
                if (hasRootAttribute)
                {
                    var rootAttribute = hierarchy.InstanceType.GetTypeInfo().GetCustomAttribute<XmlRootAttribute>(true);
                    elementName = rootAttribute.ElementName;
                    ns = rootAttribute.Namespace;
                }

                if (hasElementAttribute)
                {
                    var elementAttribute = hierarchy.MemberReference.GetCustomAttribute<XmlElementAttribute>();
                    elementName = elementAttribute.ElementName;
                    ns = elementAttribute.Namespace;
                }

                if (!string.IsNullOrEmpty(elementName))
                {
                    rootOrElementName = elementName;
                }
            }

            var instance = hierarchy.Instance as XmlQualifiedEntity;
            return instance ?? new XmlQualifiedEntity(rootOrElementName.SanitizeXmlElementName(), ns);
        }

        /// <summary>  
        /// Orders a sequence of <see cref="IHierarchy{T}"/> from <paramref name="hierarchies"/> by nodes having an <see cref="XmlAttributeAttribute"/> decoration.
        /// </summary>
        /// <typeparam name="T">The type of the node represented in the hierarchical structure.</typeparam>
        /// <param name="hierarchies">The <see cref="T:IEnumerable{IHierarchy{object}}"/> to extend.</param>
        /// <returns>A sequence of <see cref="IHierarchy{T}"/> that is sorted by nodes having an <see cref="XmlAttributeAttribute"/> decoration first.</returns>
        public static IEnumerable<IHierarchy<T>> OrderByXmlAttributes<T>(this IEnumerable<IHierarchy<T>> hierarchies)
        {
            Validator.ThrowIfNull(hierarchies, nameof(hierarchies));
            var attributes = new List<IHierarchy<T>>();
            var rest = new List<IHierarchy<T>>();
            foreach (var value in hierarchies)
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