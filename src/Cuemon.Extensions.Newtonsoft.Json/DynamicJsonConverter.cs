using System;
using Newtonsoft.Json;

namespace Cuemon.Extensions.Newtonsoft.Json
{
    /// <summary>
    /// Provides a factory based way to create and wrap an <see cref="JsonConverter"/> implementation.
    /// </summary>
    public static class DynamicJsonConverter
    {
        /// <summary>
        /// Creates a dynamic instance of an <see cref="JsonConverter"/> implementation wrapping <see cref="JsonConverter.WriteJson"/> through <paramref name="writer"/> and <see cref="JsonConverter.ReadJson"/> through <paramref name="reader"/>.
        /// </summary>
        /// <typeparam name="T">The type to implement an <see cref="JsonConverter"/>.</typeparam>
        /// <param name="writer">The delegate that converts <typeparamref name="T"/> to its JSON representation.</param>
        /// <param name="reader">The delegate that generates <typeparamref name="T"/> from its JSON representation.</param>
        /// <returns>An <see cref="JsonConverter"/> implementation of <typeparamref name="T"/>.</returns>
        public static JsonConverter Create<T>(Action<JsonWriter, T> writer = null, Func<JsonReader, Type, T> reader = null)
        {
            var castedWriter = writer == null ? (Action<JsonWriter, object>) null : (w, t) => writer(w, (T)t);
            var castedReader = reader == null ? (Func<JsonReader, Type, object>) null : (r, t) => reader(r, t);
            return Create(typeof(T), castedWriter, castedReader);
        }

        /// <summary>
        /// Creates a dynamic instance of an <see cref="JsonConverter" /> implementation wrapping <see cref="JsonConverter.WriteJson" /> through <paramref name="writer" /> and <see cref="JsonConverter.ReadJson" /> through <paramref name="reader" />.
        /// </summary>
        /// <param name="objectType">The type of the object to convert.</param>
        /// <param name="writer">The delegate that converts <paramref name="objectType"/> to its JSON representation.</param>
        /// <param name="reader">The delegate that generates <paramref name="objectType"/> from its JSON representation.</param>
        /// <returns>An <see cref="JsonConverter" /> implementation of <paramref name="objectType"/>.</returns>
        public static JsonConverter Create(Type objectType, Action<JsonWriter, object> writer = null, Func<JsonReader, Type, object> reader = null)
        {
            return new DynamicJsonConverterCore(objectType, writer, reader);
        }
    }

    internal class DynamicJsonConverterCore : JsonConverter
    {
        internal DynamicJsonConverterCore(Type objectType, Action<JsonWriter, object> writer, Func<JsonReader, Type, object> reader)
        {
            ObjectType = objectType;
            Writer = writer;
            Reader = reader;
        }

        private Type ObjectType { get; set; }

        private Action<JsonWriter, object> Writer { get; }

        private Func<JsonReader, Type, object> Reader { get; }

        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter" /> to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (Writer == null) { throw new NotImplementedException("Delegate writer is null."); }
            Writer.Invoke(writer, value);
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
            if (Reader == null) { throw new NotImplementedException("Delegate reader is null."); }
            return Reader.Invoke(reader, objectType);
        }

        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns><c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.</returns>
        public override bool CanConvert(Type objectType)
        {
            return ObjectType.IsAssignableFrom(objectType);
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