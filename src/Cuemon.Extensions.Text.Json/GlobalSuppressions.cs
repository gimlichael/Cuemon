// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Critical Code Smell", "S3776:Cognitive Complexity of methods should not be too high", Justification = "Transitioned legacy code ;-)", Scope = "member", Target = "~M:Cuemon.Extensions.Text.Json.JsonReaderExtensions.ToHierarchy(System.Text.Json.Utf8JsonReader)~Cuemon.IHierarchy{Cuemon.DataPair}")]
[assembly: SuppressMessage("Major Code Smell", "S907:\"goto\" statement should not be used", Justification = "Transitioned legacy code ;-)", Scope = "member", Target = "~M:Cuemon.Extensions.Text.Json.JsonReaderExtensions.ToHierarchy(System.Text.Json.Utf8JsonReader)~Cuemon.IHierarchy{Cuemon.DataPair}")]
[assembly: SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "False-positive; .NET 7 reads this value.", Scope = "member", Target = "~P:Cuemon.Extensions.Text.Json.Converters.FlagsEnumConverter.TypeToConvert")]
[assembly: SuppressMessage("Major Code Smell", "S1172:Unused method parameters should be removed", Justification = "False-positive; value is conditionally used.", Scope = "member", Target = "~M:Cuemon.Extensions.Text.Json.Converters.ExceptionConverter.ParseJsonReader(System.Text.Json.Utf8JsonReader@,System.Type)~System.Collections.Generic.Stack{System.Collections.Generic.IList{Cuemon.Reflection.MemberArgument}}")]
