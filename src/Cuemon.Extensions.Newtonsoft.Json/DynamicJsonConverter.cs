using System;
using System.Linq;
using Cuemon.Extensions.Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

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
        /// <param name="reader">The function delegate that generates <typeparamref name="T"/> from its JSON representation.</param>
        /// <returns>An <see cref="JsonConverter"/> implementation of <typeparamref name="T"/>.</returns>
        public static JsonConverter Create<T>(Action<JsonWriter, T, JsonSerializer> writer = null, Func<JsonReader, Type, T, JsonSerializer, T> reader = null)
        {
            var objectType = typeof(T);
            return Create(objectType.IsAssignableFrom, writer, reader);
        }

        /// <summary>
        /// Creates a dynamic instance of an <see cref="JsonConverter" /> implementation wrapping <see cref="JsonConverter.WriteJson" /> through <paramref name="writer" /> and <see cref="JsonConverter.ReadJson" /> through <paramref name="reader" />.
        /// </summary>
        /// <typeparam name="T">The type to implement an <see cref="JsonConverter"/>.</typeparam>
        /// <param name="predicate">The function delegate that validates if given <see cref="Type"/> can be converted to and from JSON.</param>
        /// <param name="writer">The delegate that, when <paramref name="predicate"/> returns true, converts <typeparamref name="T"/> to its JSON representation.</param>
        /// <param name="reader">The function delegate that, when <paramref name="predicate"/> returns true, generates <typeparamref name="T"/> from its JSON representation.</param>
        /// <returns>An <see cref="JsonConverter" /> implementation of <typeparamref name="T"/>.</returns>
        public static JsonConverter Create<T>(Func<Type, bool> predicate, Action<JsonWriter, T, JsonSerializer> writer = null, Func<JsonReader, Type, T, JsonSerializer, T> reader = null)
        {
            var castedWriter = writer == null ? (Action<JsonWriter, object, JsonSerializer>) null : (w, t, s) => writer(w, (T)t, s);
            var castedReader = reader == null ? (Func<JsonReader, Type, object, JsonSerializer, object>) null : (r, t, o, s) => reader(r, t, (T)o, s);
            return new DynamicJsonConverterCore(predicate, castedWriter, castedReader, typeof(T));
        }

        /// <summary>
        /// Creates a dynamic instance of an <see cref="JsonConverter" /> implementation wrapping <see cref="JsonConverter.WriteJson" /> through <paramref name="writer" /> and <see cref="JsonConverter.ReadJson" /> through <paramref name="reader" />.
        /// </summary>
        /// <param name="objectType">The type of the object to convert.</param>
        /// <param name="writer">The delegate that converts <paramref name="objectType"/> to its JSON representation.</param>
        /// <param name="reader">The function delegate that generates <paramref name="objectType"/> from its JSON representation.</param>
        /// <returns>An <see cref="JsonConverter" /> implementation of <paramref name="objectType"/>.</returns>
        public static JsonConverter Create(Type objectType, Action<JsonWriter, object, JsonSerializer> writer = null, Func<JsonReader, Type, object, JsonSerializer, object> reader = null)
        {
            return new DynamicJsonConverterCore(objectType.IsAssignableFrom, writer, reader, objectType);
        }

        /// <summary>
        /// Creates a dynamic instance of an <see cref="JsonConverter" /> implementation wrapping <see cref="JsonConverter.CanConvert"/> through <paramref name="predicate"/>, <see cref="JsonConverter.WriteJson" /> through <paramref name="writer" /> and <see cref="JsonConverter.ReadJson" /> through <paramref name="reader" />.
        /// </summary>
        /// <param name="predicate">The function delegate that validates if given <see cref="Type"/> can be converted to and from JSON.</param>
        /// <param name="writer">The delegate that converts a given <see cref="Type"/> to its JSON representation.</param>
        /// <param name="reader">The function delegate that generates <see cref="Type"/> from its JSON representation.</param>
        /// <returns>An <see cref="JsonConverter" /> implementation of <see cref="Type"/>.</returns>
        public static JsonConverter Create(Func<Type, bool> predicate, Action<JsonWriter, object, JsonSerializer> writer = null, Func<JsonReader, Type, object, JsonSerializer, object> reader = null)
        {
            return new DynamicJsonConverterCore(predicate, writer, reader);
        }

        /// <summary>
        /// Creates a dynamic instance of <paramref name="converter"/>.
        /// </summary>
        /// <param name="converter">The <see cref="JsonConverter"/> to wrap.</param>
        /// <returns>An <see cref="JsonConverter" /> implementation of <see cref="Type"/>.</returns>
        public static JsonConverter Create(JsonConverter converter)
        {
            return new DynamicJsonConverterCore(converter.CanConvert, converter.WriteJson, converter.ReadJson, searchForNamingStrategy: true);
        }
    }

    internal class DynamicJsonConverterCore : JsonConverter
    {
        internal DynamicJsonConverterCore(Func<Type, bool> predicate, Action<JsonWriter, object, JsonSerializer> writer, Func<JsonReader, Type, object, JsonSerializer, object> reader, Type objectType = null, bool searchForNamingStrategy = false)
        {
            Predicate = predicate;
            Writer = writer;
            Reader = reader;
            SearchForNamingStrategy = searchForNamingStrategy;
            ObjectType = objectType;
        }

        private object ObjectType { get; }

        private bool SearchForNamingStrategy { get; }

        private Func<Type, bool> Predicate { get; }

        private Action<JsonWriter, object, JsonSerializer> Writer { get; }

        private Func<JsonReader, Type, object, JsonSerializer, object> Reader { get; }

        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter" /> to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (Writer == null) { throw new NotImplementedException("Delegate writer is null."); }
            if (SearchForNamingStrategy) // special case; we wrap an existing converter to allow possible naming strategy being applied transparently for the developer
            {
                var namingStrategyProperty = Writer.Target?.GetType().GetProperties().FirstOrDefault(pi => pi.PropertyType == typeof(NamingStrategy));
                namingStrategyProperty?.SetValue(Writer.Target, serializer.ContractResolver.ResolveNamingStrategyOrDefault());
            }
            Writer.Invoke(writer, value, serializer);
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
            return Reader.Invoke(reader, objectType, existingValue, serializer);
        }

        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns><c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.</returns>
        public override bool CanConvert(Type objectType)
        {
            return Predicate(objectType);
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