using System;
using System.Threading;

namespace Cuemon.Runtime
{
    /// <summary>
    /// An abstract class for establishing a watcher, that can monitor and signal changes of a resource by raising the <see cref="Changed"/> event.
    /// </summary>
    public abstract class Watcher : IDisposable
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Watcher"/> class, where the signaling is initiated immediately and hereby followed by a periodic signaling every 2 minutes.
        /// </summary>
        protected Watcher() : this(TimeSpan.FromMinutes(2))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Watcher"/> class, where the signaling is initiated immediately.
        /// </summary>
        /// <param name="period">The time interval between periodic signaling. Specify negative one (-1) milliseconds to disable periodic signaling.</param>
        protected Watcher(TimeSpan period) : this(TimeSpan.Zero, period)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Watcher"/> class.
        /// </summary>
        /// <param name="dueTime">A <see cref="TimeSpan"/> representing the amount of time to delay before the <see cref="Watcher"/> starts signaling. Specify negative one (-1) milliseconds to prevent the signaling from starting. Specify zero (0) to start the signaling immediately.</param>
        /// <param name="period">The time interval between periodic signaling. Specify negative one (-1) milliseconds to disable periodic signaling.</param>
        protected Watcher(TimeSpan dueTime, TimeSpan period) : this(dueTime, period, TimeSpan.Zero)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Watcher"/> class.
        /// </summary>
        /// <param name="dueTime">A <see cref="TimeSpan"/> representing the amount of time to delay before the <see cref="Watcher"/> starts signaling. Specify negative one (-1) milliseconds to prevent the signaling from starting. Specify zero (0) to start the signaling immediately.</param>
        /// <param name="period">The time interval between periodic signaling. Specify negative one (-1) milliseconds to disable periodic signaling.</param>
        /// <param name="dueTimeOnChanged">The amount of time to postpone a <see cref="Watcher.Changed"/> event. Specify zero (0) to disable postponing.</param>
        protected Watcher(TimeSpan dueTime, TimeSpan period, TimeSpan dueTimeOnChanged)
        {
            DueTime = dueTime;
            Period = period;
            DueTimeOnChanged = dueTimeOnChanged;
            UtcLastModified = DateTime.UtcNow;
            Timer = new Timer(TimerInvoking, null, dueTime, period);
        }
        #endregion

        #region Events
        /// <summary>
        /// Occurs when a resource has changed.
        /// </summary>
        public event EventHandler<WatcherEventArgs> Changed;
        #endregion

        #region Properties
        /// <summary>
        /// Gets time when the resource being monitored was last changed.
        /// </summary>
        /// <value>The time when the resource being monitored was last changed.</value>
        /// <remarks>This property is measured in Coordinated Universal Time (UTC) (also known as Greenwich Mean Time).</remarks>
        public DateTime UtcLastModified { get; private set; }

        /// <summary>
        /// Gets the time when the last signaling occurred.
        /// </summary>
        /// <value>The time when the last signaling occurred.</value>
        /// <remarks>This property is measured in Coordinated Universal Time (UTC) (also known as Greenwich Mean Time).</remarks>
        public DateTime UtcLastSignaled { get; private set; }

        /// <summary>
        /// Gets or sets the <see cref="TimeSpan"/> representing the amount of time to delay before the <see cref="Watcher"/> starts signaling.
        /// </summary>
        /// <value>A <see cref="TimeSpan"/> representing the amount of time to delay before the <see cref="Watcher"/> starts signaling.</value>
        protected TimeSpan DueTime { get; private set; }

        /// <summary>
        /// Gets the time interval between periodic signaling.
        /// </summary>
        /// <value>A <see cref="TimeSpan"/> representing the time interval between periodic signaling.</value>
        protected TimeSpan Period { get; private set; }

        /// <summary>
        /// Gets the amount of time to postpone a <see cref="Watcher.Changed"/> event.
        /// </summary>
        protected TimeSpan DueTimeOnChanged { get; private set; }

