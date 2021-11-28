---
uid: Cuemon.Extensions.Xml
summary: *content
---
The Cuemon.Extensions.Xml namespace contains extension methods that complements the Cuemon.Xml namespace while being an addition to the System.Xml namespace.

Availability: NET Standard 2.0, .NET 5.0, .NET 6.0

Complements: [Cuemon.Xml namespace](https://docs.cuemon.net/api/dotnet/Cuemon.Xml.html) 🔗

## Github branches 🖇️

[development](https://github.com/gimlichael/Cuemon/tree/development/src/Cuemon.Extensions.Xml) 🧪\
[release](https://github.com/gimlichael/Cuemon/tree/release/src/Cuemon.Extensions.Xml) 🎬\
[master](https://github.com/gimlichael/Cuemon/tree/master/src/Cuemon.Extensions.Xml) 🛡️

## NuGet packages

📦 Focus Pack\
[Cuemon.Extensions.Xml (CI)](https://nuget.cuemon.net/packages/Cuemon.Extensions.Xml)\
[Cuemon.Extensions.Xml (Stable and Preview)](https://www.nuget.org/packages/Cuemon.Extensions.Xml)\
![NuGet Version](https://img.shields.io/nuget/v/Cuemon.Extensions.Xml?logo=nuget) ![NuGet Preview Version](https://img.shields.io/nuget/vpre/Cuemon.Extensions.Xml?logo=nuget) ![NuGet Downloads](https://img.shields.io/nuget/dt/Cuemon.Extensions.Xml?color=blueviolet&logo=nuget)
\
\
🏭 Productivity Pack\
[Cuemon.Core.App (CI)](https://nuget.cuemon.net/packages/Cuemon.Core.App)\
[Cuemon.Core.App (Stable and Preview)](https://www.nuget.org/packages/Cuemon.Core.App)\
![NuGet Version](https://img.shields.io/nuget/v/Cuemon.Core.App?logo=nuget) ![NuGet Preview Version](https://img.shields.io/nuget/vpre/Cuemon.Core.App?logo=nuget) ![NuGet Downloads](https://img.shields.io/nuget/dt/Cuemon.Core.App?color=blueviolet&logo=nuget)

### Extension Methods

|Type|Ext|Methods|
|--:|:-:|---|
|byte[]|⬇️|`ToXmlReader`|
|DateTime|⬇️|`ToString`|
|IHierarchy{T}|⬇️|`HasXmlIgnoreAttribute`, `IsNodeEnumerable`, `GetXmlRootOrElement`, `OrderByXmlAttributes`|
|Stream|⬇️|`ToXmlReader`, `CopyXmlStream`, `TryDetectXmlEncoding`, `RemoveXmlNamespaceDeclarations`|
|String|⬇️|`EscapeXml`, `UnescapeXml`, `SanitizeXmlElementName`, `SanitizeXmlElementText`|
|Uri|⬇️|`ToXmlReader`|
|XmlReader|⬇️|`Chunk`, `ToHierarchy`, `ToStream`, `MoveToFirstElement`|
|XmlWriter|⬇️|`WriteObject`, `WriteObject{T}`, `WriteStartElement`, `WriteEncapsulatingElementWhenNotNull{T}`, `WriteXmlRootElement{T}`|