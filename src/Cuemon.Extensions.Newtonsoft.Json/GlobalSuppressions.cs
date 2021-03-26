// This file is used by Code Analysis to maintain SuppressMessage 
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given 
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Major Code Smell", "S3445:Exceptions should not be explicitly rethrown", Justification = "This is by design; we only want the stacktrace from within the validator method.", Scope = "member", Target = "~M:Cuemon.Extensions.Newtonsoft.Json.ValidatorExtensions.InvalidJsonDocument(Cuemon.Validator,Newtonsoft.Json.JsonReader@,System.String,System.String)")]
[assembly: SuppressMessage("Major Code Smell", "S907:\"goto\" statement should not be used", Justification = "Legacy code ;-)", Scope = "member", Target = "~M:Cuemon.Extensions.Newtonsoft.Json.JsonReaderExtensions.ToHierarchy(Newtonsoft.Json.JsonReader)~Cuemon.IHierarchy{Cuemon.DataPair}")]
[assembly: SuppressMessage("Critical Code Smell", "S3776:Cognitive Complexity of methods should not be too high", Justification = "Legacy code ;-)", Scope = "member", Target = "~M:Cuemon.Extensions.Newtonsoft.Json.JsonReaderExtensions.ToHierarchy(Newtonsoft.Json.JsonReader)~Cuemon.IHierarchy{Cuemon.DataPair}")]
[assembly: SuppressMessage("Usage", "CA2200:Rethrow to preserve stack details", Justification = "By design; we do not want to clutter the exception away from context.", Scope = "member", Target = "~M:Cuemon.Extensions.Newtonsoft.Json.ValidatorExtensions.InvalidJsonDocument(Cuemon.Validator,Newtonsoft.Json.JsonReader@,System.String,System.String)")]
[assembly: SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "Left for debugging purposes.", Scope = "member", Target = "~P:Cuemon.Extensions.Newtonsoft.Json.DynamicJsonConverterCore.ObjectType")]