        /// <summary>
        /// Changes the signaling timer of the <see cref="Watcher"/>.
        /// </summary>
        /// <param name="dueTime">A <see cref="TimeSpan"/> representing the amount of time to delay before the <see cref="Watcher"/> starts signaling. Specify negative one (-1) milliseconds to prevent the signaling from starting. Specify zero (0) to start the signaling immediately.</param>
        /// If <paramref name="dueTime"/> is zero (0), the signaling is started immediately. If <paramref name="dueTime"/> is negative one (-1) milliseconds, the signaling is never started; and the underlying timer is disabled, but can be re-enabled by specifying a positive value for <paramref name="dueTime"/>.
        public void ChangeSignaling(TimeSpan dueTime)
        {
            ChangeSignaling(dueTime, Period);
        }

        /// <summary>
        /// Changes the signaling timer of the <see cref="Watcher"/>.
        /// </summary>
        /// <param name="dueTime">A <see cref="TimeSpan"/> representing the amount of time to delay before the <see cref="Watcher"/> starts signaling. Specify negative one (-1) milliseconds to prevent the signaling from starting. Specify zero (0) to start the signaling immediately.</param>
        /// <param name="period">The time interval between periodic signaling. Specify negative one (-1) milliseconds to disable periodic signaling.</param>
        /// <remarks>If <paramref name="dueTime" /> is zero (0), the signaling is started immediately. If <paramref name="dueTime" /> is negative one (-1) milliseconds, the signaling is never started; and the underlying timer is disabled, but can be re-enabled by specifying a positive value for <paramref name="dueTime" />.
        /// If <paramref name="period" /> is zero (0) or negative one (-1) milliseconds, and <paramref name="dueTime" /> is positive, the signaling is done once; the periodic behavior of the underlying timer is disabled, but can be re-enabled by specifying a value greater than zero for <paramref name="period" />.</remarks>
        public virtual void ChangeSignaling(TimeSpan dueTime, TimeSpan period)
        {
            DueTime = dueTime;
            Period = period;
            Timer.Change(dueTime, period);
        }

        private Timer Timer { get; set; }

        private Timer TimerPostponing { get; set; }

        private bool IsDisposed { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Marks the time when a resource being monitored was last changed.
        /// </summary>
        /// <param name="utcLastModified">The time when a resource being monitored was last changed.</param>
        protected void SetUtcLastModified(DateTime utcLastModified)
        {
            if (utcLastModified.Kind != DateTimeKind.Utc) { throw new ArgumentException("The time from when the resource being monitored was last changed, must be specified in the Coordinated Universal Time (UTC).", nameof(utcLastModified)); }
            UtcLastModified = utcLastModified;
        }

        private void TimerInvoking(object o)
        {
            UtcLastSignaled = DateTime.UtcNow;
            HandleSignaling();
        }

        /// <summary>
        /// Handles the signaling of this <see cref="Watcher"/>.
        /// </summary>
        protected abstract void HandleSignaling();

        private void PostponedHandleSignaling(object parameter)
        {
            OnChangedRaisedCore(parameter as WatcherEventArgs);
        }

        /// <summary>
        /// Raises the <see cref="Changed"/> event.
        /// </summary>
        /// <remarks>This method raises the <see cref="Changed"/> event with <see cref="UtcLastModified"/> and <see cref="DueTimeOnChanged"/> passed to a new instance of <see cref="WatcherEventArgs"/>.</remarks>
        protected void OnChangedRaised()
        {
            OnChangedRaised(new WatcherEventArgs(UtcLastModified, DueTimeOnChanged));
        }

        /// <summary>
        /// Raises the <see cref="Changed"/> event.
        /// </summary>
        /// <param name="e">The <see cref="WatcherEventArgs"/> instance containing the event data.</param>
        protected virtual void OnChangedRaised(WatcherEventArgs e)
        {
            if (TimerPostponing != null) { return; } // we already have a postponed signaling
            if (DueTimeOnChanged != TimeSpan.Zero)
            {
                TimerPostponing = new Timer(PostponedHandleSignaling, e, DueTimeOnChanged, TimeSpan.FromMilliseconds(-1));
                return;
            }
            OnChangedRaisedCore(e);
        }

        private void OnChangedRaisedCore(WatcherEventArgs e)
        {
            EventHandler<WatcherEventArgs> handler = Changed;
            EventUtility.Raise(handler, this, e);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (IsDisposed) { return; }
            IsDisposed = true;
            if (disposing)
            {
                if (Timer != null) { Timer.Dispose(); }
                if (TimerPostponing != null) { TimerPostponing.Dispose(); }
            }
            Timer = null;
            TimerPostponing = null;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}