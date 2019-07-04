using System;
using System.Collections.Generic;

namespace Cuemon.Integrity
{
    public class ConvertibleOptions : EndianOptions
    {
        public ConvertibleOptions()
        {
            Converters = new ConverterCollection();
        }

        public ConverterCollection Converters { get; internal set; }

        public class ConverterCollection
        {
            private readonly Dictionary<Type, Func<IConvertible, byte[]>> _converters = new Dictionary<Type, Func<IConvertible, byte[]>>();

            public ConverterCollection Add<T>(Func<T, byte[]> converter) where T : IConvertible
            {
                Add(typeof(T), c => converter((T)c));
                return this;
            }

            public ConverterCollection Add(Type type, Func<IConvertible, byte[]> converter)
            {
                _converters.Add(type, converter);
                return this;
            }

            public Func<IConvertible, byte[]> this[Type type]
            {
                get
                {
                    if (type == null) { return null; }
                    return _converters.TryGetValue(type, out var converter) ? converter : null;
                }
            }
        }
    }
}