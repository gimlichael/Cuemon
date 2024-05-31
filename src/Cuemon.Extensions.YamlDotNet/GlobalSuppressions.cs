// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Major Code Smell", "S3011:Reflection should not be used to increase accessibility of classes, methods, or fields", Justification = "By design.", Scope = "member", Target = "~M:Cuemon.Extensions.YamlDotNet.Formatters.YamlFormatter.Serialize(System.Object,System.Type)~System.IO.Stream")]
[assembly: SuppressMessage("Major Code Smell", "S3011:Reflection should not be used to increase accessibility of classes, methods, or fields", Justification = "By design.", Scope = "member", Target = "~M:Cuemon.Extensions.YamlDotNet.Formatters.YamlFormatter.Deserialize(System.IO.Stream,System.Type)~System.Object")]
[assembly: SuppressMessage("Major Code Smell", "S3011:Reflection should not be used to increase accessibility of classes, methods, or fields", Justification = "By design.", Scope = "member", Target = "~M:Cuemon.Extensions.YamlDotNet.Formatters.YamlFormatter.UseBuilder~YamlDotNet.Serialization.SerializerBuilder")]
