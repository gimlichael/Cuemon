
// This file is used by Code Analysis to maintain SuppressMessage 
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given 
// a specific target and scoped to a namespace, type, member, etc.

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S3445:Exceptions should not be explicitly rethrown", Justification = "This is by design; we only want the stacktrace from within the validator method.", Scope = "member", Target = "~M:Cuemon.Extensions.Newtonsoft.Json.ValidatorExtensions.IfNotValidJsonDocument(Cuemon.Validator,Newtonsoft.Json.JsonReader@,System.String,System.String)")]