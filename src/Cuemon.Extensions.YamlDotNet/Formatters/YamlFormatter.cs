using System;
using System.IO;
using System.Reflection;
using Cuemon.Extensions.YamlDotNet.Converters;
using Cuemon.IO;
using Cuemon.Runtime.Serialization.Formatters;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace Cuemon.Extensions.YamlDotNet.Formatters
{
    /// <summary>
    /// Serializes and deserializes an object, in YAML format.
    /// </summary>
    /// <seealso cref="Formatter" />.
    /// <seealso cref="YamlConverter"/>.
    public class YamlFormatter : StreamFormatter<YamlFormatterOptions>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="YamlFormatter"/> class.
        /// </summary>
        public YamlFormatter() : this((Action<YamlFormatterOptions>)null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="YamlFormatter"/> class.
        /// </summary>
        /// <param name="setup">The <see cref="YamlFormatterOptions"/> which need to be configured.</param>
        public YamlFormatter(Action<YamlFormatterOptions> setup) : this(Patterns.Configure(setup))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="YamlFormatter"/> class.
        /// </summary>
        /// <param name="options">The configured <see cref="YamlFormatterOptions"/>.</param>
        public YamlFormatter(YamlFormatterOptions options) : base(options)
        {
            Options.RefreshWithConverterDependencies();
        }

        /// <summary>
        /// Serializes the specified <paramref name="source"/> to an object of <see cref="Stream"/>.
        /// </summary>
        /// <param name="source">The object to serialize to YAML format.</param>
        /// <param name="objectType">The type of the object to serialize.</param>
        /// <returns>A stream of the serialized <paramref name="source"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> is null -or-
        /// <paramref name="objectType"/> is null.
        /// </exception>
        public override Stream Serialize(object source, Type objectType)
        {
            Validator.ThrowIfNull(source);
            Validator.ThrowIfNull(objectType);

            return StreamFactory.Create(writer =>
            {
                var settings = new EmitterSettings(Options.Settings.WhiteSpaceIndentation, 
                    Options.Settings.TextWidth, 
                    Options.Settings.IsCanonical, 
                    1024, 
                    indentSequences: Options.Settings.IndentSequences, 
                    newLine: Options.Settings.NewLine);
                var emitter = new Emitter(writer, settings);
                UseBuilder()
                    .Build()
                    .Serialize(emitter, source, objectType);
            }, o =>
            {
                o.Encoding = Options.Settings.Encoding;
                o.Preamble = Options.Settings.Preamble;
                o.NewLine = Options.Settings.NewLine;
                o.FormatProvider = Options.Settings.FormatProvider;
            });
        }

        private SerializerBuilder UseBuilder()
        {
            var builder = new SerializerBuilder()
                .ConfigureDefaultValuesHandling(Options.Settings.ValuesHandling)
                .WithDefaultScalarStyle(Options.Settings.ScalarStyle)
                .WithNamingConvention(Options.Settings.NamingConvention)
                .WithMaximumRecursion(Options.Settings.MaximumRecursion)
                .WithNewLine(Options.Settings.NewLine)
                .WithEnumNamingConvention(Options.Settings.EnumNamingConvention)
                .WithYamlFormatter(Options.Settings.Formatter);
            if (!Options.Settings.UseAliases) { builder.DisableAliases(); }
            if (Options.Settings.EnsureRoundtrip) { builder.EnsureRoundtrip(); }
            if (Options.Settings.ReflectionRules.Flags.HasFlag(BindingFlags.NonPublic))
            {
                builder.IncludeNonPublicProperties();
                builder.EnablePrivateConstructors();
            }

            foreach (var converter in Options.Settings.Converters)
            {
                if (converter is YamlConverter yamlConverter) { yamlConverter.FormatterOptions = Options; }
                builder.WithTypeConverter(converter);
            }

            return builder;
        }

        internal void Serialize(IEmitter emitter, object source, Type objectType)
        {
            UseBuilder()
                .BuildValueSerializer()
                .SerializeValue(emitter, source, objectType);
        }

        /// <summary>
        /// Deserializes the specified <paramref name="value" /> into an object of <paramref name="objectType"/>.
        /// </summary>
        /// <param name="value">The object from which to deserialize the object graph.</param>
        /// <param name="objectType">The type of the deserialized object.</param>
        /// <returns>An object of <paramref name="objectType" />.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> is null -or-
        /// <paramref name="objectType"/> is null.
        /// </exception>
        public override object Deserialize(Stream value, Type objectType)
        {
            Validator.ThrowIfNull(value);
            Validator.ThrowIfNull(objectType);
            var builder = new DeserializerBuilder()
                .WithNamingConvention(Options.Settings.NamingConvention)
                .WithEnumNamingConvention(Options.Settings.EnumNamingConvention)
                .WithYamlFormatter(Options.Settings.Formatter)
                .WithCaseInsensitivePropertyMatching();
            if (Options.Settings.ReflectionRules.Flags.HasFlag(BindingFlags.NonPublic))
            {
                builder.IncludeNonPublicProperties();
                builder.EnablePrivateConstructors();
            }
            
            foreach (var converter in Options.Settings.Converters)
            {
                if (converter is YamlConverter yamlConverter) { yamlConverter.FormatterOptions = Options; }
                builder.WithTypeConverter(converter);
            }

            var serializer = builder.Build();
            var parser = new Parser(new StreamReader(value, Options.Encoding));
            return serializer.Deserialize(parser, objectType);
        }
    }
}
