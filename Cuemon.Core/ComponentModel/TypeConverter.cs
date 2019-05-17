using System;

namespace Cuemon.ComponentModel
{
    internal static class TypeConverter<TInput>
    {
        public static T UseConverter<TConverter, T>(TInput input) where TConverter : class, ITypeConverter<TInput>, new()
        {
            return Activator.CreateInstance<TConverter>().ChangeType<T>(input);
        }

        public static object UseConverter<TConverter>(TInput input, Type targetType) where TConverter : class, ITypeConverter<TInput>, new()
        {
            return Activator.CreateInstance<TConverter>().ChangeType(input, targetType);
        }
    }

    internal static class TypeConverter<TInput, TOptions> where TOptions : class, new()
    {
        public static T UseConverter<TConverter, T>(TInput input, Action<TOptions> setup) where TConverter : class, ITypeConverter<TInput, TOptions>, new()
        {
            return Activator.CreateInstance<TConverter>().ChangeType<T>(input, setup);
        }

        public static object UseConverter<TConverter>(TInput input, Type targetType, Action<TOptions> setup) where TConverter : class, ITypeConverter<TInput, TOptions>, new()
        {
            return Activator.CreateInstance<TConverter>().ChangeType(input, targetType, setup);
        }
    }
}