using System;
using Newtonsoft.Json;

namespace Cuemon.Extensions.AspNetCore.Mvc.Formatters.Json
{
    /// <summary>
    /// Extension methods for the <see cref="JsonSerializerSettings"/> class.
    /// </summary>
    public static class JsonSerializerSettingsExtensions
    {
        /// <summary>
        /// Instructs a JSON serializer to propagate the <see cref="JsonSerializerSettings"/> specified by <typeparamref name="T"/> on to <paramref name="s1"/> with an optional <paramref name="setup"/> delegate.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="JsonSerializerSettings"/> to use.</typeparam>
        /// <param name="s1">The <see cref="JsonSerializerSettings"/> to extend.</param>
        /// <param name="setup">The <see cref="JsonSerializerSettings"/> which need to be configured.</param>
        public static void Use<T>(this JsonSerializerSettings s1, Action<T> setup = null) where T : JsonSerializerSettings, new()
        {
            Validator.ThrowIfNull(s1, nameof(s1));
            var s2 = Patterns.Configure(setup);
            s1.CheckAdditionalContent = s2.CheckAdditionalContent;
            s1.ConstructorHandling = s2.ConstructorHandling;
            s1.Context = s2.Context;
            s1.ContractResolver = s2.ContractResolver;
            s1.Converters = s2.Converters;
            s1.Culture = s2.Culture;
            s1.DateFormatHandling = s2.DateFormatHandling;
            s1.DateTimeZoneHandling = s2.DateTimeZoneHandling;
            s1.DateFormatString = s2.DateFormatString;
            s1.DateParseHandling = s2.DateParseHandling;
            s1.Error = s2.Error;
            s1.DefaultValueHandling = s2.DefaultValueHandling;
            s1.EqualityComparer = s2.EqualityComparer;
            s1.FloatFormatHandling = s2.FloatFormatHandling;
            s1.FloatParseHandling = s2.FloatParseHandling;
            s1.Formatting = s2.Formatting;
            s1.MaxDepth = s2.MaxDepth;
            s1.MetadataPropertyHandling = s2.MetadataPropertyHandling;
            s1.MissingMemberHandling = s2.MissingMemberHandling;
            s1.NullValueHandling = s2.NullValueHandling;
            s1.ObjectCreationHandling = s2.ObjectCreationHandling;
            s1.PreserveReferencesHandling = s2.PreserveReferencesHandling;
            s1.ReferenceLoopHandling = s2.ReferenceLoopHandling;
            s1.ReferenceResolverProvider = s2.ReferenceResolverProvider;
            s1.SerializationBinder = s2.SerializationBinder;
            s1.StringEscapeHandling = s2.StringEscapeHandling;
            s1.TraceWriter = s2.TraceWriter;
            s1.TypeNameAssemblyFormatHandling = s2.TypeNameAssemblyFormatHandling;
            s1.TypeNameHandling = s2.TypeNameHandling;
        }
    }
}