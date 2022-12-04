// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Major Code Smell", "S1118:Utility classes should not have public constructors", Justification = "Special implementation; Program.cs is only available runtime when active configuration is set to Surrogate (works like a console app).", Scope = "type", Target = "~T:Cuemon.Extensions.Globalization.Program")]
[assembly: SuppressMessage("Minor Code Smell", "S3963:\"static\" fields should be initialized inline", Justification = "Special implementation; Program.cs is only available runtime when active configuration is set to Surrogate (works like a console app).", Scope = "member", Target = "~M:Cuemon.Extensions.Globalization.Program.#cctor")]
[assembly: SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Internal class; should not be subject for CA.", Scope = "member", Target = "~M:Cuemon.Extensions.Globalization.CultureInfoSurrogate.#ctor(System.Runtime.Serialization.SerializationInfo,System.Runtime.Serialization.StreamingContext)")]
[assembly: SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Internal class; should not be subject for CA.", Scope = "member", Target = "~M:Cuemon.Extensions.Globalization.Program.Main(System.String[])")]
