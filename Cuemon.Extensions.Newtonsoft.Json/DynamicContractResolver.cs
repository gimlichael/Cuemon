using System;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Cuemon.Extensions.Newtonsoft.Json
{
    /// <summary>
    /// Provides a factory based way to create and wrap an <see cref="IContractResolver"/> implementation.
    /// </summary>
    public static class DynamicContractResolver
    {
        /// <summary>
        /// Creates the specified resolver.
        /// </summary>
        /// <typeparam name="T">The type that inherits from <see cref="IContractResolver"/>.</typeparam>
        /// <param name="resolver">The instance of an <see cref="IContractResolver"/> implementation.</param>
        /// <param name="jsonPropertyHandlers">The array of delegates that will handle custom rules of a <see cref="JsonProperty"/>.</param>
        /// <returns>An <see cref="IContractResolver"/> implementation of <typeparamref name="T"/>.</returns>
        public static IContractResolver Create<T>(T resolver, params Action<JsonProperty>[] jsonPropertyHandlers) where T : IContractResolver
        {
            switch (typeof(T).Name)
            {
                case nameof(CamelCasePropertyNamesContractResolver):
                    return new DynamicCamelCasePropertyNamesContractResolver(jsonPropertyHandlers);
                default:
                    return new DynamicDefaultContractResolver(jsonPropertyHandlers);
            }
        }
    }

    internal class DynamicDefaultContractResolver : DefaultContractResolver
    {
        internal DynamicDefaultContractResolver(IEnumerable<Action<JsonProperty>> jsonPropertyHandlers)
        {
            JsonPropertyHandlers = jsonPropertyHandlers;
        }

        private IEnumerable<Action<JsonProperty>> JsonPropertyHandlers { get; set; }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);
            foreach (var handler in JsonPropertyHandlers)
            {
                handler(property);
            }
            return property;
        }
    }

    internal class DynamicCamelCasePropertyNamesContractResolver : CamelCasePropertyNamesContractResolver
    {
        internal DynamicCamelCasePropertyNamesContractResolver(IEnumerable<Action<JsonProperty>> jsonPropertyHandlers)
        {
            JsonPropertyHandlers = jsonPropertyHandlers;
        }

        private IEnumerable<Action<JsonProperty>> JsonPropertyHandlers { get; set; }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);
            foreach (var handler in JsonPropertyHandlers)
            {
                handler(property);
            }
            return property;
        }
    }
}