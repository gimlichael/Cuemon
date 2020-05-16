// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Critical Code Smell", "S3217:\"Explicit\" conversions of \"foreach\" loops should not be used", Justification = "The class is designed for SQL server.", Scope = "member", Target = "~M:Cuemon.Extensions.Data.SqlClient.SqlDataManager.GetCommandCore(Cuemon.Data.IDataCommand,System.Data.Common.DbParameter[])~System.Data.Common.DbCommand")]
[assembly: SuppressMessage("Major Code Smell", "S1066:Collapsible \"if\" statements should be merged", Justification = "Kept for readability.", Scope = "member", Target = "~M:Cuemon.Extensions.Data.SqlClient.SqlDataManager.GetCommandCore(Cuemon.Data.IDataCommand,System.Data.Common.DbParameter[])~System.Data.Common.DbCommand")]
