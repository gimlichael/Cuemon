using System;
using System.Collections.Generic;
using System.Linq;

namespace Cuemon.Runtime.Caching
{
    /// <summary>
    /// An internal representation of a Cache object.
    /// </summary>
    internal class Cache
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Cache"/> class.
        /// </summary>
        /// <param name="key">The identifier of this <see cref="Cache"/>.</param>
        /// <param name="value">The cached value of this <see cref="Cache"/>.</param>
        /// <param name="group">The group to associate and organize this <see cref="Cache"/> by.</param>
        /// <param name="dependencies">A sequence of <see cref="Dependency"/> objects for the item. When any dependency changes, the object becomes invalid and is removed from the cache. If there are no dependencies, this parameter contains a null reference (Nothing in Visual Basic).</param>
        /// <param name="absoluteExpiration">The absolute expiration date time value of this <see cref="Cache"/>.</param>
        /// <param name="slidingExpiration">The sliding expiration value of this <see cref="Cache"/>.</param>
        internal Cache(string key, object value, string group, IEnumerable<IDependency> dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration)
        {
            Key = key;
            Value = value;
            Group = group;
            Dependencies = dependencies == null ? null : new List<IDependency>(dependencies);
            AbsoluteExpiration = absoluteExpiration.ToUniversalTime();
            SlidingExpiration = slidingExpiration;
            Created = DateTime.UtcNow;
            LastAccessed = Created;
        }
        #endregion

        #region Events
        /// <summary>
        /// Occurs when a <see cref="Cache"/> object with a <see cref="Dependency"/> has expired.
        /// </summary>
        public event EventHandler<CacheEventArgs> Expired;
        #endregion

        #region Properties
        /// <summary>
        /// Gets the identifier of this <see cref="Cache"/>.
        /// </summary>
        /// <value>The identifier of this <see cref="Cache"/>.</value>
        public string Key { get; private set; }

        /// <summary>
        /// Gets the value of this <see cref="Cache"/>.
        /// </summary>
        /// <value>The value of this <see cref="Cache"/>.</value>
        public object Value { get; set; }

        /// <summary>
        /// Gets the group to associate and organize this <see cref="Cache"/> by.
        /// </summary>
        /// <value>The group to associate and organize this <see cref="Cache"/> by.</value>
        public string Group { get; private set; }

        /// <summary>
        /// Gets a sequence of objects implementing the <see cref="IDependency"/> interface assigned to this <see cref="Cache"/>.
        /// </summary>
        /// <value>A sequence of objects implementing the <see cref="IDependency"/> interface assigned to this <see cref="Cache"/>.</value>
        public IEnumerable<IDependency> Dependencies { get; private set; }

        /// <summary>
        /// Gets the UTC absolute expiration date time value of this <see cref="Cache"/>.
        /// </summary>
        /// <value>The UTC absolute expiration date time value of this <see cref="Cache"/>.</value>
        public DateTime AbsoluteExpiration { get; private set; }

        /// <summary>
        /// Gets the UTC date time value from when this <see cref="Cache"/> was created.
        /// </summary>
        /// <value>The UTC date time value from when this <see cref="Cache"/> was created.</value>
        public DateTime Created { get; private set; }

        /// <summary>
        /// Gets the UTC date time value from when this <see cref="Cache"/> was last accessed.
        /// </summary>
        /// <value>The UTC date time value from when this <see cref="Cache"/> was last accessed.</value>
        public DateTime LastAccessed { get; private set; }

        /// <summary>
        /// Gets the sliding expiration value of this <see cref="Cache"/>.
        /// </summary>
        /// <value>The sliding expiration value of this <see cref="Cache"/>.</value>
        public TimeSpan SlidingExpiration { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Cache"/> should use the AbsoluteExpiration property for the caching logic.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this <see cref="Cache"/> should use the AbsoluteExpiration property for the caching logic; otherwise, <c>false</c>.
        /// </value>
        public bool UseAbsoluteExpiration
        {
            get { return (DateTime.MaxValue.ToUniversalTime() != AbsoluteExpiration); }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Cache"/> should use the SlidingExpiration property for the caching logic.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this <see cref="Cache"/> should use the SlidingExpiration property for the caching logic; otherwise, <c>false</c>.
        /// </value>
        public bool UseSlidingExpiration
        {
            get { return (TimeSpan.Zero != SlidingExpiration); }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Cache"/> is relying on a <see cref="Dependency"/> object.
        /// </summary>
        /// <value><c>true</c> if this <see cref="Cache"/> is relying on a <see cref="Dependency"/> object; otherwise, <c>false</c>.</value>
        public bool UseDependency 
        {
            get { return (Dependencies != null && Dependencies.Any()); }
        }

        /// <summary>
        /// Determines whether the specified time resolves this <see cref="Cache"/> as expired.
        /// </summary>
        /// <param name="time">The date and time to evaluate against.</param>
        /// <returns>
        /// 	<c>true</c> if the specified time resolves this <see cref="Cache"/> as expired; otherwise, <c>false</c>.
        /// </returns>
        public bool HasExpired(DateTime time)
        {
            if (!CanExpire) { return false; }
            if (UseAbsoluteExpiration)
            {
                if (time >= AbsoluteExpiration) { return true; }
            }
            else if (UseSlidingExpiration)
            {
                TimeSpan currentPeriod = (time - LastAccessed);
                if (currentPeriod >= SlidingExpiration) { return true; }
            }
            else if (UseDependency)
            {
                foreach (IDependency dependency in Dependencies)
                {
                    if (dependency.HasChanged) { return true; }
                }
            }
            return false;
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Cache"/> can expire.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this <see cref="Cache"/> can expire; otherwise, <c>false</c>.
        /// </value>
        public bool CanExpire
        {
            get { return UseAbsoluteExpiration || UseSlidingExpiration || (UseDependency); }
        }
        #endregion

        #region Methods

        internal void StartDependencies()
        {
            if (!UseDependency) { return; }
            foreach (IDependency dependency in Dependencies)
            {
                dependency.DependencyChanged += ProcessDependencyChanged;
                dependency.Start();
            }
        }

        /// <summary>
        /// Refreshes the UTC date time value from when this <see cref="Cache"/> was created.
        /// </summary>
        public void Refresh()
        {
            LastAccessed = DateTime.UtcNow;
        }

        private void ProcessDependencyChanged(object sender, DependencyEventArgs e)
        {
            OnExpiredRaised(new CacheEventArgs(this));
            if (UseDependency)
            {
                foreach (IDependency dependency in Dependencies)
                {
                    dependency.DependencyChanged -= ProcessDependencyChanged;
                }
            }
        }

        /// <summary>
        /// Raises the <see cref="Expired"/> event.
        /// </summary>
        /// <param name="e">The <see cref="CacheEventArgs"/> instance containing the event data.</param>
        protected virtual void OnExpiredRaised(CacheEventArgs e)
        {
            Expired?.Invoke(this, e);
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="string" /> that represents this instance.</returns>
        public override string ToString()
        {
            return Generate.ObjectPortrayal(this, o => o.BypassOverrideCheck = true);
        }

        #endregion
    }
}