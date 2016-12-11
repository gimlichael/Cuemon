using System;
using System.Xml;

namespace Cuemon.Xml
{
    /// <summary>
    /// Specifies options that is related to <see cref="XmlWriterUtility" /> operations.
    /// </summary>
    public class XmlCopyOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XmlCopyOptions"/> class.
        /// </summary>
        public XmlCopyOptions()
        {
            LeaveStreamOpen = false;
            WriterSettings = null;
        }

        /// <summary>
        /// Gets or sets a value indicating whether [leave stream open].
        /// </summary>
        /// <value><c>true</c> if the <see cref="XmlReader"/> is being left open; otherwise, <c>false</c> and the <see cref="XmlReader"/> is disposed of.</value>
        public bool LeaveStreamOpen { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="XmlWriterSettings"/> which will be applied doing the copying process and need to be configured.
        /// </summary>
        /// <value>The writer settings.</value>
        public Action<XmlWriterSettings> WriterSettings { get; set; }
    }
}