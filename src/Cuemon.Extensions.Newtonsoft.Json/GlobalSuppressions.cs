// This file is used by Code Analysis to maintain SuppressMessage 
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given 
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Major Code Smell", "S907:\"goto\" statement should not be used", Justification = "Legacy code ;-)", Scope = "member", Target = "~M:Cuemon.Extensions.Newtonsoft.Json.JsonReaderExtensions.ToHierarchy(Newtonsoft.Json.JsonReader)~Cuemon.IHierarchy{Cuemon.DataPair}")]
[assembly: SuppressMessage("Critical Code Smell", "S3776:Cognitive Complexity of methods should not be too high", Justification = "Legacy code ;-)", Scope = "member", Target = "~M:Cuemon.Extensions.Newtonsoft.Json.JsonReaderExtensions.ToHierarchy(Newtonsoft.Json.JsonReader)~Cuemon.IHierarchy{Cuemon.DataPair}")]
[assembly: SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "Left for debugging purposes.", Scope = "member", Target = "~P:Cuemon.Extensions.Newtonsoft.Json.DynamicJsonConverterCore.ObjectType")]
[assembly: SuppressMessage("Major Bug", "S3343:Caller information parameters should come at the end of the parameter list", Justification = "For consistency (and to align with Microsoft recent guard improvements), paramName should always be the 2nd parameter (when applicable) OR paramName and message goes next to each other.", Scope = "member", Target = "~M:Cuemon.Extensions.Newtonsoft.Json.ValidatorExtensions.InvalidJsonDocument(Cuemon.Validator,Newtonsoft.Json.JsonReader@,System.String,System.String)")]
[assembly: SuppressMessage("Major Bug", "S3343:Caller information parameters should come at the end of the parameter list", Justification = "For consistency (and to align with Microsoft recent guard improvements), paramName should always be the 2nd parameter (when applicable) OR paramName and message goes next to each other.", Scope = "member", Target = "~M:Cuemon.Extensions.Newtonsoft.Json.ValidatorExtensions.InvalidJsonDocument(Cuemon.Validator,System.String,System.String,System.String)")]
[assembly: SuppressMessage("Major Code Smell", "S1172:Unused method parameters should be removed", Justification = "False-positive; value is conditionally used.", Scope = "member", Target = "~M:Cuemon.Extensions.Newtonsoft.Json.Converters.ExceptionConverter.ParseJsonReader(Newtonsoft.Json.JsonReader,System.Type)~System.Collections.Generic.Stack{System.Collections.Generic.IList{Cuemon.Reflection.MemberArgument}}")]
