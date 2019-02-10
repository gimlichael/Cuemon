using System;
using System.Data;

namespace Cuemon.Data
{
	/// <summary>
	/// Represents a database column.
	/// </summary>
	public sealed class DbColumn
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="DbColumn"/> class.
		/// </summary>
		/// <param name="columnDbType">The <see cref="DbType"/> of the database column.</param>
		/// <param name="columnValue">The value of the database column.</param>
		public DbColumn(DbType columnDbType, object columnValue)
		{
			ColumnDbType = columnDbType;
			ColumnValue = columnValue;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets the <see cref="DbType"/> of the database column.
		/// </summary>
		/// <value>
		/// The <see cref="DbType"/> of the database column.
		/// </value>
		public DbType ColumnDbType { get; private set; }

		/// <summary>
		/// Gets the <see cref="Type"/> of the database column.
		/// </summary>
		/// <value>
		/// The <see cref="Type"/> of the database column.
		/// </value>
		public Type ColumnType
		{
			get { return DataManager.ParseDbType(ColumnDbType); }
		}

		/// <summary>
		/// Gets the database column value.
		/// </summary>
		public object ColumnValue { get; private set; }
		#endregion

		#region Methods
		#endregion
	}
}