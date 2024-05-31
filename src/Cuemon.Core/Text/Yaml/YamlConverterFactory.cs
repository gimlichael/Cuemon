using System;
using System.CodeDom.Compiler;
using Cuemon.Runtime.Serialization;
using Cuemon.Text.Yaml.Converters;

namespace Cuemon.Text.Yaml
{
    /// <summary>
    /// Provides a factory based way to create and wrap an <see cref="YamlConverter"/> implementation.
    /// </summary>
    [Obsolete("All YAML marshalling has been moved to its own assembly; Cuemon.Extensions.YamlDotNet. This member will be removed with next major version.")]
    public static class YamlConverterFactory
    {
        /// <summary>
        /// Creates a dynamic instance of an <see cref="YamlConverter{T}" /> implementation wrapping <see cref="YamlConverter{T}.WriteYaml" /> through <paramref name="writer" /> and <see cref="YamlConverter{T}.ReadYaml" /> through <paramref name="reader" />.
        /// </summary>
        /// <typeparam name="T">The type to implement an <see cref="YamlConverter" /> for.</typeparam>
        /// <param name="writer">The delegate that converts <typeparamref name="T" /> to its YAML representation.</param>
        /// <param name="reader">The delegate that generates <typeparamref name="T" /> from its YAML representation.</param>
        /// <returns>An <see cref="YamlConverter" /> implementation of <typeparamref name="T" />.</returns>
        public static YamlConverter Create<T>(Action<YamlTextWriter, T, YamlSerializerOptions> writer = null, Func<YamlTextReader, Type, YamlSerializerOptions, T> reader = null)
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
        public static YamlConverter Create<T>(Func<Type, bool> predicate, Action<YamlTextWriter, T, YamlSerializerOptions> writer = null, Func<YamlTextReader, Type, YamlSerializerOptions, T> reader = null)
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
        public static YamlConverter Create(Type typeToConvert, Action<IndentedTextWriter, object, YamlSerializerOptions> writer = null, Func<YamlTextReader, Type, YamlSerializerOptions, object> reader = null)
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
        public static YamlConverter Create(Func<Type, bool> predicate, Action<YamlTextWriter, object, YamlSerializerOptions> writer = null, Func<YamlTextReader, Type, YamlSerializerOptions, object> reader = null)
        {
            return new DynamicConvertFactory(predicate, writer, reader);
        }
    }

    [Obsolete("All YAML marshalling has been moved to its own assembly; Cuemon.Extensions.YamlDotNet. This member will be removed with next major version.")]
    internal class DynamicConvertFactory : YamlConverter
    {
        internal DynamicConvertFactory(Func<Type, bool> predicate, Action<YamlTextWriter, object, YamlSerializerOptions> writer, Func<YamlTextReader, Type, YamlSerializerOptions, object> reader)
        {
            Predicate = predicate;
            Writer = writer;
            Reader = reader;
        }
        
        private Func<Type, bool> Predicate { get; }

        private Action<YamlTextWriter, object, YamlSerializerOptions> Writer { get; }

        private Func<YamlTextReader, Type, YamlSerializerOptions, object> Reader { get; }

        internal override void WriteYamlCore(YamlTextWriter writer, object value, YamlSerializerOptions so)
        {
            if (Writer == null) { throw new NotImplementedException("Delegate writer is null."); }
            Writer.Invoke(writer, value, so);
        }

        internal override object ReadYamlCore(YamlTextReader reader, Type typeToConvert, YamlSerializerOptions so)
        {
            if (Reader == null) { throw new NotImplementedException("Function delegate reader is null."); }
            return Reader.Invoke(reader, typeToConvert, so);
        }

        public override bool CanConvert(Type typeToConvert)
        {
            return Predicate.Invoke(typeToConvert);
        }
    }

    [Obsolete("All YAML marshalling has been moved to its own assembly; Cuemon.Extensions.YamlDotNet. This member will be removed with next major version.")]
    internal class DynamicConvertFactory<T> : YamlConverter<T>
    {
        internal DynamicConvertFactory(Func<Type, bool> predicate, Action<YamlTextWriter, T, YamlSerializerOptions> writer, Func<YamlTextReader, Type, YamlSerializerOptions, T> reader)
        {
            Predicate = predicate;
            Writer = writer;
            Reader = reader;
        }

        private Func<Type, bool> Predicate { get; }

        private Action<YamlTextWriter, T, YamlSerializerOptions> Writer { get; }

        private Func<YamlTextReader, Type, YamlSerializerOptions, T> Reader { get; }

        public override void WriteYaml(YamlTextWriter writer, T value, YamlSerializerOptions so)
        {
            if (Writer == null) { throw new NotImplementedException("Delegate writer is null."); }
            Writer.Invoke(writer, value, so);
        }

        public override T ReadYaml(YamlTextReader reader, Type typeToConvert, YamlSerializerOptions so)
        {
            if (Reader == null) { throw new NotImplementedException("Function delegate reader is null."); }
            return Reader.Invoke(reader, typeToConvert, so);
        }

        public override bool CanConvert(Type typeToConvert)
        {
            return Predicate.Invoke(typeToConvert);
        }
    }
}
