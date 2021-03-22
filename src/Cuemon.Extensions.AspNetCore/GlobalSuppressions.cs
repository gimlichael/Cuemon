// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Performance", "CA1835:Prefer the 'Memory'-based overloads for 'ReadAsync' and 'WriteAsync'", Justification = "Already fixed for .NET 5, but SonarCloud cannot figure out multiple framework support, eg. NETStandard 2.", Scope = "member", Target = "~M:Cuemon.Extensions.AspNetCore.Http.HttpResponseExtensions.WriteBodyAsync(Microsoft.AspNetCore.Http.HttpResponse,System.Func{System.Byte[]})~System.Threading.Tasks.Task")]
