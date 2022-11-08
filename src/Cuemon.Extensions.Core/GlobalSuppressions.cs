
// This file is used by Code Analysis to maintain SuppressMessage 
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given 
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Major Code Smell", "S3445:Exceptions should not be explicitly rethrown", Justification = "This is by design; we only want the stacktrace from within the validator method.", Scope = "member", Target = "~M:Cuemon.Extensions.ValidatorExtensions.HasDifference(Cuemon.Validator,System.String,System.String,System.String,System.String)")]
[assembly: SuppressMessage("Major Code Smell", "S3445:Exceptions should not be explicitly rethrown", Justification = "This is by design; we only want the stacktrace from within the validator method.", Scope = "member", Target = "~M:Cuemon.Extensions.ValidatorExtensions.NoDifference(Cuemon.Validator,System.String,System.String,System.String,System.String)")] 
[assembly: SuppressMessage("Usage", "CA2200:Rethrow to preserve stack details", Justification = "By design; we do not want to clutter the exception away from context.", Scope = "member", Target = "~M:Cuemon.Extensions.ValidatorExtensions.NoDifference(Cuemon.Validator,System.String,System.String,System.String,System.String)")]
[assembly: SuppressMessage("Usage", "CA2200:Rethrow to preserve stack details", Justification = "By design; we do not want to clutter the exception away from context.", Scope = "member", Target = "~M:Cuemon.Extensions.ValidatorExtensions.HasDifference(Cuemon.Validator,System.String,System.String,System.String,System.String)")]
[assembly: SuppressMessage("Usage", "CA2249:Consider using 'string.Contains' instead of 'string.IndexOf'", Justification = "False-Positive. Contains does not support comparison rules in NET Standard 2.", Scope = "member", Target = "~M:Cuemon.Extensions.StringExtensions.ContainsAny(System.String,System.String,System.StringComparison)~System.Boolean")]
[assembly: SuppressMessage("Major Code Smell", "S3445:Exceptions should not be explicitly rethrown", Justification = "This is by design; we only want the stacktrace from within the validator method.", Scope = "member", Target = "~M:Cuemon.Extensions.ValidatorExtensions.ContainsReservedKeyword(Cuemon.Validator,System.String,System.Collections.Generic.IEnumerable{System.String},System.Collections.Generic.IEqualityComparer{System.String},System.String,System.String)")]
[assembly: SuppressMessage("Minor Code Smell", "S3267:Loops should be simplified with \"LINQ\" expressions", Justification = "False-positive.", Scope = "member", Target = "~M:Cuemon.Extensions.StringExtensions.JsUnescape(System.String)~System.String")]
[assembly: SuppressMessage("Style", "IDE0220:Add explicit cast", Justification = "False-Positive", Scope = "member", Target = "~M:Cuemon.Extensions.StringExtensions.JsUnescape(System.String)~System.String")]
