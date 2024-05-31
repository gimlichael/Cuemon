---
uid: Cuemon.Extensions.Xml
summary: *content
---
The `Cuemon.Extensions.Xml` namespace contains extension methods that complements the `Cuemon.Xml` namespace while being an addition to the `System.Xml` namespace.

[!INCLUDE [availability-default](../../includes/availability-default.md)]

Complements: [Cuemon.Xml namespace](/api/dotnet/Cuemon.Xml.html) 📘

### Extension Methods

|Type|Ext|Methods|
|--:|:-:|---|
|byte[]|⬇️|`ToXmlReader`|
|DateTime|⬇️|`ToString`|
|IHierarchy{T}|⬇️|`HasXmlIgnoreAttribute`, `IsNodeEnumerable`, `GetXmlQualifiedEntity`, `OrderByXmlAttributes`|
|Stream|⬇️|`ToXmlReader`, `CopyXmlStream`, `TryDetectXmlEncoding`, `RemoveXmlNamespaceDeclarations`|
|String|⬇️|`EscapeXml`, `UnescapeXml`, `SanitizeXmlElementName`, `SanitizeXmlElementText`|
|Uri|⬇️|`ToXmlReader`|
|XmlReader|⬇️|`Chunk`, `ToHierarchy`, `ToStream`, `MoveToFirstElement`|
|XmlWriter|⬇️|`WriteObject`, `WriteObject{T}`, `WriteStartElement`, `WriteEncapsulatingElementWhenNotNull{T}`, `WriteXmlRootElement{T}`|
