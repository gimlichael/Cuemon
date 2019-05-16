using System;

namespace Cuemon.ComponentModel.TypeConverters
{
    /// <summary>
    /// Provides a parser that converts a <see cref="string"/> to its equivalent <see cref="Guid"/>.
    /// </summary>
    public class GuidParser : IParser<Guid, GuidOptions>
    {
        /// <summary>
        /// Converts the string representation of a GUID to its <see cref="Guid"/> equivalent.
        /// </summary>
        /// <param name="input">The string to convert.</param>
        /// <returns>A <see cref="Guid"/> equivalent to <paramref name="input"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="input"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="input"/> cannot be empty or consist only of white-space characters.
        /// </exception>
        /// <exception cref="FormatException">
        /// <paramref name="input"/> was not recognized to be a GUID.
        /// </exception>
        /// <seealso cref="Guid.Parse"/>
        /// <seealso cref="Guid.ParseExact"/>
        public Guid Parse(string input, Action<GuidOptions> setup = null)
        {
            Validator.ThrowIfNullOrWhitespace(input, nameof(input));
            var options = Patterns.Configure(setup);
            if (EnumUtility.HasFlag(options.Formats, GuidFormats.Any)) { return Guid.Parse(input); }
            var hyphens = (input.IndexOf('-') > 0);
            var braces = (input.StartsWith("{", StringComparison.OrdinalIgnoreCase) && input.EndsWith("}", StringComparison.OrdinalIgnoreCase));
            var parentheses = (input.StartsWith("(", StringComparison.OrdinalIgnoreCase) && input.EndsWith(")", StringComparison.OrdinalIgnoreCase));
            var xformat = braces && input.Split(',').Length == 11;
            if (xformat && EnumUtility.HasFlag(options.Formats, GuidFormats.X)) { return Guid.ParseExact(input, "X"); }
            if (parentheses && hyphens && EnumUtility.HasFlag(options.Formats, GuidFormats.P)) { return Guid.ParseExact(input, "P"); }
            if (braces && hyphens && EnumUtility.HasFlag(options.Formats, GuidFormats.B)) { return Guid.ParseExact(input, "B"); }
            if (hyphens && EnumUtility.HasFlag(options.Formats, GuidFormats.D)) { return Guid.ParseExact(input, "D"); }
            if (!hyphens && EnumUtility.HasFlag(options.Formats, GuidFormats.N)) { return Guid.ParseExact(input, "N"); }
            throw new FormatException($"The {nameof(input)} is not in a recognized format.");
        }

        /// <summary>
        /// Converts the string representation of a GUID to its <see cref="Guid"/> equivalent. A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="input">The string to convert.</param>
        /// <param name="result">When this method returns, contains the <see cref="Guid"/> equivalent of the <paramref name="input"/>, if the conversion succeeded, or <c>default</c> if the conversion failed.</param>
        /// <returns><c>true</c> if <paramref name="input"/> was converted successfully; otherwise, <c>false</c>.</returns>
        public bool TryParse(string input, out Guid result, Action<GuidOptions> setup = null)
        {
            return Patterns.TryParse(() => Parse(input, setup), out result);
        }
    }
}