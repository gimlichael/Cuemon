using System;
using System.Reflection;

namespace Cuemon
{
    internal static class Infrastructure
    {
        internal static MethodInfo ResolveDelegateInfo(Delegate wrapper, Delegate original)
        {
            if (original != null) { return original.GetMethodInfo(); }
            if (wrapper != null) { return wrapper.GetMethodInfo(); }
            return null;
        }

        internal static object DefaultPropertyValueResolver(object source, PropertyInfo pi)
        {
            return source == null ? null : pi.GetValue(source, null);
        }
    }
}