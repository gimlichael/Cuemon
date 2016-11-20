using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Cuemon.AspNetCore.Mvc
{
    /// <summary>
    /// A <see cref="ConfigureOptions{TOptions}"/> implementation which will add the XML serializer formatters to <see cref="MvcOptions"/>.
    /// </summary>
    public class XmlSerializationMvcOptionsSetup : ConfigureOptions<MvcOptions>
    {
        /// <summary>
        /// Creates a new <see cref="XmlSerializationMvcOptionsSetup"/>.
        /// </summary>
        public XmlSerializationMvcOptionsSetup() : base(ConfigureMvc)
        {
        }

        /// <summary>
        /// Adds the XML serializer formatters to <see cref="MvcOptions"/>.
        /// </summary>
        /// <param name="options">The <see cref="MvcOptions"/>.</param>
        public static void ConfigureMvc(MvcOptions options)
        {
            options.OutputFormatters.Add(new XmlSerializationOutputFormatter());
            //options.InputFormatters.Add(new XmlSerializerInputFormatter());
        }
    }
}