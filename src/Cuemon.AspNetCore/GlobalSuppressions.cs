// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Major Code Smell", "S3442:\"abstract\" classes should not have \"public\" constructors", Justification = "Infrastructure class.", Scope = "member", Target = "~M:Cuemon.AspNetCore.Infrastructure.ConfigurableMiddlewareCore`1.#ctor(Microsoft.AspNetCore.Http.RequestDelegate,System.Action{`0})")]
[assembly: SuppressMessage("Major Code Smell", "S3442:\"abstract\" classes should not have \"public\" constructors", Justification = "Infrastructure class.", Scope = "member", Target = "~M:Cuemon.AspNetCore.Infrastructure.MiddlewareCore.#ctor(Microsoft.AspNetCore.Http.RequestDelegate)")]
[assembly: SuppressMessage("Major Code Smell", "S3442:\"abstract\" classes should not have \"public\" constructors", Justification = "Infrastructure class.", Scope = "member", Target = "~M:Cuemon.AspNetCore.Infrastructure.ConfigurableMiddlewareCore`1.#ctor(Microsoft.AspNetCore.Http.RequestDelegate,Microsoft.Extensions.Options.IOptions{`0})")]
#if NETCOREAPP3_1_OR_GREATER
[assembly: SuppressMessage("Critical Code Smell", "S927:parameter names should match base declaration and other partial definitions", Justification = "Generic parameter; implementation reflects usage.", Scope = "member", Target = "~M:Cuemon.AspNetCore.Hosting.HostingEnvironmentMiddleware.InvokeAsync(Microsoft.AspNetCore.Http.HttpContext,Microsoft.Extensions.Hosting.IHostEnvironment)~System.Threading.Tasks.Task")]
#endif
[assembly: SuppressMessage("Major Code Smell", "S2436:Types and methods should not have too many generic parameters", Justification = "By design; allow up to a max. of 5 generic parameters.", Scope = "type", Target = "~T:Cuemon.AspNetCore.ConfigurableMiddleware`3")]
[assembly: SuppressMessage("Major Code Smell", "S2436:Types and methods should not have too many generic parameters", Justification = "By design; allow up to a max. of 5 generic parameters.", Scope = "type", Target = "~T:Cuemon.AspNetCore.ConfigurableMiddleware`4")]
[assembly: SuppressMessage("Major Code Smell", "S2436:Types and methods should not have too many generic parameters", Justification = "By design; allow up to a max. of 5 generic parameters.", Scope = "type", Target = "~T:Cuemon.AspNetCore.ConfigurableMiddleware`5")]
[assembly: SuppressMessage("Major Code Smell", "S2436:Types and methods should not have too many generic parameters", Justification = "By design; allow up to a max. of 5 generic parameters.", Scope = "type", Target = "~T:Cuemon.AspNetCore.ConfigurableMiddleware`6")]
[assembly: SuppressMessage("Major Code Smell", "S2436:Types and methods should not have too many generic parameters", Justification = "By design; allow up to a max. of 5 generic parameters.", Scope = "type", Target = "~T:Cuemon.AspNetCore.Middleware`3")]
[assembly: SuppressMessage("Major Code Smell", "S2436:Types and methods should not have too many generic parameters", Justification = "By design; allow up to a max. of 5 generic parameters.", Scope = "type", Target = "~T:Cuemon.AspNetCore.Middleware`4")]
[assembly: SuppressMessage("Major Code Smell", "S2436:Types and methods should not have too many generic parameters", Justification = "By design; allow up to a max. of 5 generic parameters.", Scope = "type", Target = "~T:Cuemon.AspNetCore.Middleware`5")]
