using System;
using Cuemon.Extensions.YamlDotNet.Converters;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace Cuemon.Extensions.YamlDotNet
{
    /// <summary>
    /// Provides a factory based way to create and wrap an <see cref="YamlConverter"/> implementation.
    /// </summary>
    public static class YamlConverterFactory
    {
        /// <summary>
        /// Creates a dynamic instance of an <see cref="YamlConverter{T}" /> implementation wrapping <see cref="YamlConverter{T}.WriteYaml" /> through <paramref name="writer" /> and <see cref="YamlConverter{T}.ReadYaml" /> through <paramref name="reader" />.
        /// </summary>
        /// <typeparam name="T">The type to implement an <see cref="YamlConverter" /> for.</typeparam>
        /// <param name="writer">The delegate that converts <typeparamref name="T" /> to its YAML representation.</param>
        /// <param name="reader">The delegate that generates <typeparamref name="T" /> from its YAML representation.</param>
        /// <returns>An <see cref="YamlConverter" /> implementation of <typeparamref name="T" />.</returns>
        public static YamlConverter Create<T>(Action<IEmitter, T> writer = null, Func<IParser, Type, T> reader = null)
        {
            return new DynamicConvertFactory<T>(typeof(T).IsAssignableFrom, writer, reader);
        }

        /// <summary>
        /// Creates a dynamic instance of an <see cref="YamlConverter{T}" /> implementation wrapping <see cref="YamlConverter{T}.WriteYaml" /> through <paramref name="writer" /> and <see cref="YamlConverter{T}.ReadYaml" /> through <paramref name="reader" />.
        /// </summary>
        /// <typeparam name="T">The type to implement an <see cref="YamlConverter" /> for.</typeparam>
        /// <param name="predicate">The function delegate that validates if the given <see cref="Type"/> can be converted to or from YAML.</param>
        /// <param name="writer">The delegate that converts <typeparamref name="T" /> to its YAML representation.</param>
        /// <param name="reader">The delegate that generates <typeparamref name="T" /> from its YAML representation.</param>
        /// <returns>An <see cref="YamlConverter" /> implementation of <typeparamref name="T" />.</returns>
        public static YamlConverter Create<T>(Func<Type, bool> predicate, Action<IEmitter, T> writer = null, Func<IParser, Type, T> reader = null)
        {
            return new DynamicConvertFactory<T>(predicate, writer, reader);
        }

        /// <summary>
        /// Creates a dynamic instance of an <see cref="YamlConverter" /> implementation wrapping <see cref="YamlConverter.WriteYamlCore" /> through <paramref name="writer" /> and <see cref="YamlConverter.ReadYamlCore" /> through <paramref name="reader" />.
        /// </summary>
        /// <param name="typeToConvert">The type to convert.</param>
        /// <param name="writer">The delegate that converts an object to its YAML representation.</param>
        /// <param name="reader">The delegate that generates an object from its YAML representation.</param>
        /// <returns>An <see cref="YamlConverter" /> implementation of an object.</returns>
        public static YamlConverter Create(Type typeToConvert, Action<IEmitter, object> writer = null, Func<IParser, Type, object> reader = null)
        {
            return new DynamicConvertFactory(typeToConvert.IsAssignableFrom, writer, reader);
        }

        /// <summary>
        /// Creates a dynamic instance of an <see cref="YamlConverter" /> implementation wrapping <see cref="YamlConverter.WriteYamlCore" /> through <paramref name="writer" /> and <see cref="YamlConverter.ReadYamlCore" /> through <paramref name="reader" />.
        /// </summary>
        /// <param name="predicate">The function delegate that validates if the given <see cref="Type"/> can be converted to or from YAML.</param>
        /// <param name="writer">The delegate that converts an object to its YAML representation.</param>
        /// <param name="reader">The delegate that generates an object from its YAML representation.</param>
        /// <returns>An <see cref="YamlConverter" /> implementation of an object.</returns>
        public static YamlConverter Create(Func<Type, bool> predicate, Action<IEmitter, object> writer = null, Func<IParser, Type, object> reader = null)
        {
            return new DynamicConvertFactory(predicate, writer, reader);
        }
    }

    internal class DynamicConvertFactory : YamlConverter
    {
        internal DynamicConvertFactory(Func<Type, bool> predicate, Action<IEmitter, object> writer, Func<IParser, Type, object> reader)
        {
            Predicate = predicate;
            Writer = writer;
            Reader = reader;
        }
        
        private Func<Type, bool> Predicate { get; }

        private Action<IEmitter, object> Writer { get; }

        private Func<IParser, Type, object> Reader { get; }

        internal override void WriteYamlCore(IEmitter writer, object value, ObjectSerializer serializer)
        {
            if (Writer == null) { throw new NotImplementedException("Delegate writer is null."); }
            Writer.Invoke(writer, value);
        }

        internal override object ReadYamlCore(IParser reader, Type typeToConvert, ObjectDeserializer deserializer)
        {
            if (Reader == null) { throw new NotImplementedException("Function delegate reader is null."); }
            return Reader.Invoke(reader, typeToConvert);
        }

        public override bool CanConvert(Type typeToConvert)
        {
            return Predicate.Invoke(typeToConvert);
        }
    }

    internal class DynamicConvertFactory<T> : YamlConverter<T>
    {
        internal DynamicConvertFactory(Func<Type, bool> predicate, Action<IEmitter, T> writer, Func<IParser, Type, T> reader)
        {
            Predicate = predicate;
            Writer = writer;
            Reader = reader;
        }

        private Func<Type, bool> Predicate { get; }

        private Action<IEmitter, T> Writer { get; }

        private Func<IParser, Type, T> Reader { get; }

        public override void WriteYaml(IEmitter writer, T value)
        {
            if (Writer == null) { throw new NotImplementedException("Delegate writer is null."); }
            Writer.Invoke(writer, value);
        }

        public override T ReadYaml(IParser reader, Type typeToConvert)
        {
            if (Reader == null) { throw new NotImplementedException("Function delegate reader is null."); }
            return Reader.Invoke(reader, typeToConvert);
        }

        public override bool CanConvert(Type typeToConvert)
        {
            return Predicate.Invoke(typeToConvert);
        }
    }
}
