using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Cuemon.AspNetCore.Mvc.Formatters.Xml
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
            options.OutputFormatters.Insert(0, new XmlSerializationOutputFormatter());
            options.InputFormatters.Insert(0, new XmlSerializationInputFormatter());
        }
    }
}