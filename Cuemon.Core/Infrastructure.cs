using System;
using System.IO;
using System.Reflection;

namespace Cuemon
{
    internal static class Infrastructure
    {
        internal static void CopyStream(Stream source, Stream destination, int bufferSize = 81920, bool changePosition = true)
        {
            long lastPosition = 0;
            if (changePosition && source.CanSeek)
            {
                lastPosition = source.Position;
                if (source.CanSeek) { source.Position = 0; }
            }

            source.CopyTo(destination, bufferSize);
            destination.Flush();

            if (changePosition && source.CanSeek) { source.Position = lastPosition; }
            if (changePosition && destination.CanSeek) { destination.Position = 0; }
        }

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