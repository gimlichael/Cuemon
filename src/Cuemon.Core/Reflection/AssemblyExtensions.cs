using System.Reflection;

namespace Cuemon.Reflection
{
    internal static class AssemblyExtensions
    {
        internal static string GetLocation(this Assembly assembly)
        {
            Validator.ThrowIfNull(assembly, nameof(assembly));
            var location = assembly.GetType().GetProperty("Location", ReflectionUtility.BindingInstancePublicAndPrivate);
            return location?.GetValue(assembly) as string;
        }
    }
}