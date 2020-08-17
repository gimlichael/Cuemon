// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Major Code Smell", "S1854:Unused assignments should be removed", Justification = "False-positive; inside try block.", Scope = "member", Target = "~M:Cuemon.Extensions.Resilience.TransientOperation.TryWithFuncCore``3(Cuemon.TesterFuncFactory{``0,``2,``1},``2@,System.Action{Cuemon.Extensions.Resilience.TransientOperationOptions})~``1")]
[assembly: SuppressMessage("Major Code Smell", "S1854:Unused assignments should be removed", Justification = "False-positive; inside try block.", Scope = "member", Target = "~M:Cuemon.Extensions.Resilience.TransientOperation.WithActionCore``1(Cuemon.ActionFactory{``0},System.Action{Cuemon.Extensions.Resilience.TransientOperationOptions})")]
[assembly: SuppressMessage("Major Code Smell", "S1854:Unused assignments should be removed", Justification = "False-positive; inside try block.", Scope = "member", Target = "~M:Cuemon.Extensions.Resilience.TransientOperation.WithFuncCore``2(Cuemon.FuncFactory{``0,``1},System.Action{Cuemon.Extensions.Resilience.TransientOperationOptions})~``1")]
[assembly: SuppressMessage("Major Code Smell", "S1854:Unused assignments should be removed", Justification = "False-positive; inside try block.", Scope = "member", Target = "~M:Cuemon.Extensions.Resilience.TransientOperation.WithActionAsyncCore``1(Cuemon.TaskActionFactory{``0},System.Action{Cuemon.Extensions.Resilience.TransientOperationOptions},System.Threading.CancellationToken)~System.Threading.Tasks.Task")]
[assembly: SuppressMessage("Major Code Smell", "S1854:Unused assignments should be removed", Justification = "False-positive; inside try block.", Scope = "member", Target = "~M:Cuemon.Extensions.Resilience.TransientOperation.WithFuncAsyncCore``2(Cuemon.TaskFuncFactory{``0,``1},System.Action{Cuemon.Extensions.Resilience.TransientOperationOptions},System.Threading.CancellationToken)~System.Threading.Tasks.Task{``1}")]
