---
uid: Cuemon.Extensions.Net.Security
summary: *content
---
The `Cuemon.Extensions.Net.Security` namespace contains extension methods that provides a generic way to make a Uniform Resource Identifier signed and tampering protected. This could be used to make your own lightweight concept of a Azure shared access signatures (SAS). Originally part of Cuemon .NET Framework: https://github.com/gimlichael/CuemonNetFramework/blob/master/Cuemon.Web/Security/WebSecurityUtility.cs. Greatly simplified anno 2020.

[!INCLUDE [availability-default](../../includes/availability-default.md)]

### Extension Methods

|Type|Ext|Methods|
|--:|:-:|---|
|String|⬇️|`ToSignedUri`, `ValidateSignedUri`|
|Uri|⬇️|`ToSignedUri`, `ValidateSignedUri`|
