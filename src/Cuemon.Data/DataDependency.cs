using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using Cuemon.Runtime;

namespace Cuemon.Data
{
	/// <summary>
	/// This <see cref="DataDependency"/> class will monitor any changes occurred to an underlying data source while notifying subscribing objects.
	/// </summary>
	public sealed class DataDependency : Dependency
	{
        private readonly IDependency _defaultDependency;

		/// <summary>
		/// Initializes a new instance of the <see cref="DataDependency"/> class.
		/// </summary>
		/// <param name="manager">The <see cref="DataManager"/> to be used for the underlying data operations.</param>
		/// <param name="command">The <see cref="IDataCommand"/> to execute and monitor for changes.</param>
		/// <param name="parameters">An optional sequence of <see cref="DbParameter"/> to use with the associated <paramref name="command"/>.</param>
		/// <remarks>The signaling is default delayed 15 seconds before first invoke. Signaling occurs every 2 minutes.</remarks>
		public DataDependency(DataManager manager, IDataCommand command, params DbParameter[] parameters) : this(manager, command, TimeSpan.FromMinutes(2), parameters)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DataDependency"/> class.
		/// </summary>
		/// <param name="manager">The <see cref="DataManager"/> to be used for the underlying data operations.</param>
		/// <param name="command">The <see cref="IDataCommand"/> to execute and monitor for changes.</param>
		/// <param name="period">The time interval between periodic signaling for changes to the specified <paramref name="command"/> by the associated <see cref="DataWatcher"/>. Specify negative one (-1) milliseconds to disable periodic signaling.</param>
		/// <param name="parameters">An optional sequence of <see cref="DbParameter"/> to use with the associated <paramref name="command"/>.</param>
		/// <remarks>The signaling is default delayed 15 seconds before first invoke.</remarks>
		public DataDependency(DataManager manager, IDataCommand command, TimeSpan period, params DbParameter[] parameters) : this(manager, command, TimeSpan.FromSeconds(15), period, parameters)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DataDependency"/> class.
		/// </summary>
		/// <param name="manager">The <see cref="DataManager"/> to be used for the underlying data operations.</param>
		/// <param name="command">The <see cref="IDataCommand"/> to execute and monitor for changes.</param>
		/// <param name="dueTime">The amount of time to delay before the associated <see cref="DataWatcher"/> starts signaling. Specify negative one (-1) milliseconds to prevent the signaling from starting. Specify zero (0) to start the signaling immediately.</param>
		/// <param name="period">The time interval between periodic signaling for changes to the specified <paramref name="command"/> by the associated <see cref="DataWatcher"/>. Specify negative one (-1) milliseconds to disable periodic signaling.</param>
		/// <param name="parameters">An optional sequence of <see cref="DbParameter"/> to use with the associated <paramref name="command"/>.</param>
		public DataDependency(DataManager manager, IDataCommand command, TimeSpan dueTime, TimeSpan period, params DbParameter[] parameters) : this(manager, command, dueTime, period, TimeSpan.Zero, parameters)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DataDependency"/> class.
		/// </summary>
		/// <param name="manager">The <see cref="DataManager"/> to be used for the underlying data operations.</param>
		/// <param name="command">The <see cref="IDataCommand"/> to execute and monitor for changes.</param>
		/// <param name="dueTime">The amount of time to delay before the associated <see cref="DataWatcher"/> starts signaling. Specify negative one (-1) milliseconds to prevent the signaling from starting. Specify zero (0) to start the signaling immediately.</param>
		/// <param name="period">The time interval between periodic signaling for changes to the specified <paramref name="command"/> by the associated <see cref="DataWatcher"/>. Specify negative one (-1) milliseconds to disable periodic signaling.</param>
		/// <param name="parameters">An optional sequence of <see cref="DbParameter"/> to use with the associated <paramref name="command"/>.</param>
		/// <param name="dueTimeOnChanged">The amount of time to postpone a <see cref="Watcher.Changed"/> event. Specify zero (0) to disable postponing.</param>
		public DataDependency(DataManager manager, IDataCommand command, TimeSpan dueTime, TimeSpan period, TimeSpan dueTimeOnChanged, params DbParameter[] parameters) : base(watcherChanged =>
        {
            var watchers = new List<DataWatcher>();

			// code to be written

            return watchers;
        }, false)
		{
		    Manager = manager;
		    Command = command;
		    DueTime = dueTime;
		    Period = period;
		    DueTimeOnChanged = dueTimeOnChanged;
		    Parameters = parameters;
        }

        private DataManager Manager { get; set; }

        private IDataCommand Command { get; set; }

        private TimeSpan DueTime { get; set; }

        private TimeSpan Period { get; set; }

        private TimeSpan DueTimeOnChanged { get; set; }

        private DbParameter[] Parameters { get; set; }

        /// <summary>
        /// Starts and performs the necessary dependency tasks of this instance.
        /// </summary>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public override Task StartAsync()
        {
            return _defaultDependency.StartAsync();
        }
    }
}