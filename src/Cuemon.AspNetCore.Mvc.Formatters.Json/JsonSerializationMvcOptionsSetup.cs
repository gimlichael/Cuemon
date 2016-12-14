using Cuemon.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Cuemon.AspNetCore.Mvc
{
    /// <summary>
    /// A <see cref="ConfigureOptions{TOptions}"/> implementation which will add the XML serializer formatters to <see cref="MvcOptions"/>.
    /// </summary>
    public class JsonSerializationMvcOptionsSetup : ConfigureOptions<MvcOptions>
    {
        /// <summary>
        /// Creates a new <see cref="JsonSerializationMvcOptionsSetup"/>.
        /// </summary>
        public JsonSerializationMvcOptionsSetup() : base(ConfigureMvc)
        {
        }

        /// <summary>
        /// Adds the XML serializer formatters to <see cref="MvcOptions"/>.
        /// </summary>
        /// <param name="options">The <see cref="MvcOptions"/>.</param>
        public static void ConfigureMvc(MvcOptions options)
        {
            options.OutputFormatters.Insert(0, new JsonSerializationOutputFormatter());
            options.InputFormatters.Insert(0, new JsonSerializationInputFormatter());
        }
    }
}