using System;

namespace Cuemon.Runtime
{
    /// <summary>
    /// Provides data for watcher related operations.
    /// </summary>
    public class WatcherEventArgs : EventArgs
    {
        private readonly DateTime _utcLastModified;
        private readonly TimeSpan _delayed;

        /// <summary>
        /// Initializes a new instance of the <see cref="WatcherEventArgs"/> class.
        /// </summary>
        protected WatcherEventArgs() : this(DateTime.MinValue)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WatcherEventArgs"/> class.
        /// </summary>
        /// <param name="utcLastModified">The time when a <see cref="Watcher"/> last detected changes to a resource.</param>
        public WatcherEventArgs(DateTime utcLastModified) : this(utcLastModified, TimeSpan.Zero)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WatcherEventArgs"/> class.
        /// </summary>
        /// <param name="utcLastModified">The time when a <see cref="Watcher"/> last detected changes to a resource.</param>
        /// <param name="delayed">The time a <see cref="Watcher"/> was intentionally delayed before signaling changes to a resource.</param>
        public WatcherEventArgs(DateTime utcLastModified, TimeSpan delayed)
        {
            _utcLastModified = utcLastModified;
            _delayed = delayed;
        }

        /// <summary>
        /// Gets the time when a watcher last detected changes to a resource, or a <see cref="DateTime.MinValue"/> if an empty event.
        /// </summary>
        /// <value>The time when a watcher last detected changes to a resource, or a <see cref="DateTime.MinValue"/> if an empty event.</value>
        /// <remarks>This property is measured in Coordinated Universal Time (UTC) (also known as Greenwich Mean Time).</remarks>
        public DateTime UtcLastModified
        {
            get { return _utcLastModified; }
        }

        /// <summary>
        /// Gets the time a <see cref="Watcher"/> was intentionally delayed before signaling changes to a resource.
        /// </summary>
        public TimeSpan Delayed
        {
            get { return _delayed; }
        }

        /// <summary>
        /// Represents an event with no event data.
        /// </summary>
        public new readonly static WatcherEventArgs Empty = new WatcherEventArgs();
    }
}