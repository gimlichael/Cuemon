namespace Cuemon
{
    /// <summary>
    /// Defines some standardized patterns to use when formatting date- and time values.
    /// </summary>
    public enum StandardizedDateTimeFormatPattern
    {
        /// <summary>
        /// Displays a date using the ISO8601 basic date format, eg.: YYYYMMDD.
        /// </summary>
        Iso8601CompleteDateBasic,
        /// <summary>
        /// Displays a date using the ISO8601 extended date format (human readable), eg.: YYYY-MM-DD.
        /// </summary>
        Iso8601CompleteDateExtended,
        /// <summary>
        /// Displays a date using the ISO8601 basic date format in conjunction with the ISO8601 time format, eg.: YYYYMMDDThhmmssTZD.
        /// </summary>
        Iso8601CompleteDateTimeBasic,
        /// <summary>
        /// Displays a date using the ISO8601 extended date format (human readable) in conjunction with the ISO8601 extended time format (human readable), eg.: YYYY-MM-DDThh:mm:ssTZD.
        /// </summary>
        Iso8601CompleteDateTimeExtended
    }
}