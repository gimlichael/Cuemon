using Cuemon.Extensions.AspNetCore.Xml.Converters;
using Cuemon.Xml.Serialization.Formatters;

namespace Cuemon.Extensions.AspNetCore.Xml
{
    internal static class Bootstrapper
    {
        private static readonly object PadLock = new();
        private static bool _initialized;

        internal static void Initialize()
        {
            if (!_initialized)
            {
                lock (PadLock)
                {
                    if (!_initialized)
                    {
                        _initialized = true;
                        XmlFormatterOptions.DefaultConverters += list =>
                        {
                            list.AddStringValuesConverter()
                                .AddHeaderDictionaryConverter()
                                .AddFormCollectionConverter()
                                .AddQueryCollectionConverter()
                                .AddCookieCollectionConverter()
                                .AddProblemDetailsConverter();
                        };
                    }
                }
            }
        }
    }
}
