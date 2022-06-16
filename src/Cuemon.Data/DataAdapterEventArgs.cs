using System;

namespace Cuemon.Data
{
    /// <summary>
    /// Provides data for <see cref="DataAdapter"/> related operations.
    /// </summary>
    public class DataAdapterEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataAdapterEventArgs"/> class.
        /// </summary>
        protected DataAdapterEventArgs() { }

        /// <summary>
        /// Represents an <see cref="DataAdapter"/> event with no event data.
        /// </summary>
        public new static readonly DataAdapterEventArgs Empty = new();
    }
}