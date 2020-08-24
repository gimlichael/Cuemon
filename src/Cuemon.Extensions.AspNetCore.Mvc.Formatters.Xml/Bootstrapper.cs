using Cuemon.Extensions.AspNetCore.Mvc.Formatters.Xml.Converters;
using Cuemon.Xml.Serialization;

namespace Cuemon.Extensions.AspNetCore.Mvc.Formatters.Xml
{
    internal static class Bootstrapper
    {
        private static readonly object PadLock = new object();
        private static bool _initialized = false;

        internal static void Initialize()
        {
            if (!_initialized)
            {
                lock (PadLock)
                {
                    if (!_initialized)
                    {
                        _initialized = true;
                        XmlSerializerOptions.DefaultConverters += list =>
                        {
                            list.AddHttpExceptionDescriptorConverter()
                                .AddStringValuesConverter()
                                .AddHeaderDictionaryConverter()
                                .AddFormCollectionConverter()
                                .AddQueryCollectionConverter()
                                .AddCookieCollectionConverter();
                        };
                    }
                }
                
            }
        }
    }
}