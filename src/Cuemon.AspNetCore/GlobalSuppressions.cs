// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S3442:\"abstract\" classes should not have \"public\" constructors", Justification = "Infrastructure class.", Scope = "member", Target = "~M:Cuemon.AspNetCore.Infrastructure.ConfigurableMiddlewareCore`1.#ctor(Microsoft.AspNetCore.Http.RequestDelegate,System.Action{`0})")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S3442:\"abstract\" classes should not have \"public\" constructors", Justification = "Infrastructure class.", Scope = "member", Target = "~M:Cuemon.AspNetCore.Infrastructure.MiddlewareCore.#ctor(Microsoft.AspNetCore.Http.RequestDelegate)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S3442:\"abstract\" classes should not have \"public\" constructors", Justification = "Infrastructure class.", Scope = "member", Target = "~M:Cuemon.AspNetCore.Infrastructure.ConfigurableMiddlewareCore`1.#ctor(Microsoft.AspNetCore.Http.RequestDelegate,Microsoft.Extensions.Options.IOptions{`0})")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Critical Code Smell", "S927:parameter names should match base declaration and other partial definitions", Justification = "Generic parameter; implementation reflects usage.", Scope = "member", Target = "~M:Cuemon.AspNetCore.Hosting.HostingEnvironmentMiddleware.InvokeAsync(Microsoft.AspNetCore.Http.HttpContext,Microsoft.Extensions.Hosting.IHostEnvironment)~System.Threading.Tasks.Task")]
