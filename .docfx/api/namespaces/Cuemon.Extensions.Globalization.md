---
uid: Cuemon.Extensions.Globalization
summary: *content
---
The `Cuemon.Extensions.Globalization`  namespace contains extension methods that is an addition to the `System.Globalization` namespace.

[!INCLUDE [availability-default](../../includes/availability-default.md)]

Complements: [System.Globalization namespace](https://docs.microsoft.com/en-us/dotnet/api/system.globalization) 🔗

### Extension Methods

|Type|Ext|Methods|
|--:|:-:|---|
|CultureInfo|⬇️|`UseNationalLanguageSupport`|

### CSharp Example

```csharp
var danishCultureIcu = new CultureInfo("da-dk", false);
var danishCultureNls = new CultureInfo("da-dk", false).UseNationalLanguageSupport();

// danishCultureIcu outputs dd.MM.yyyy from danishCultureIcu.DateTimeFormat.ShortDatePattern
// danishCultureNls outputs dd-MM-yyyy from danishCultureNls.DateTimeFormat.ShortDatePattern
```
