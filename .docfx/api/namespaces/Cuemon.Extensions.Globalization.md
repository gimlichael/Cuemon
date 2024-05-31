---
uid: Cuemon.Extensions.Globalization
summary: *content
---
The `Cuemon.Extensions.Globalization` namespace contains extension methods that complements the `Cuemon.Globalization` namespace while being an addition to the `System.Globalization` namespace.

[!INCLUDE [availability-default](../../includes/availability-default.md)]

Complements: [Cuemon.Globalization namespace](/api/dotnet/Cuemon.Globalization.html) 📘

### Extension Methods

|Type|Ext|Methods|
|--:|:-:|---|
|CultureInfo|⬇️|`UseNationalLanguageSupport`|

### CSharp Example
```csharp
var danishCultureIcu = CultureInfo("da-dk");
var danishCultureNls = new CultureInfo("da-dk").UseNationalLanguageSupport();

// danishCultureIcu outputs dd.MM.yyyy from danishCultureIcu.DateTimeFormat.ShortDatePattern
// danishCultureNls outputs dd-MM-yyyy from danishCultureNls.DateTimeFormat.ShortDatePattern
```