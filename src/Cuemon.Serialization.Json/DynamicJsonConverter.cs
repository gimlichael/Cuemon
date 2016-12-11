using System;
using Newtonsoft.Json;

namespace Cuemon.Serialization.Json
{
    /// <summary>
    /// Provides a factory based way to create and wrap an <see cref="JsonConverter"/> implementation.
    /// </summary>
    public static class DynamicJsonConverter
    {
        /// <summary>
        /// Creates a dynamic instance of an <see cref="JsonConverter"/> implementation wrapping <see cref="JsonConverter.WriteJson"/> through <paramref name="writer"/> and <see cref="JsonConverter.ReadJson"/> through <paramref name="reader"/>.
        /// </summary>
        /// <typeparam name="T">The type of the <paramref name="source"/> to implement an <see cref="JsonConverter"/>.</typeparam>
        /// <param name="source">The object that needs support for an <see cref="JsonConverter"/> implementation.</param>
        /// <param name="writer">The delegate that converts <paramref name="source"/> to its JSON representation.</param>
        /// <param name="reader">The delegate that generates <paramref name="source"/> from its JSON representation.</param>
        /// <returns>An <see cref="JsonConverter"/> implementation of <paramref name="source"/>.</returns>
        public static JsonConverter Create<T>(T source, Action<JsonWriter, T> writer, Func<JsonReader, Type, T> reader = null)
        {
            Validator.ThrowIfNull(source, nameof(source));
            return new DynamicJsonConverter<T>(source, writer, reader);
        }
    }

    internal class DynamicJsonConverter<T> : JsonConverter
    {
        internal DynamicJsonConverter(T source, Action<JsonWriter, T> writer, Func<JsonReader, Type, T> reader)
        {
            Source = source;
            Writer = writer;
            Reader = reader;
        }

        private T Source { get; }

        private Action<JsonWriter, T> Writer { get; }

        private Func<JsonReader, Type, T> Reader { get; }

        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter" /> to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (Writer == null) { throw new NotImplementedException(); }
            Writer.Invoke(writer, Source);
        }

        /// <summary>
        /// Reads the JSON representation of the object.
        /// </summary>
        /// <param name="reader">The <see cref="T:Newtonsoft.Json.JsonReader" /> to read from.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="existingValue">The existing value of object being read.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>The object value.</returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (Reader == null) { throw new NotImplementedException(); }
            return Reader.Invoke(reader, objectType);
        }

        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns><c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.</returns>
        public override bool CanConvert(Type objectType)
        {
            return Source.GetType().HasTypes(objectType);
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="T:Newtonsoft.Json.JsonConverter" /> can read JSON.
        /// </summary>
        /// <value><c>true</c> if this <see cref="T:Newtonsoft.Json.JsonConverter" /> can read JSON; otherwise, <c>false</c>.</value>
        public override bool CanRead => Reader != null;

        /// <summary>
        /// Gets a value indicating whether this <see cref="T:Newtonsoft.Json.JsonConverter" /> can write JSON.
        /// </summary>
        /// <value><c>true</c> if this <see cref="T:Newtonsoft.Json.JsonConverter" /> can write JSON; otherwise, <c>false</c>.</value>
        public override bool CanWrite => Writer != null;
    }
}