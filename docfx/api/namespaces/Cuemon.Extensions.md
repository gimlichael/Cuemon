---
uid: Cuemon.Extensions
summary: *content
---
The Cuemon.Extensions namespace contains extension methods that complements the Cuemon namespace while being an addition to the System namespace.

Availability: NET Standard 2.0

Complements: [Cuemon namespace](https://docs.cuemon.net/api/dotnet/Cuemon.html) 🔗

Github branches 🌱\
[development](https://github.com/gimlichael/Cuemon/tree/development/src/Cuemon.Extensions.Core)\
[release](https://github.com/gimlichael/Cuemon/tree/release/src/Cuemon.Extensions.Core)\
[master](https://github.com/gimlichael/Cuemon/tree/master/src/Cuemon.Extensions.Core)

NuGet packages 📦\
[Cuemon.Extensions.Core (CI)](https://nuget.cuemon.net/packages/Cuemon.Extensions.Core)\
[Cuemon.Extensions.Core (Stable and Preview)](https://www.nuget.org/packages/Cuemon.Extensions.Core)

### Extension Methods

|Type|Ext|Methods|
|--:|:-:|---|
|Action|⬇️|`Configure{TOptions}`, `CreateInstance{T}`|
|Byte|⬇️|`ToEncodedString`, `ToHexadecimalString`, `ToBinaryString`, `ToUrlEncodedBase64String`, `ToBase64String`, `TryDetectUnicodeEncoding`|
|Char|⬇️|`ToEnumerable`, `FromChars`|
|Condition|⬇️|`HasDifference`|
|DateTime|⬇️|`ToUnixEpochTime`, `ToUtcKind`, `ToLocalKind`, `ToDefaultKind`, `IsWithinRange`, `IsTimeOfDayNight`, `IsTimeOfDayMorning`, `IsTimeOfDayForenoon`, `IsTimeOfDayAfternoon`, `IsTimeOfDayEvening`, `Floor`, `Ceiling`, `Round`|
|Double|⬇️|`FromUnixEpochTime`, `ToTimeSpan`, `Factorial`, `RoundOff`|
|Exception|⬇️|`Flatten`|
|Int*|⬇️|`Min`, `Max`, `IsPrime`, `IsCountableSequence`, `IsEven`, `IsOdd`|
|Mapping|⬇️|`Add`|
|Object|⬇️|`UseWrapper{T}`, `As{T}`, `GetHashCode32{T}`, `GetHashCode64{T}`, `ToDelimitedString{T}`, `Adjust{T}`, `IsNullable{T}`|
|String|⬇️|`Difference`, `ToCharArray`, `ToByteArray`, `FromUrlEncodedBase64`, `ToGuid`, `FromBinaryDigits`, `FromBase64`, `Join`, `ToCasing`, `ToUri`, `IsNullOrEmpty`, `IsNullOrWhiteSpace`, `IsEmailAddress`, `IsGuid`, `IsHex`, `IsNumeric`, `IsBase64`, `IsCountableSequence`, `SplitDelimited`, `Count`, `RemoveAll`, `ReplaceAll`, `JsEscape`, `JsUnescape`, `ContainsAny`, `ContainsAll`, `EqualsAny`, `StartsWith`, `TrimAll`, `IsSequenceOf{T}`, `FromHexadecimal`, `ToHexadecimal`, `ToEnum{TEnum}`, `ToTimeSpan`, `SubstringBefore`, `Chunk`, `SuffixWith`, `SuffixWithForwardingSlash`, `PrefixWith`|
|TimeSpan|⬇️|`GetTotalNanoseconds`, `GetTotalMicroseconds`, `Floor`, `Ceiling`, `Round`|
|Type|⬇️|`ToFriendlyName`, `ToTypeCode`, `HasEqualityComparerImplementation`, `HasComparableImplementation`, `HasComparerImplementation`, `HasEnumerableImplementation`, `HasDictionaryImplementation`, `HasKeyValuePairImplementation`, `IsNullable`, `HasAnonymousCharacteristics`, `IsComplex`, `IsSimple`, `GetDefaultValue`, `HasTypes`, `HasInterfaces`, `HasAttributes`|
|Validator|⬇️|`HasDifference`, `NoDifference`|