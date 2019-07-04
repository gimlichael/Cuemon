using System;
using System.Data.Common;

namespace Cuemon.Data
{
    /// <summary>
    /// An abstract class representing the actual data binding to a data source.
    /// </summary>
    public abstract class DataAdapter
    {
        private readonly object _padLock = new object();

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="DataAdapter"/> class.
        /// </summary>
        protected DataAdapter()
        {
            // this should only be called when we are doing binding through System.Reflections.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataAdapter"/> class.
        /// </summary>
        /// <param name="manager">The data manager as underlying DSL wrapper logic.</param>
        protected DataAdapter(DataManager manager)
        {
            Manager = manager;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the data manager for this object.
        /// Please be aware, that you should only use this for custom methods as you will loose event control on Entity classes by using the manager directly.
        /// </summary>
        /// <value>A <b><see cref="DataManager"/></b> object.</value>
        public DataManager Manager { get; private set; }
        #endregion

        #region Events
        /// <summary>
        /// Occurs before a Delete operation.
        /// </summary>
        public event EventHandler<DataAdapterEventArgs> Deleting;

        /// <summary>
        /// Occurs when a Delete operation has completed.
        /// </summary>
        public event EventHandler<DataAdapterEventArgs> Deleted;

        /// <summary>
        /// Occurs when an Insert operation has completed.
        /// </summary>
        public event EventHandler<DataAdapterEventArgs> Inserted;

        /// <summary>
        /// Occurs before an Insert operation.
        /// </summary>
        public event EventHandler<DataAdapterEventArgs> Inserting;

        /// <summary>
        /// Occurs when a Select operation has completed.
        /// </summary>
        public event EventHandler<DataAdapterEventArgs> Selected;

        /// <summary>
        /// Occurs before a Select operation.
        /// </summary>
        public event EventHandler<DataAdapterEventArgs> Selecting;

        /// <summary>
        /// Occurs when an Update operation has completed.
        /// </summary>
        public event EventHandler<DataAdapterEventArgs> Updated;

        /// <summary>
        /// Occurs before an Update operation.
        /// </summary>
        public event EventHandler<DataAdapterEventArgs> Updating;
        #endregion

        #region Methods
        /// <summary>
        /// Raises the <see cref="Deleted"/> event.
        /// </summary>
        /// <param name="e">The <see cref="DataAdapterEventArgs"/> instance containing the event data.</param>
        protected virtual void OnDeletedRaised(DataAdapterEventArgs e)
        {
            var handler = Deleting;
            handler?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the <see cref="Deleting"/> event.
        /// </summary>
        /// <param name="e">The <see cref="DataAdapterEventArgs"/> instance containing the event data.</param>
        protected virtual void OnDeletingRaised(DataAdapterEventArgs e)
        {
            var handler = Deleted;
            handler?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the <see cref="Inserted"/> event.
        /// </summary>
        /// <param name="e">The <see cref="DataAdapterEventArgs"/> instance containing the event data.</param>
        protected virtual void OnInsertedRaised(DataAdapterEventArgs e)
        {
            var handler = Inserted;
            handler?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the <see cref="Inserting"/> event.
        /// </summary>
        /// <param name="e">The <see cref="DataAdapterEventArgs"/> instance containing the event data.</param>
        protected virtual void OnInsertingRaised(DataAdapterEventArgs e)
        {
            var handler = Inserting;
            handler?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the <see cref="Selected"/> event.
        /// </summary>
        /// <param name="e">The <see cref="DataAdapterEventArgs"/> instance containing the event data.</param>
        protected virtual void OnSelectedRaised(DataAdapterEventArgs e)
        {
            var handler = Selected;
            handler?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the <see cref="Selecting"/> event.
        /// </summary>
        /// <param name="e">The <see cref="DataAdapterEventArgs"/> instance containing the event data.</param>
        protected virtual void OnSelectingRaised(DataAdapterEventArgs e)
        {
            var handler = Selecting;
            handler?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the <see cref="Updated"/> event.
        /// </summary>
        /// <param name="e">The <see cref="DataAdapterEventArgs"/> instance containing the event data.</param>
        protected virtual void OnUpdatedRaised(DataAdapterEventArgs e)
        {
            var handler = Updated;
            handler?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the <see cref="Updating"/> event.
        /// </summary>
        /// <param name="e">The <see cref="DataAdapterEventArgs"/> instance containing the event data.</param>
        protected virtual void OnUpdatingRaised(DataAdapterEventArgs e)
        {
            var handler = Updating;
            handler?.Invoke(this, e);
        }

        /// <summary>
        /// Deletes data from a data source.
        /// </summary>
        /// <param name="dataCommand">The data command to execute.</param>
        /// <param name="parameters">The parameters to use in the command.</param>
        protected void Delete(IDataCommand dataCommand, params DbParameter[] parameters)
        {
            ExecuteDelete(dataCommand, parameters);
        }

        /// <summary>
        /// Inserts data to a data source with default insert action, AffectedRows.
        /// </summary>
        /// <param name="dataCommand">The data command to execute.</param>
        /// <param name="parameters">The parameters to use in the command.</param>
        /// <returns>A <see cref="void"/> object.</returns>
        protected object Insert(IDataCommand dataCommand, params DbParameter[] parameters)
        {
            return ExecuteInsert(dataCommand, QueryInsertAction.AffectedRows, parameters);
        }

        /// <summary>
        /// Inserts data to a data source.
        /// </summary>
        /// <param name="dataCommand">The data command to execute.</param>
        /// <param name="action">The insert action you wish to apply.</param>
        /// <param name="parameters">The parameters to use in the command.</param>
        /// <returns>A <see cref="void"/> object.</returns>
        protected object Insert(IDataCommand dataCommand, QueryInsertAction action, params DbParameter[] parameters)
        {
            return ExecuteInsert(dataCommand, action, parameters);
        }

        /// <summary>
        /// Selects data from a data source.
        /// </summary>
        /// <param name="dataCommand">The data command to execute.</param>
        /// <param name="parameters">The parameters to use in the command.</param>
        /// <returns>An object supporting the <see cref="DbDataReader"/> interface.</returns>
        protected DbDataReader Select(IDataCommand dataCommand, params DbParameter[] parameters)
        {
            return ExecuteSelect(dataCommand, parameters);
        }

        /// <summary>
        /// Updates data in the data source.
        /// </summary>
        /// <param name="dataCommand">The data command to execute.</param>
        /// <param name="parameters">The parameters to use in the command.</param>
        protected void Update(IDataCommand dataCommand, params DbParameter[] parameters)
        {
            ExecuteUpdate(dataCommand, parameters);            
        }

        /// <summary>
        /// Executes the delete statement for the Delete method.
        /// </summary>
        /// <param name="dataCommand">The data command to execute.</param>
        /// <param name="parameters">The parameters to use in the command.</param>
        private void ExecuteDelete(IDataCommand dataCommand, params DbParameter[] parameters)
        {
            lock (_padLock)
            {
                OnDeletingRaised(DataAdapterEventArgs.Empty);
                Manager.Execute(dataCommand, parameters);
                OnDeletedRaised(DataAdapterEventArgs.Empty);
            }
        }

        /// <summary>
        /// Executes the insert statement for the Insert method.
        /// </summary>
        /// <param name="dataCommand">The data command to execute.</param>
        /// <param name="action">The insert action you wish to apply.</param>
        /// <param name="parameters">The parameters to use in the command.</param>
        /// <returns></returns>
        private object ExecuteInsert(IDataCommand dataCommand, QueryInsertAction action, params DbParameter[] parameters)
        {
            lock (_padLock)
            {
                OnInsertingRaised(DataAdapterEventArgs.Empty);
                object value = null;
                switch (action)
                {
                    case QueryInsertAction.AffectedRows:
                        value = Manager.Execute(dataCommand, parameters);
                        break;
                    case QueryInsertAction.IdentityDecimal:
                        value = Manager.ExecuteIdentityDecimal(dataCommand, parameters);
                        break;
                    case QueryInsertAction.IdentityInt32:
                        value = Manager.ExecuteIdentityInt32(dataCommand, parameters);
                        break;
                    case QueryInsertAction.IdentityInt64:
                        value = Manager.ExecuteIdentityInt64(dataCommand, parameters);
                        break;
                    case QueryInsertAction.Void:
                        Manager.Execute(dataCommand, parameters);
                        break;
                }
                OnInsertedRaised(DataAdapterEventArgs.Empty);
                return value;
            }
        }

        /// <summary>
        /// Executes the select statement for the Select method.
        /// </summary>
        /// <param name="dataCommand">The data command to execute.</param>
        /// <param name="parameters">The parameters to use in the command.</param>
        private DbDataReader ExecuteSelect(IDataCommand dataCommand, params DbParameter[] parameters)
        {
            lock (_padLock)
            {
                OnSelectingRaised(DataAdapterEventArgs.Empty);
                var reader = Manager.ExecuteReader(dataCommand, parameters);
                OnSelectedRaised(DataAdapterEventArgs.Empty);
                return reader;
            }
        }

        /// <summary>
        /// Executes the update statement Update method.
        /// </summary>
        /// <param name="dataCommand">The data command.</param>
        /// <param name="parameters">The parameters.</param>
        private void ExecuteUpdate(IDataCommand dataCommand, params DbParameter[] parameters)
        {
            lock (_padLock)
            {
                OnUpdatingRaised(DataAdapterEventArgs.Empty);
                Manager.Execute(dataCommand, parameters);
                OnUpdatedRaised(DataAdapterEventArgs.Empty);
            }
        }
        #endregion
    }
}