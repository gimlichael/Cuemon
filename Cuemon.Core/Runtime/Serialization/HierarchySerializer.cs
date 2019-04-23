using System;
using Cuemon.Reflection;

namespace Cuemon.Runtime.Serialization
{
    /// <summary>
    /// Provides a way to serialize objects to nodes of <see cref="IHierarchy{T}"/>.
    /// </summary>
    public class HierarchySerializer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HierarchySerializer"/> class.
        /// </summary>
        /// <param name="source">The object to convert to nodes of <see cref="IHierarchy{T}"/>.</param>
        /// <param name="setup">The <see cref="ObjectHierarchyOptions"/> which need to be configured.</param>
        public HierarchySerializer(object source, Action<ObjectHierarchyOptions> setup = null)
        {
            Nodes = ReflectionUtility.GetObjectHierarchy(source, setup).Root();
        }

        /// <summary>
        /// Gets the result of the <see cref="IHierarchy{T}"/>.
        /// </summary>
        /// <value>The converted nodes of the the by constructor defined source object.</value>
        public IHierarchy<object> Nodes { get; }
    }
}