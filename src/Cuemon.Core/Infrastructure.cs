using System.Reflection;

namespace Cuemon
{
    internal static class Infrastructure
    {
        internal static object DefaultPropertyValueResolver(object source, PropertyInfo pi)
        {
            return source == null ? null : pi.GetValue(source, null);
        }
    }
}
