using Cuemon.Extensions.AspNetCore.Mvc.Formatters.Text.Json.Converters;
using Cuemon.Extensions.Text.Json.Formatters;

namespace Cuemon.Extensions.AspNetCore.Mvc.Formatters.Text.Json
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
                        JsonFormatterOptions.DefaultConverters += list =>
                        {
                            list.AddStringValuesConverter();
                        };
                    }
                }
            }
        }
    }
}
