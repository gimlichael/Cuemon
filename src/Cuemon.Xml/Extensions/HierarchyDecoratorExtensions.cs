using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;
using Cuemon.Reflection;
using Cuemon.Xml.Serialization;

namespace Cuemon.Xml
{
    /// <summary>
    /// Extension methods for the <see cref="IHierarchy{T}"/> interface tailored to adhere the decorator pattern.
    /// </summary>
    /// <seealso cref="IDecorator{T}"/>
    /// <seealso cref="Decorator{T}"/>
    public static class HierarchyDecoratorExtensions
    {
        /// <summary>
        /// Determines whether the underlying <see cref="T:IHierarchy{object}"/> of the <paramref name="decorator"/> implements <see cref="XmlIgnoreAttribute"/>.
        /// </summary>
        /// <param name="decorator">The <see cref="T:IDecorator{IHierarchy{object}}"/> to extend.</param>
        /// <returns>
        /// 	<c>true</c> if the underlying <see cref="T:IHierarchy{object}"/> of the <paramref name="decorator"/> implements <see cref="XmlIgnoreAttribute"/>; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="decorator"/> cannot be null.
        /// </exception>
        public static bool HasXmlIgnoreAttribute(this IDecorator<IHierarchy<object>> decorator)
        {
            Validator.ThrowIfNull(decorator, nameof(decorator));
            return decorator.Inner.HasMemberReference && Decorator.Enclose(decorator.Inner.MemberReference).HasAttribute(typeof(XmlIgnoreAttribute));
        }

        /// <summary>
        /// Determines whether the underlying <see cref="T:IHierarchy{object}"/> of the <paramref name="decorator"/> implements either <see cref="IEnumerable"/> or <see cref="IEnumerable{T}"/> and is not a <see cref="string"/>.
        /// </summary>
        /// <param name="decorator">The <see cref="T:IDecorator{IHierarchy{object}}"/> to extend.</param>
        /// <returns>
        /// 	<c>true</c> if the underlying <see cref="T:IHierarchy{object}"/> of the <paramref name="decorator"/> implements either <see cref="IEnumerable"/> or <see cref="IEnumerable{T}"/> and is not a <see cref="string"/>; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="decorator"/> cannot be null.
        /// </exception>
        public static bool IsNodeEnumerable(this IDecorator<IHierarchy<object>> decorator)
        {
            Validator.ThrowIfNull(decorator, nameof(decorator));
            return Decorator.Enclose(decorator.Inner.InstanceType).HasEnumerableImplementation() && (decorator.Inner.InstanceType != typeof(string));
        }

        /// <summary>
        /// Resolves an <see cref="XmlQualifiedEntity"/> from the specified <paramref name="qualifiedRootEntity"/> or from the underlying <see cref="T:IHierarchy{object}"/> of the <paramref name="decorator"/>.
        /// </summary>
        /// <param name="decorator">The <see cref="T:IDecorator{IHierarchy{object}}"/> to extend.</param>
        /// <param name="qualifiedRootEntity">The optional <see cref="XmlQualifiedEntity"/> that is part of the equation.</param>
        /// <returns>An <see cref="XmlQualifiedEntity"/> that is either from <paramref name="qualifiedRootEntity"/>, <see cref="XmlRootAttribute"/> or <see cref="XmlElementAttribute"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="decorator"/> cannot be null.
        /// </exception>
        public static XmlQualifiedEntity GetXmlRootOrElement(this IDecorator<IHierarchy<object>> decorator, XmlQualifiedEntity qualifiedRootEntity = null)
        {
            Validator.ThrowIfNull(decorator, nameof(decorator));
            if (qualifiedRootEntity != null && !string.IsNullOrWhiteSpace(qualifiedRootEntity.LocalName)) { return qualifiedRootEntity; }
            var hasRootAttribute = Decorator.Enclose(decorator.Inner.InstanceType).HasAttribute(typeof(XmlRootAttribute));
            var hasElementAttribute = decorator.Inner.HasMemberReference && Decorator.Enclose(decorator.Inner.MemberReference).HasAttribute(typeof(XmlElementAttribute));
            var rootOrElementName = decorator.Inner.HasMemberReference 
                ? Decorator.Enclose(decorator.Inner.MemberReference.Name).SanitizeXmlElementName() 
                : Decorator.Enclose(Decorator.Enclose(decorator.Inner.InstanceType).ToFriendlyName(o => o.ExcludeGenericArguments = true)).SanitizeXmlElementName();
            string ns = null;

            if (hasRootAttribute || hasElementAttribute)
            {
                string elementName = null;
                if (hasRootAttribute)
                {
                    var rootAttribute = decorator.Inner.InstanceType.GetTypeInfo().GetCustomAttribute<XmlRootAttribute>(true);
                    elementName = rootAttribute.ElementName;
                    ns = rootAttribute.Namespace;
                }

                if (hasElementAttribute)
                {
                    var elementAttribute = decorator.Inner.MemberReference.GetCustomAttribute<XmlElementAttribute>();
                    elementName = elementAttribute.ElementName;
                    ns = elementAttribute.Namespace;
                }

                if (!string.IsNullOrEmpty(elementName))
                {
                    rootOrElementName = elementName;
                }
            }

            var instance = decorator.Inner.Instance as XmlQualifiedEntity;
            return instance ?? new XmlQualifiedEntity(Decorator.Enclose(rootOrElementName).SanitizeXmlElementName(), ns);
        }

        /// <summary>  
        /// Orders a sequence of <see cref="IHierarchy{T}"/> from the underlying <see cref="T:IHierarchy{object}"/> of the <paramref name="decorator"/> by nodes having an <see cref="XmlAttributeAttribute"/> decoration.
        /// </summary>
        /// <typeparam name="T">The type of the node represented in the hierarchical structure.</typeparam>
        /// <param name="decorator">The <see cref="T:IDecorator{IHierarchy{object}}"/> to extend.</param>
        /// <returns>A sequence of <see cref="IHierarchy{T}"/> that is sorted by nodes having an <see cref="XmlAttributeAttribute"/> decoration first.</returns>
        public static IEnumerable<IHierarchy<T>> OrderByXmlAttributes<T>(this IDecorator<IEnumerable<IHierarchy<T>>> decorator)
        {
            var attributes = new List<IHierarchy<T>>();
            var rest = new List<IHierarchy<T>>();
            foreach (var value in decorator.Inner)
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