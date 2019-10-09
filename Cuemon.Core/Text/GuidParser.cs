using System;

namespace Cuemon.Text
{
    /// <summary>
    /// Provides a parser that converts a <see cref="string"/> to its equivalent <see cref="Guid"/>.
    /// </summary>
    public class GuidParser : IParser<Guid, GuidOptions>
    {
        /// <summary>
        /// Converts the string representation of a GUID to its <see cref="Guid"/> equivalent.
        /// </summary>
        /// <param name="value">The string to convert.</param>
        /// <param name="setup">The <see cref="GuidOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Guid"/> equivalent to <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> cannot be empty or consist only of white-space characters.
        /// </exception>
        /// <exception cref="FormatException">
        /// <paramref name="value"/> was not recognized to be a GUID.
        /// </exception>
        /// <seealso cref="Guid.Parse"/>
        /// <seealso cref="Guid.ParseExact"/>
        public Guid Parse(string value, Action<GuidOptions> setup = null)
        {
            Validator.ThrowIfNullOrWhitespace(value, nameof(value));
            var options = Patterns.Configure(setup);
            if (options.Formats.HasFlag(GuidFormats.Any)) { return Guid.Parse(value); }
            var hyphens = value.IndexOf('-') != -1;
            var braces = (value.StartsWith("{", StringComparison.OrdinalIgnoreCase) && value.EndsWith("}", StringComparison.OrdinalIgnoreCase));
            var parentheses = (value.StartsWith("(", StringComparison.OrdinalIgnoreCase) && value.EndsWith(")", StringComparison.OrdinalIgnoreCase));
            var xformat = braces && value.Split(',').Length == 11;
            if (xformat && options.Formats.HasFlag(GuidFormats.X)) { return Guid.ParseExact(value, "X"); }
            if (parentheses && hyphens && options.Formats.HasFlag(GuidFormats.P)) { return Guid.ParseExact(value, "P"); }
            if (braces && hyphens && options.Formats.HasFlag(GuidFormats.B)) { return Guid.ParseExact(value, "B"); }
            if (hyphens && options.Formats.HasFlag(GuidFormats.D)) { return Guid.ParseExact(value, "D"); }
            if (!hyphens && options.Formats.HasFlag(GuidFormats.N)) { return Guid.ParseExact(value, "N"); }
            throw new FormatException($"The {nameof(value)} is not in a recognized format.");
        }

        /// <summary>
        /// Converts the string representation of a GUID to its <see cref="Guid"/> equivalent. A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="value">The string to convert.</param>
        /// <param name="result">When this method returns, contains the <see cref="Guid"/> equivalent of the <paramref name="value"/>, if the conversion succeeded, or <c>default</c> if the conversion failed.</param>
        /// <param name="setup">The <see cref="GuidOptions"/> which may be configured.</param>
        /// <returns><c>true</c> if <paramref name="value"/> was converted successfully; otherwise, <c>false</c>.</returns>
        public bool TryParse(string value, out Guid result, Action<GuidOptions> setup = null)
        {
            return Patterns.TryInvoke(() => Parse(value, setup), out result);
        }
    }
}