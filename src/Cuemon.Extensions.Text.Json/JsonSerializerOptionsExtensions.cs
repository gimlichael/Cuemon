using System.Text.Json;

namespace Cuemon.Extensions.Text.Json
{
    /// <summary>
    /// Extension methods for the <see cref="JsonSerializerOptions"/> class.
    /// </summary>
    public static class JsonSerializerOptionsExtensions
    {
        /// <summary>
        /// Returns the specified <paramref name="name"/> adhering to the underlying <see cref="JsonSerializerOptions.PropertyNamingPolicy"/>.
        /// </summary>
        /// <param name="options">The options from which to apply a property naming policy.</param>
        /// <param name="name">The name.</param>
        /// <returns>When <see cref="JsonSerializerOptions.PropertyNamingPolicy"/> is null, the specified <paramref name="name"/> is returned unaltered; otherwise it is converted according to the <see cref="JsonNamingPolicy"/></returns>
        /// <remarks>A convenient way of defining the property name according to Microsoft design decisions.</remarks>
        public static string SetPropertyName(this JsonSerializerOptions options, string name)
        {
            return options.PropertyNamingPolicy.DefaultOrConvertName(name);
        }
    }
}
