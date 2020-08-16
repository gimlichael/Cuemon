// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Major Bug", "S2259:Null pointers should not be dereferenced", Justification = "The variable, fieldCount, is initialized to -1 in case of record being null - hence, the for loop will never iterate.", Scope = "member", Target = "~M:Cuemon.Data.DataTransferColumnCollection.#ctor(System.Data.IDataRecord)")]
