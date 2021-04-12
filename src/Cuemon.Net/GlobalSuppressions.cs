// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Major Code Smell", "S3925:\"ISerializable\" should be implemented correctly", Justification = "PlatformNotSupportedException", Scope = "type", Target = "~T:Cuemon.Net.QueryStringCollection")]
[assembly: SuppressMessage("Critical Code Smell", "S3776:Cognitive Complexity of methods should not be too high", Justification = "https://github.com/mono/mono/blob/da11592cbea4269971f4b1f9624769a85cc10660/mcs/class/System.Web/System.Web/HttpUtility.cs", Scope = "member", Target = "~M:Cuemon.Net.StringDecoratorExtensions.UrlDecode(Cuemon.IDecorator{System.String},System.Action{Cuemon.Text.EncodingOptions})~System.String")]
[assembly: SuppressMessage("Usage", "CA2249:Consider using 'string.Contains' instead of 'string.IndexOf'", Justification = "False-Positive. Contains does not support char.", Scope = "member", Target = "~M:Cuemon.Net.StringDecoratorExtensions.UrlDecode(Cuemon.IDecorator{System.String},System.Action{Cuemon.Text.EncodingOptions})~System.String")]
[assembly: SuppressMessage("Major Bug", "S2259:Null pointers should not be dereferenced", Justification = "False-Positive; if hasUtcLastModified is false, hasEntityTag has to be true - otherwise an exception is thrown.", Scope = "member", Target = "~M:Cuemon.Net.Http.HttpWatcher.FetchUsingHttpHeadAsync(Cuemon.Net.Http.HttpManager)~System.Threading.Tasks.Task")]
