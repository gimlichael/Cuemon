using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace Cuemon.IO
{
    /// <summary>
    /// Specifies a set of features to support on the <see cref="StreamWriter"/> object created by the <see cref="StreamWriterUtility.CreateStream(Cuemon.Action{System.IO.StreamWriter})"/> method. This class cannot be inherited.
    /// </summary>
    public sealed class StreamWriterSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StreamWriterSettings"/> class.
        /// </summary>
        public StreamWriterSettings()
        {
            AutoFlush = false;
            BufferSize = 1024;
            Encoding = Encoding.UTF8;
            FormatProvider = CultureInfo.InvariantCulture;
            NewLine = "\r\n";
        }

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="StreamWriter"/> will flush its buffer to the underlying stream after every call to the <c>Write</c> method.
        /// </summary>
        /// <value><c>true</c> to force <see cref="StreamWriter"/> to flush its buffer; otherwise, <c>false</c>.</value>
        public bool AutoFlush { get; set; }

        /// <summary>
        /// Gets or sets the size of the buffer.
        /// </summary>
        /// <value>The size of the buffer in bytes for the <see cref="StreamWriter"/>.</value>
        public int BufferSize { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Encoding"/> in which the output is written by the <see cref="StreamWriter"/>.
        /// </summary>
        /// <value>The <see cref="Encoding"/> for the <see cref="StreamWriter"/>.</value>
        public Encoding Encoding { get; set; }

        /// <summary>
        /// Gets or sets the culture-specific formatting information used when writing data with the <see cref="StreamWriter"/>.
        /// </summary>
        /// <value>An <see cref="IFormatProvider"/> that contains the culture-specific formatting information. The default is <see cref="CultureInfo.InvariantCulture"/>.</value>
        public IFormatProvider FormatProvider { get; set; }

        /// <summary>
        /// Gets or sets the line terminator string used by the <see cref="StreamWriter"/>.
        /// </summary>
        /// <value>The line terminator string for the <see cref="StreamWriter"/>.</value>
        public string NewLine { get; set; }
    }
}