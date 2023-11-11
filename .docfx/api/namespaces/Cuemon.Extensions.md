﻿---
uid: Cuemon.Extensions
summary: *content
---
The `Cuemon.Extensions` namespace contains extension methods that complements the `Cuemon` namespace while being an addition to the `System` namespace.

[!INCLUDE [availability-default](../../includes/availability-default.md)]

Complements: [Cuemon namespace](/api/dotnet/Cuemon.html) 📘

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
|Mapping|⬇️|`AddMapping`|
|MethodDescriptor|⬇️|`HasParameters`|
|Object|⬇️|`UseWrapper{T}`, `As{T}`, `As`, `GetHashCode32{T}`, `GetHashCode64{T}`, `ToDelimitedString{T}`, `Adjust{T}`, `Alter{T}`, `IsNullable{T}`|
|String|⬇️|`ReplaceLineEndings`, `Difference`, `ToByteArray`, `FromUrlEncodedBase64`, `ToGuid`, `FromBinaryDigits`, `FromBase64`, `ToCasing`, `ToUri`, `IsNullOrEmpty`, `IsNullOrWhiteSpace`, `IsEmailAddress`, `IsGuid`, `IsHex`, `IsNumeric`, `IsBase64`, `SplitDelimited`, `Count`, `RemoveAll`, `ReplaceAll`, `JsEscape`, `JsUnescape`, `ContainsAny`, `ContainsAll`, `EqualsAny`, `StartsWith`, `TrimAll`, `IsSequenceOf{T}`, `FromHexadecimal`, `ToHexadecimal`, `ToEnum{TEnum}`, `ToTimeSpan`, `SubstringBefore`, `Chunk`, `SuffixWith`, `SuffixWithForwardingSlash`, `PrefixWith`|
|TimeSpan|⬇️|`GetTotalNanoseconds`, `GetTotalMicroseconds`, `Floor`, `Ceiling`, `Round`|
|Type|⬇️|`ToFriendlyName`, `ToTypeCode`, `HasEqualityComparerImplementation`, `HasComparableImplementation`, `HasComparerImplementation`, `HasEnumerableImplementation`, `HasDictionaryImplementation`, `HasKeyValuePairImplementation`, `IsNullable`, `HasAnonymousCharacteristics`, `IsComplex`, `IsSimple`, `GetDefaultValue`, `HasTypes`, `HasInterfaces`, `HasAttributes`|
|Validator|⬇️|`ContainsReservedKeyword`, `HasDifference`, `NoDifference`|
