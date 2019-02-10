using System;
using System.Collections.Generic;
using Cuemon.Collections.Generic;
using Cuemon.Runtime;

namespace Cuemon.Net
{
	/// <summary>
	/// This <see cref="NetDependency"/> class will monitor any changes occurred to a Uniform Resource Identifier while notifying subscribing objects.
	/// </summary>
	public sealed class NetDependency : Dependency
	{
		private readonly object _locker = new object();

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="NetDependency"/> class.
		/// </summary>
		/// <param name="value">The URI string to monitor for changes.</param>
		/// <remarks>The signaling is default delayed 15 seconds before first invoke. Signaling occurs every 2 minutes.</remarks>
		public NetDependency(string value) : this(new Uri(value))
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NetDependency"/> class.
		/// </summary>
		/// <param name="value">The URI to monitor for changes.</param>
		/// <remarks>The signaling is default delayed 15 seconds before first invoke.</remarks>
		public NetDependency(Uri value) : this(EnumerableUtility.Yield(value), false)
		{
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="NetDependency"/> class.
        /// </summary>
        /// <param name="values">An <see cref="IEnumerable{T}"/> of URI locations that this <see cref="NetDependency"/> will monitor. When any of these resources changes, this <see cref="NetDependency"/> will notify any subscribing objects of the change.</param>
        /// <remarks>The signaling is default delayed 15 seconds before first invoke. Signaling occurs every 2 minutes.</remarks>
        public NetDependency(IEnumerable<Uri> values) : this(values, false)
        {
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="NetDependency"/> class.
		/// </summary>
		/// <param name="values">An <see cref="IEnumerable{T}"/> of URI locations that this <see cref="NetDependency"/> will monitor. When any of these resources changes, this <see cref="NetDependency"/> will notify any subscribing objects of the change.</param>
        /// <param name="checkResponseData">if set to <c>true</c>, a MD5 hash check of the response data is used to determine a change state of the resource; <c>false</c> to check only for the last modification of the resource.</param>
		/// <remarks>The signaling is default delayed 15 seconds before first invoke. Signaling occurs every 2 minutes. The <paramref name="checkResponseData"/> is useful, when the web server you are probing does not contain the Last-Modified header.</remarks>
		public NetDependency(IEnumerable<Uri> values, bool checkResponseData) : this(values, checkResponseData, TimeSpan.FromSeconds(15), TimeSpan.FromMinutes(2))
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NetDependency"/> class.
		/// </summary>
		/// <param name="values">An <see cref="IEnumerable{T}"/> of URI locations that this <see cref="NetDependency"/> will monitor. When any of these resources changes, this <see cref="NetDependency"/> will notify any subscribing objects of the change.</param>
		/// <param name="period">The time interval between periodic signaling to the specified <paramref name="values"/> by the associated <see cref="NetWatcher"/>. Specify negative one (-1) milliseconds to disable periodic signaling.</param>
		/// <param name="dueTime">The amount of time to delay before the associated <see cref="NetWatcher"/> starts signaling. Specify negative one (-1) milliseconds to prevent the signaling from starting. Specify zero (0) to start the signaling immediately.</param>
		public NetDependency(IEnumerable<Uri> values, TimeSpan dueTime, TimeSpan period) : this(values, false, dueTime, period)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NetDependency"/> class.
		/// </summary>
		/// <param name="values">An <see cref="IEnumerable{T}"/> of URI locations that this <see cref="NetDependency"/> will monitor. When any of these resources changes, this <see cref="NetDependency"/> will notify any subscribing objects of the change.</param>
		/// <param name="period">The time interval between periodic signaling to the specified <paramref name="values"/> by the associated <see cref="NetWatcher"/>. Specify negative one (-1) milliseconds to disable periodic signaling.</param>
		/// <remarks>The signaling is default delayed 15 seconds before first invoke.</remarks>
		public NetDependency(IEnumerable<Uri> values, TimeSpan period) : this(values, TimeSpan.FromSeconds(15), period)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NetDependency"/> class.
		/// </summary>
		/// <param name="values">An <see cref="IEnumerable{T}"/> of URI locations that this <see cref="NetDependency"/> will monitor. When any of these resources changes, this <see cref="NetDependency"/> will notify any subscribing objects of the change.</param>
        /// <param name="checkResponseData">if set to <c>true</c>, a MD5 hash check of the response data is used to determine a change state of the resource; <c>false</c> to check only for the last modification of the resource.</param>
		/// <param name="period">The time interval between periodic signaling to the specified <paramref name="values"/> by the associated <see cref="NetWatcher"/>. Specify negative one (-1) milliseconds to disable periodic signaling.</param>
        /// <remarks>The signaling is default delayed 15 seconds before first invoke. The <paramref name="checkResponseData"/> is useful when the web server you are probing does not contain the Last-Modified header.</remarks>
		public NetDependency(IEnumerable<Uri> values, bool checkResponseData, TimeSpan period) : this(values, checkResponseData, TimeSpan.FromSeconds(15), period)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NetDependency"/> class.
		/// </summary>
		/// <param name="values">An <see cref="IEnumerable{T}"/> of URI locations that this <see cref="NetDependency"/> will monitor. When any of these resources changes, this <see cref="NetDependency"/> will notify any subscribing objects of the change.</param>
        /// <param name="checkResponseData">if set to <c>true</c>, a MD5 hash check of the response data is used to determine a change state of the resource; <c>false</c> to check only for the last modification of the resource.</param>
		/// <param name="period">The time interval between periodic signaling to the specified <paramref name="values"/> by the associated <see cref="NetWatcher"/>. Specify negative one (-1) milliseconds to disable periodic signaling.</param>
		/// <param name="dueTime">The amount of time to delay before the associated <see cref="NetWatcher"/> starts signaling. Specify negative one (-1) milliseconds to prevent the signaling from starting. Specify zero (0) to start the signaling immediately.</param>
		/// <remarks>The <paramref name="checkResponseData"/> is useful when the web server you are probing does not contain the Last-Modified header.</remarks>
		public NetDependency(IEnumerable<Uri> values, bool checkResponseData, TimeSpan dueTime, TimeSpan period)
		{
			if (values == null) { throw new ArgumentNullException(nameof(values)); }
		    Uris = values;
		    CheckResponseData = checkResponseData;
		    DueTime = dueTime;
		    Period = period;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NetDependency"/> class.
		/// </summary>
		/// <param name="values">An <see cref="IEnumerable{T}"/> of URI string locations that this <see cref="NetDependency"/> will monitor. When any of these resources changes, this <see cref="NetDependency"/> will notify any subscribing objects of the change.</param>
		/// <remarks>The signaling is default delayed 15 seconds before first invoke. Signaling occurs every 2 minutes.</remarks>
		public NetDependency(IEnumerable<string> values) : this(values, false)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NetDependency"/> class.
		/// </summary>
		/// <param name="values">An <see cref="IEnumerable{T}"/> of URI string locations that this <see cref="NetDependency"/> will monitor. When any of these resources changes, this <see cref="NetDependency"/> will notify any subscribing objects of the change.</param>
        /// <param name="checkResponseData">if set to <c>true</c>, a MD5 hash check of the response data is used to determine a change state of the resource; <c>false</c> to check only for the last modification of the resource.</param>
		/// <remarks>The signaling is default delayed 15 seconds before first invoke. Signaling occurs every 2 minutes. The <paramref name="checkResponseData"/> is useful, when the web server you are probing does not contain the Last-Modified header.</remarks>
		public NetDependency(IEnumerable<string> values, bool checkResponseData) : this(values, checkResponseData, TimeSpan.FromSeconds(15), TimeSpan.FromMinutes(2))
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NetDependency"/> class.
		/// </summary>
		/// <param name="values">An <see cref="IEnumerable{T}"/> of URI locations that this <see cref="NetDependency"/> will monitor. When any of these resources changes, this <see cref="NetDependency"/> will notify any subscribing objects of the change.</param>
		/// <param name="period">The time interval between periodic signaling to the specified <paramref name="values"/> by the associated <see cref="NetWatcher"/>. Specify negative one (-1) milliseconds to disable periodic signaling.</param>
		/// <param name="dueTime">The amount of time to delay before the associated <see cref="NetWatcher"/> starts signaling. Specify negative one (-1) milliseconds to prevent the signaling from starting. Specify zero (0) to start the signaling immediately.</param>
		public NetDependency(IEnumerable<string> values, TimeSpan dueTime, TimeSpan period) : this(values, false, dueTime, period)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NetDependency"/> class.
		/// </summary>
		/// <param name="values">An <see cref="IEnumerable{T}"/> of URI locations that this <see cref="NetDependency"/> will monitor. When any of these resources changes, this <see cref="NetDependency"/> will notify any subscribing objects of the change.</param>
		/// <param name="period">The time interval between periodic signaling to the specified <paramref name="values"/> by the associated <see cref="NetWatcher"/>. Specify negative one (-1) milliseconds to disable periodic signaling.</param>
		public NetDependency(IEnumerable<string> values, TimeSpan period) : this(values, TimeSpan.FromSeconds(15), period)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NetDependency"/> class.
		/// </summary>
		/// <param name="values">An <see cref="IEnumerable{T}"/> of URI locations that this <see cref="NetDependency"/> will monitor. When any of these resources changes, this <see cref="NetDependency"/> will notify any subscribing objects of the change.</param>
        /// <param name="checkResponseData">if set to <c>true</c>, a MD5 hash check of the response data is used to determine a change state of the resource; <c>false</c> to check only for the last modification of the resource.</param>
		/// <param name="period">The time interval between periodic signaling to the specified <paramref name="values"/> by the associated <see cref="NetWatcher"/>. Specify negative one (-1) milliseconds to disable periodic signaling.</param>
		/// <remarks>The <paramref name="checkResponseData"/> is useful when the web server you are probing does not contain the Last-Modified header.</remarks>
		public NetDependency(IEnumerable<string> values, bool checkResponseData, TimeSpan period) : this(values, checkResponseData, TimeSpan.FromSeconds(15), period)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NetDependency"/> class.
		/// </summary>
		/// <param name="values">An <see cref="IEnumerable{T}"/> of URI locations that this <see cref="NetDependency"/> will monitor. When any of these resources changes, this <see cref="NetDependency"/> will notify any subscribing objects of the change.</param>
        /// <param name="checkResponseData">if set to <c>true</c>, a MD5 hash check of the response data is used to determine a change state of the resource; <c>false</c> to check only for the last modification of the resource.</param>
		/// <param name="dueTime">The amount of time to delay before the associated <see cref="NetWatcher"/> starts signaling. Specify negative one (-1) milliseconds to prevent the signaling from starting. Specify zero (0) to start the signaling immediately.</param>
		/// <param name="period">The time interval between periodic signaling to the specified <paramref name="values"/> by the associated <see cref="NetWatcher"/>. Specify negative one (-1) milliseconds to disable periodic signaling.</param>
		/// <remarks>The <paramref name="checkResponseData"/> is useful when the web server you are probing does not contain the Last-Modified header.</remarks>
		public NetDependency(IEnumerable<string> values, bool checkResponseData, TimeSpan dueTime, TimeSpan period) : this(ToUriSequence(values), checkResponseData, dueTime, period)
		{
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="NetDependency"/> class.
        /// </summary>
        /// <param name="values">An array of URI locations that this <see cref="NetDependency"/> will monitor. When any of these resources changes, this <see cref="NetDependency"/> will notify any subscribing objects of the change.</param>
        /// <remarks>The signaling is default delayed 15 seconds before first invoke. Signaling occurs every 2 minutes.</remarks>
        public NetDependency(params string[] values) : this(values as IEnumerable<string>)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NetDependency"/> class.
        /// </summary>
        /// <param name="checkResponseData">if set to <c>true</c>, a MD5 hash check of the response data is used to determine a change state of the resource; <c>false</c> to check only for the last modification of the resource.</param>
        /// <param name="values">An array of URI locations that this <see cref="NetDependency"/> will monitor. When any of these resources changes, this <see cref="NetDependency"/> will notify any subscribing objects of the change.</param>
        /// <remarks>The signaling is default delayed 15 seconds before first invoke. Signaling occurs every 2 minutes. The <paramref name="checkResponseData"/> is useful, when the web server you are probing does not contain the Last-Modified header.</remarks>
        public NetDependency(bool checkResponseData, params string[] values) : this(values, checkResponseData)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NetDependency"/> class.
        /// </summary>
        /// <param name="period">The time interval between periodic signaling to the specified <paramref name="values"/> by the associated <see cref="NetWatcher"/>. Specify negative one (-1) milliseconds to disable periodic signaling.</param>
        /// <param name="values">An array of URI string locations that this <see cref="NetDependency"/> will monitor. When any of these resources changes, this <see cref="NetDependency"/> will notify any subscribing objects of the change.</param>
        public NetDependency(TimeSpan period, params string[] values) : this(values,  period)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NetDependency"/> class.
        /// </summary>
        /// <param name="period">The time interval between periodic signaling to the specified <paramref name="values"/> by the associated <see cref="NetWatcher"/>. Specify negative one (-1) milliseconds to disable periodic signaling.</param>
        /// <param name="dueTime">The amount of time to delay before the associated <see cref="NetWatcher"/> starts signaling. Specify negative one (-1) milliseconds to prevent the signaling from starting. Specify zero (0) to start the signaling immediately.</param>
        /// <param name="values">An array of URI string locations that this <see cref="NetDependency"/> will monitor. When any of these resources changes, this <see cref="NetDependency"/> will notify any subscribing objects of the change.</param>
        public NetDependency(TimeSpan dueTime, TimeSpan period, params string[] values) : this(values, dueTime, period)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NetDependency"/> class.
        /// </summary>
        /// <param name="checkResponseData">if set to <c>true</c>, a MD5 hash check of the response data is used to determine a change state of the resource; <c>false</c> to check only for the last modification of the resource.</param>
        /// <param name="period">The time interval between periodic signaling to the specified <paramref name="values"/> by the associated <see cref="NetWatcher"/>. Specify negative one (-1) milliseconds to disable periodic signaling.</param>
        /// <param name="values">An array of URI string locations that this <see cref="NetDependency"/> will monitor. When any of these resources changes, this <see cref="NetDependency"/> will notify any subscribing objects of the change.</param>
        /// <remarks>The <paramref name="checkResponseData"/> is useful when the web server you are probing does not contain the Last-Modified header.</remarks>
        public NetDependency(bool checkResponseData, TimeSpan period, params string[] values) : this(values, checkResponseData, period)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NetDependency"/> class.
        /// </summary>
        /// <param name="checkResponseData">if set to <c>true</c>, a MD5 hash check of the response data is used to determine a change state of the resource; <c>false</c> to check only for the last modification of the resource.</param>
        /// <param name="period">The time interval between periodic signaling to the specified <paramref name="values"/> by the associated <see cref="NetWatcher"/>. Specify negative one (-1) milliseconds to disable periodic signaling.</param>
        /// <param name="dueTime">The amount of time to delay before the associated <see cref="NetWatcher"/> starts signaling. Specify negative one (-1) milliseconds to prevent the signaling from starting. Specify zero (0) to start the signaling immediately.</param>
        /// <param name="values">An array of URI string locations that this <see cref="NetDependency"/> will monitor. When any of these resources changes, this <see cref="NetDependency"/> will notify any subscribing objects of the change.</param>
        /// <remarks>The <paramref name="checkResponseData"/> is useful when the web server you are probing does not contain the Last-Modified header.</remarks>
        public NetDependency(bool checkResponseData, TimeSpan dueTime, TimeSpan period, params string[] values) : this(values, checkResponseData, dueTime, period)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NetDependency"/> class.
        /// </summary>
        /// <param name="values">An array of URI locations that this <see cref="NetDependency"/> will monitor. When any of these resources changes, this <see cref="NetDependency"/> will notify any subscribing objects of the change.</param>
        /// <remarks>The signaling is default delayed 15 seconds before first invoke. Signaling occurs every 2 minutes.</remarks>
        public NetDependency(params Uri[] values) : this(values as IEnumerable<Uri>)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NetDependency"/> class.
        /// </summary>
        /// <param name="checkResponseData">if set to <c>true</c>, a MD5 hash check of the response data is used to determine a change state of the resource; <c>false</c> to check only for the last modification of the resource.</param>
        /// <param name="values">An array of URI locations that this <see cref="NetDependency"/> will monitor. When any of these resources changes, this <see cref="NetDependency"/> will notify any subscribing objects of the change.</param>
        /// <remarks>The signaling is default delayed 15 seconds before first invoke. Signaling occurs every 2 minutes. The <paramref name="checkResponseData"/> is useful, when the web server you are probing does not contain the Last-Modified header.</remarks>
        public NetDependency(bool checkResponseData, params Uri[] values) : this(values, checkResponseData)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NetDependency"/> class.
        /// </summary>
        /// <param name="period">The time interval between periodic signaling to the specified <paramref name="values"/> by the associated <see cref="NetWatcher"/>. Specify negative one (-1) milliseconds to disable periodic signaling.</param>
        /// <param name="values">An array of URI locations that this <see cref="NetDependency"/> will monitor. When any of these resources changes, this <see cref="NetDependency"/> will notify any subscribing objects of the change.</param>
        public NetDependency(TimeSpan period, params Uri[] values) : this(values,  period)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NetDependency"/> class.
        /// </summary>
        /// <param name="period">The time interval between periodic signaling to the specified <paramref name="values"/> by the associated <see cref="NetWatcher"/>. Specify negative one (-1) milliseconds to disable periodic signaling.</param>
        /// <param name="dueTime">The amount of time to delay before the associated <see cref="NetWatcher"/> starts signaling. Specify negative one (-1) milliseconds to prevent the signaling from starting. Specify zero (0) to start the signaling immediately.</param>
        /// <param name="values">An array of URI locations that this <see cref="NetDependency"/> will monitor. When any of these resources changes, this <see cref="NetDependency"/> will notify any subscribing objects of the change.</param>
        public NetDependency(TimeSpan dueTime, TimeSpan period, params Uri[] values) : this(values, dueTime, period)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NetDependency"/> class.
        /// </summary>
        /// <param name="checkResponseData">if set to <c>true</c>, a MD5 hash check of the response data is used to determine a change state of the resource; <c>false</c> to check only for the last modification of the resource.</param>
        /// <param name="period">The time interval between periodic signaling to the specified <paramref name="values"/> by the associated <see cref="NetWatcher"/>. Specify negative one (-1) milliseconds to disable periodic signaling.</param>
        /// <param name="values">An array of URI locations that this <see cref="NetDependency"/> will monitor. When any of these resources changes, this <see cref="NetDependency"/> will notify any subscribing objects of the change.</param>
        /// <remarks>The <paramref name="checkResponseData"/> is useful when the web server you are probing does not contain the Last-Modified header.</remarks>
        public NetDependency(bool checkResponseData, TimeSpan period, params Uri[] values) : this(values, checkResponseData, period)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NetDependency"/> class.
        /// </summary>
        /// <param name="checkResponseData">if set to <c>true</c>, a MD5 hash check of the response data is used to determine a change state of the resource; <c>false</c> to check only for the last modification of the resource.</param>
        /// <param name="period">The time interval between periodic signaling to the specified <paramref name="values"/> by the associated <see cref="NetWatcher"/>. Specify negative one (-1) milliseconds to disable periodic signaling.</param>
        /// <param name="dueTime">The amount of time to delay before the associated <see cref="NetWatcher"/> starts signaling. Specify negative one (-1) milliseconds to prevent the signaling from starting. Specify zero (0) to start the signaling immediately.</param>
        /// <param name="values">An array of URI locations that this <see cref="NetDependency"/> will monitor. When any of these resources changes, this <see cref="NetDependency"/> will notify any subscribing objects of the change.</param>
        /// <remarks>The <paramref name="checkResponseData"/> is useful when the web server you are probing does not contain the Last-Modified header.</remarks>
        public NetDependency(bool checkResponseData, TimeSpan dueTime, TimeSpan period, params Uri[] values) : this(values, checkResponseData, dueTime, period)
        {
        }
		#endregion

		#region Properties
		private DateTime UtcCreated { get; set; }

		private IEnumerable<NetWatcher> Watchers
		{
		    get; set;
		}

		/// <summary>
		/// Gets a value indicating whether the <see cref="Dependency"/> object has changed.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if the <see cref="Dependency"/> object has changed; otherwise, <c>false</c>.
		/// </value>
		public override bool HasChanged
		{
			get { return (UtcLastModified > UtcCreated); }
		}

        private IEnumerable<Uri> Uris { get; set; }

        private bool CheckResponseData { get; set; }

        private TimeSpan DueTime { get; set; }

        private TimeSpan Period { get; set; }
		#endregion

		#region Methods

	    private static IEnumerable<Uri> ToUriSequence(IEnumerable<string> uriStrings)
	    {
	        if (uriStrings == null) { return null; }
            List<Uri> uris = new List<Uri>();
            foreach (string uri in uriStrings)
            {
                Uri realUri;
                if (UriUtility.TryParse(uri, UriKind.Absolute, out realUri)) { uris.Add(realUri); }
            }
	        return uris;
	    }

        /// <summary>
        /// Starts and performs the necessary dependency tasks of this instance.
        /// </summary>
        /// <exception cref="System.ArgumentException">The provided Uri does not have a valid scheme attached. Allowed schemes for now is File, FTP or HTTP.;uris</exception>
		public override void Start()
		{
            List<NetWatcher> watchers = new List<NetWatcher>();
            foreach (Uri uri in Uris)
            {
                switch (UriSchemeConverter.FromString(uri.Scheme))
                {
                    case UriScheme.File:
                    case UriScheme.Http:
                    case UriScheme.Https:
                        NetWatcher watcher;
                        NetWatcher tempWatcher = null;
                        try
                        {
                            tempWatcher = new NetWatcher(uri, DueTime, Period, CheckResponseData);
                            tempWatcher.Changed += WatcherChanged;
                            watcher = tempWatcher;
                            tempWatcher = null;
                        }
                        finally
                        {
                            if (tempWatcher != null) { tempWatcher.Dispose(); }
                        }
                        watchers.Add(watcher);
                        break;
                    default:
                        throw new InvalidOperationException("The provided Uri does not have a valid scheme attached. Allowed schemes for now is File, HTTP or HTTPS.");
                }
            }
            Watchers = watchers.ToArray();
            UtcCreated = DateTime.UtcNow;
            SetUtcLastModified(UtcCreated);
		}

		private void WatcherChanged(object sender, WatcherEventArgs args)
		{
			SetUtcLastModified(DateTime.UtcNow);
			if (!HasChanged) { return; }
			if (Watchers != null)
			{
				lock (_locker)
				{
					if (Watchers != null)
					{
                        foreach (NetWatcher watcher in Watchers)
                        {
                            watcher.Changed -= WatcherChanged;
                            watcher.Dispose();
                        }
					}
                    Watchers = null;
				}
			}
			OnDependencyChangedRaised(new DependencyEventArgs(UtcLastModified));
		}
		#endregion
	}
}