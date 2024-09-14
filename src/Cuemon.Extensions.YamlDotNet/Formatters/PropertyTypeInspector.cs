using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.TypeInspectors;

namespace Cuemon.Extensions.YamlDotNet.Formatters
{
    internal sealed class PropertyTypeInspector : TypeInspectorSkeleton
    {
        private readonly ITypeInspector _inspector;

        internal PropertyTypeInspector(ITypeInspector inspector)
        {
            _inspector = inspector;
        }

        public override string GetEnumName(Type enumType, string name)
        {
            return _inspector.GetEnumName(enumType, name);
        }

        public override string GetEnumValue(object enumValue)
        {
            return _inspector.GetEnumValue(enumValue);
        }

        public override IEnumerable<IPropertyDescriptor> GetProperties(Type type, object container)
        {
            var properties = _inspector.GetProperties(type, container);
            return properties.Where(property => !(property.Type.GetTypeInfo().IsMarshalByRef ||
                                                 property.Type.GetTypeInfo().IsSubclassOf(typeof(Delegate)) ||
                                                 property.Name.Equals("SyncRoot", StringComparison.Ordinal) ||
                                                 property.Name.Equals("IsReadOnly", StringComparison.Ordinal) ||
                                                 property.Name.Equals("IsFixedSize", StringComparison.Ordinal) ||
                                                 property.Name.Equals("IsSynchronized", StringComparison.Ordinal) ||
                                                 property.Name.Equals("Count", StringComparison.Ordinal) ||
                                                 property.Name.Equals("HResult", StringComparison.Ordinal) ||
                                                 property.Name.Equals("Parent", StringComparison.Ordinal) ||
                                                 property.Name.Equals("TargetSite", StringComparison.Ordinal)));
        }
    }
}
