
// This file is used by Code Analysis to maintain SuppressMessage 
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given 
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Usage", "CA2249:Consider using 'string.Contains' instead of 'string.IndexOf'", Justification = "False-Positive. Contains does not support comparison rules in NET Standard 2.", Scope = "member", Target = "~M:Cuemon.Extensions.StringExtensions.ContainsAny(System.String,System.String,System.StringComparison)~System.Boolean")]
[assembly: SuppressMessage("Minor Code Smell", "S3267:Loops should be simplified with \"LINQ\" expressions", Justification = "False-positive.", Scope = "member", Target = "~M:Cuemon.Extensions.StringExtensions.JsUnescape(System.String)~System.String")]
[assembly: SuppressMessage("Style", "IDE0220:Add explicit cast", Justification = "False-Positive", Scope = "member", Target = "~M:Cuemon.Extensions.StringExtensions.JsUnescape(System.String)~System.String")]
[assembly: SuppressMessage("Major Bug", "S3343:Caller information parameters should come at the end of the parameter list", Justification = "For consistency (and to align with Microsoft recent guard improvements), paramName should always be the 2nd parameter (when applicable) OR paramName and message goes next to each other.", Scope = "member", Target = "~M:Cuemon.Extensions.ValidatorExtensions.ContainsReservedKeyword(Cuemon.Validator,System.String,System.Collections.Generic.IEnumerable{System.String},System.Collections.Generic.IEqualityComparer{System.String},System.String,System.String)")]
[assembly: SuppressMessage("Major Bug", "S3343:Caller information parameters should come at the end of the parameter list", Justification = "For consistency (and to align with Microsoft recent guard improvements), paramName should always be the 2nd parameter (when applicable) OR paramName and message goes next to each other.", Scope = "member", Target = "~M:Cuemon.Extensions.ValidatorExtensions.ContainsReservedKeyword(Cuemon.Validator,System.String,System.Collections.Generic.IEnumerable{System.String},System.String,System.String)")]
