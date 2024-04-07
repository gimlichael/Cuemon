using Cuemon.Extensions.YamlDotNet.Formatters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Cuemon.Extensions.AspNetCore.Mvc.Formatters.Text.Yaml
{
    /// <summary>
    /// A <see cref="ConfigureOptions{TOptions}"/> implementation which will add the YAML serializer formatters to <see cref="MvcOptions"/>.
    /// </summary>
    public class YamlSerializationMvcOptionsSetup : ConfigureOptions<MvcOptions>
    {
        /// <summary>
        /// Creates a new <see cref="YamlSerializationMvcOptionsSetup"/>.
        /// </summary>
        public YamlSerializationMvcOptionsSetup(IOptions<YamlFormatterOptions> formatterOptions) : base(mo =>
        {
            mo.OutputFormatters.Insert(0, new YamlSerializationOutputFormatter(formatterOptions?.Value));
            mo.InputFormatters.Insert(0, new YamlSerializationInputFormatter(formatterOptions?.Value));
        })
        {
        }
    }
}
