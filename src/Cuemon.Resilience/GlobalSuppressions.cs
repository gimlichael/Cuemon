// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Major Code Smell", "S1854:Unused assignments should be removed", Justification = "False-positive; inside try block.", Scope = "member", Target = "~M:Cuemon.Resilience.TransientOperation.WithActionCore``1(Cuemon.ActionFactory{``0},System.Action{Cuemon.Resilience.TransientOperationOptions})")]
[assembly: SuppressMessage("Major Code Smell", "S1854:Unused assignments should be removed", Justification = "False-positive; inside try block.", Scope = "member", Target = "~M:Cuemon.Resilience.TransientOperation.WithFuncCore``2(Cuemon.FuncFactory{``0,``1},System.Action{Cuemon.Resilience.TransientOperationOptions})~``1")]
[assembly: SuppressMessage("Major Code Smell", "S1854:Unused assignments should be removed", Justification = "False-positive; inside try block.", Scope = "member", Target = "~M:Cuemon.Resilience.TransientOperation.WithActionAsyncCore``1(Cuemon.TaskActionFactory{``0},System.Action{Cuemon.Resilience.TransientOperationOptions},System.Threading.CancellationToken)~System.Threading.Tasks.Task")]
[assembly: SuppressMessage("Major Code Smell", "S1854:Unused assignments should be removed", Justification = "False-positive; inside try block.", Scope = "member", Target = "~M:Cuemon.Resilience.TransientOperation.WithFuncAsyncCore``2(Cuemon.TaskFuncFactory{``0,``1},System.Action{Cuemon.Resilience.TransientOperationOptions},System.Threading.CancellationToken)~System.Threading.Tasks.Task{``1}")]
[assembly: SuppressMessage("Major Code Smell", "S1854:Unused assignments should be removed", Justification = "False-positive.", Scope = "member", Target = "~M:Cuemon.Resilience.ActionTransientWorker.ResilientAction(System.Action)")]
[assembly: SuppressMessage("Major Code Smell", "S1854:Unused assignments should be removed", Justification = "False-positive.", Scope = "member", Target = "~M:Cuemon.Resilience.AsyncActionTransientWorker.ResilientActionAsync(System.Func{System.Threading.CancellationToken,System.Threading.Tasks.Task},System.Threading.CancellationToken)~System.Threading.Tasks.Task")]
[assembly: SuppressMessage("Major Code Smell", "S1854:Unused assignments should be removed", Justification = "False-positive.", Scope = "member", Target = "~M:Cuemon.Resilience.AsyncFuncTransientWorker`1.ResilientFuncAsync(System.Func{System.Threading.CancellationToken,System.Threading.Tasks.Task{`0}},System.Threading.CancellationToken)~System.Threading.Tasks.Task{`0}")]
[assembly: SuppressMessage("Major Code Smell", "S1854:Unused assignments should be removed", Justification = "False-positive.", Scope = "member", Target = "~M:Cuemon.Resilience.FuncTransientWorker`1.ResilientFunc(System.Func{`0})~`0")]
[assembly: SuppressMessage("Major Code Smell", "S3925:\"ISerializable\" should be implemented correctly", Justification = "MethodBase and Type are not serializable; hence the workaround.", Scope = "type", Target = "~T:Cuemon.Resilience.TransientFaultEvidence")]
