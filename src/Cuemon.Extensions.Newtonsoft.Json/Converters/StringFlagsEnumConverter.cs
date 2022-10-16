﻿using System;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Cuemon.Extensions.Newtonsoft.Json.Converters
{
    /// <summary>
    /// Converts a FlagsEnum to its name string value.
    /// </summary>
    /// <seealso cref="StringEnumConverter" />
    public class StringFlagsEnumConverter : StringEnumConverter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StringFlagsEnumConverter"/> class.
        /// </summary>
        public StringFlagsEnumConverter()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringFlagsEnumConverter" /> class.
        /// </summary>
        /// <param name="namingStrategy">The naming strategy used to resolve how enum text is written.</param>
        public StringFlagsEnumConverter(NamingStrategy namingStrategy) : base(namingStrategy)
        {
        }

        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter" /> to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }

            var ns = NamingStrategy ?? new CamelCaseNamingStrategy();
            var e = (Enum)value;
            var enumName = e.ToString("G");
            var flags = enumName.Split(',');
            var enumType = e.GetType();
            if (enumType.GetTypeInfo().IsDefined(typeof(FlagsAttribute), false))
            {
                writer.WriteStartArray();
                foreach (var flag in flags)
                {
                    writer.WriteValue(ns.GetPropertyName(flag.Trim(), false));
                }

                writer.WriteEndArray();
            }
            else
            {
                if (char.IsNumber(enumName[0]) || enumName[0] == '-')
                {
                    writer.WriteValue(value);
                }
                else
                {
                    writer.WriteValue(ns.GetPropertyName(enumName, false));
                }
            }
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
            var result = 0;
            while (reader.Read())
            {
                switch (reader.TokenType)
                {
                    case JsonToken.String:
                        result |= (int)Enum.Parse(objectType, reader.Value.ToString(), true);
                        break;
                }
            }

            return result;
        }

        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns><c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.</returns>
        public override bool CanConvert(Type objectType)
        {
            return objectType.IsDefined(typeof(FlagsAttribute), false) && base.CanConvert(objectType);
        }
    }
}