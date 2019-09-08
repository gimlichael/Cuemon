using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Cuemon.Collections.Generic;
using Cuemon.Runtime;

namespace Cuemon.Data
{
	/// <summary>
	/// A <see cref="Watcher"/> implementation, that can monitor and signal changes of one or more data locations by raising the <see cref="Watcher.Changed"/> event.
	/// </summary>
	public sealed class DataWatcher : Watcher
	{
		private readonly object _locker = new object();

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="DataWatcher"/> class.
		/// </summary>
		/// <param name="manager">The <see cref="DataManager"/> to be used for the underlying data operations.</param>
		/// <param name="command">The <see cref="IDataCommand"/> to execute and monitor for changes.</param>
		/// <param name="parameters">An optional sequence of <see cref="DbParameter"/> to use with the associated <paramref name="command"/>.</param>
		/// <remarks>Monitors the provided <paramref name="command"/> for changes in an interval of two minutes using a MD5 hash check on the query result. The signaling is default delayed 15 seconds before first invoke.</remarks>
		public DataWatcher(DataManager manager, IDataCommand command, params DbParameter[] parameters) : this(manager, command, TimeSpan.FromMinutes(2), parameters)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DataWatcher"/> class.
		/// </summary>
		/// <param name="manager">The <see cref="DataManager"/> to be used for the underlying data operations.</param>
		/// <param name="command">The <see cref="IDataCommand"/> to execute and monitor for changes.</param>
		/// <param name="period">The time interval between periodic signaling for changes of the provided <paramref name="command"/>.</param>
		/// <param name="parameters">An optional array of <see cref="DbParameter"/> to use with the associated <paramref name="command"/>.</param>
		/// <remarks>Monitors the provided <paramref name="command"/> for changes in an interval specified by <paramref name="period"/> using a MD5 hash check on the query result. The signaling is default delayed 15 seconds before first invoke.</remarks>
		public DataWatcher(DataManager manager, IDataCommand command, TimeSpan period, params DbParameter[] parameters) : this(manager, command, TimeSpan.FromSeconds(15), period, TimeSpan.Zero, parameters)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DataWatcher"/> class.
		/// </summary>
		/// <param name="manager">The <see cref="DataManager"/> to be used for the underlying data operations.</param>
		/// <param name="command">The <see cref="IDataCommand"/> to execute and monitor for changes.</param>
		/// <param name="dueTime">The amount of time to delay before the associated <see cref="Watcher"/> starts signaling. Specify negative one (-1) milliseconds to prevent the signaling from starting. Specify zero (0) to start the signaling immediately.</param>
		/// <param name="period">The time interval between periodic signaling for changes of the provided <paramref name="command"/>.</param>
		/// <param name="dueTimeOnChanged">The amount of time to postpone a <see cref="Watcher.Changed"/> event. Specify zero (0) to disable postponing.</param>
		/// <param name="parameters">An optional array of <see cref="DbParameter"/> to use with the associated <paramref name="command"/>.</param>
		/// <remarks>Monitors the provided <paramref name="command"/> for changes in an interval specified by <paramref name="period"/> using a MD5 hash check on the query result.</remarks>
		public DataWatcher(DataManager manager, IDataCommand command, TimeSpan dueTime, TimeSpan period, TimeSpan dueTimeOnChanged, params DbParameter[] parameters) : base(dueTime, period, dueTimeOnChanged)
		{
            Validator.ThrowIfNull(command, nameof(command));
            Manager = manager;
			Command = command;
			Parameters = parameters;
			Signature = 0;
		}
		#endregion

		#region Properties
		private long Signature { get; set; }
		/// <summary>
		/// Gets the associated <see cref="IDataCommand"/> of this <see cref="DataWatcher"/>.
		/// </summary>
		/// <value>The associated <see cref="IDataCommand"/> of this <see cref="DataWatcher"/>.</value>
		public IDataCommand Command { get; private set; }

		/// <summary>
		/// Gets the associated array of <see cref="DbParameter"/> of this <see cref="DataWatcher"/>.
		/// </summary>
        /// <value>The associated array of <see cref="DbParameter"/> of this <see cref="DataWatcher"/>.</value>
		public DbParameter[] Parameters { get; private set; }

		/// <summary>
		/// Gets the associated <see cref="DataManager"/> of this <see cref="DataWatcher"/>.
		/// </summary>
		/// <value>The associated <see cref="DataManager"/> of this <see cref="DataWatcher"/>.</value>
		public DataManager Manager { get; private set; }
		#endregion

		#region Methods
		/// <summary>
		/// Handles the signaling of this <see cref="DataWatcher"/>.
		/// </summary>
		protected override void HandleSignaling()
		{
			lock (_locker)
			{
				var utcLastModified = DateTime.UtcNow;
                var values = new List<object[]>();
			    var manager = Manager.Clone();
				using (var reader = manager.ExecuteReader(Command, Parameters))
				{
					while (reader.Read())
					{
						var readerValues = new object[reader.FieldCount];
						reader.GetValues(readerValues);
						values.Add(readerValues);
					}
				}
                var currentSignature = Generate.HashCode64(values.Select(o => o.GetHashCode()).Cast<IConvertible>());
                values.Clear();

				if (Signature == 0) { Signature = currentSignature; }
				if (!Signature.Equals(currentSignature))
				{
					SetUtcLastModified(utcLastModified);
					OnChangedRaised();
				}
				Signature = currentSignature;
			}
		}
		#endregion
	}
}