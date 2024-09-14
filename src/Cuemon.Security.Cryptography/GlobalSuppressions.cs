// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Reliability", "CA2022:Avoid inexact read with 'Stream.Read'", Justification = "Worked since implementation; only method compatible with netstandard2.0.", Scope = "member", Target = "~M:Cuemon.Security.Cryptography.AesCryptor.CryptoTransformCore(System.Byte[],Cuemon.Security.Cryptography.AesCryptor.AesMode,System.Action{Cuemon.Security.Cryptography.AesCryptorOptions})~System.Byte[]")]
