---
uid: Cuemon.Extensions.Xml
summary: *content
---
The Cuemon.Extensions.Xml namespace contains extension methods that complements the Cuemon.Xml namespace while being an addition to the System.Xml namespace.

Availability: NET Standard 2.0

Related: [Cuemon.Xml namespace](https://docs.cuemon.net/api/dotnet/Cuemon.Xml.html) 📘

Github branches 🌱\
[development](https://github.com/gimlichael/Cuemon/tree/development/src/Cuemon.Extensions.Xml)\
[release](https://github.com/gimlichael/Cuemon/tree/release/src/Cuemon.Extensions.Xml)\
[master](https://github.com/gimlichael/Cuemon/tree/master/src/Cuemon.Extensions.Xml)

NuGet packages 📦\
[Cuemon.Extensions.Xml (CI)](https://nuget.cuemon.net/packages/Cuemon.Extensions.Xml)\
[Cuemon.Extensions.Xml (Stable and Preview)](https://www.nuget.org/packages/Cuemon.Extensions.Xml)

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