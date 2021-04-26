---
uid: dotnet-md
title: Core API
---
# Core API

Core API, that is a part of Cuemon for .NET, is a toolbelt of assemblies designed to be intuitive and follow the same namespace declarations as we have been accustomed by [System](https://docs.microsoft.com/en-us/dotnet/api/system?view=net-5.0) which is included in the .NET platform by Microsoft.

With Cuemon for .NET you will get an assembly family providing both enhancements, new features and extension methods to these namespaces of [Microsoft .NET](https://docs.microsoft.com/en-us/dotnet/api/?view=net-5.0):

+ System
  + [Cuemon](#cuemon)
+ System.Collections
  + Cuemon.Collections
+ System.Collections.Generic
  + Cuemon.Collections.Generic
+ System.Collections.Specialized
  + Cuemon.Collections.Specialized
+ System.Configuration
  + Cuemon.Configuration
+ System.Data
  + [Cuemon.Data](#cuemondata)
  + [Cuemon.Data.Integrity](#cuemondataintegrity)
  + Cuemon.Data.Xml
+ System.Data.SqlClient
  + [Cuemon.Data.SqlClient](#cuemondatasqlclient)
+ System.Diagnostics
  + [Cuemon.Diagnostics](#cuemondiagnostics)
+ System.Globalization
  + Cuemon.Globalization
+ System.IO
  + [Cuemon.IO](#cuemonio)
+ System.Messaging
  + Cuemon.Messaging
+ System.Net
  + [Cuemon.Net](#cuemonnet)
  + Cuemon.Net.Collections.Specialized
+ System.Net.Httpp
  + Cuemon.Net.Httpp
+ System.Net.Mail
  + Cuemon.Net.Mail
+ System.Reflection
  + Cuemon.Reflection
+ System.Runtime
  + Cuemon.Runtime
+ System.Runtime.Caching
  + [Cuemon.Runtime.Caching](#cuemonruntimecaching)
+ System.Runtime.Serialization
  + Cuemon.Runtime.Serialization
+ System.Runtime.Serialization.Formatters
  + Cuemon.Runtime.Serialization.Formatters
+ System.Security
  + Cuemon.Security
+ System.Security.Cryptography
  + [Cuemon.Security.Cryptography](#cuemonsecuritycryptography)
+ System.Text
  + Cuemon.Text
+ System.Threading
  + [Cuemon.Threading](#cuemonthreading)
+ System.Xml
  + [Cuemon.Xml](#cuemonxml)
+ System.Xml.Linq
  + Cuemon.Xml.Linq
+ System.Xml.Serialization
  + Cuemon.Xml.Serialization
  + Cuemon.Xml.Serialization.Converters
  + Cuemon.Xml.Serialization.Formatters
+ System.Xml.XPath
  + Cuemon.Xml.XPath

Non-mappable namespace declarations:

+ [Cuemon.Resilience](#cuemonresilience)

## Cuemon

The Cuemon namespace contains fundamental types such as value and reference types, factories and utility classes, interfaces, attributes and feature rich delegates to support functional programming to a whole new level. The namespace is an addition to the System namespace.

Availability: **.NET Standard 2.0, .NET 5.0**

Assembly: Cuemon.Core

NuGet packages: [Focus](https://www.nuget.org/packages/Cuemon.Core) | [Productivity](https://www.nuget.org/packages/Cuemon.Core.App)

Github: [Development](https://github.com/gimlichael/Cuemon/tree/development/src/Cuemon.Core) | [Release](https://github.com/gimlichael/Cuemon/tree/release/src/Cuemon.Core) | [Master](https://github.com/gimlichael/Cuemon/tree/master/src/Cuemon.Core)

## Cuemon.Data

The Cuemon.Data namespace contains types that provide ways to connect, build and manipulate different data sources. The namespace is an addition to the System.Data namespace.

Availability: **.NET Standard 2.0, .NET 5.0**

Assembly: Cuemon.Data

NuGet packages: [Focus](https://www.nuget.org/packages/Cuemon.Data) | [Productivity](https://www.nuget.org/packages/Cuemon.Core.App)

Github: [Development](https://github.com/gimlichael/Cuemon/tree/development/src/Cuemon.Data) | [Release](https://github.com/gimlichael/Cuemon/tree/release/src/Cuemon.Data) | [Master](https://github.com/gimlichael/Cuemon/tree/master/src/Cuemon.Data)

## Cuemon.Data.Integrity

The Cuemon.Data.Integrity namespace contains types that provide ways for developers to determine and maintain integrity of data that is normally associated with an entity/resource.

Availability: **.NET Standard 2.0, .NET 5.0**

Assembly: Cuemon.Data.Integrity

NuGet packages: [Focus](https://www.nuget.org/packages/Cuemon.Data.Integrity) | [Productivity](https://www.nuget.org/packages/Cuemon.Core.App)

Github: [Development](https://github.com/gimlichael/Cuemon/tree/development/src/Cuemon.Data.Integrity) | [Release](https://github.com/gimlichael/Cuemon/tree/release/src/Cuemon.Data.Integrity) | [Master](https://github.com/gimlichael/Cuemon/tree/master/src/Cuemon.Data.Integrity)

## Cuemon.Data.SqlClient

The Cuemon.Data.SqlClient namespace contains types that provide ways for developers to work with Microsoft SQL Server integrations. The namespace is an addition to the System.Data.SqlClient namespace.

Availability: **.NET Standard 2.0, .NET 5.0**

Assembly: Cuemon.Data.SqlClient

NuGet packages: [Focus](https://www.nuget.org/packages/Cuemon.Data.SqlClient) | [Productivity](https://www.nuget.org/packages/Cuemon.Core.App)

Github: [Development](https://github.com/gimlichael/Cuemon/tree/development/src/Cuemon.Data.SqlClient) | [Release](https://github.com/gimlichael/Cuemon/tree/release/src/Cuemon.Data.SqlClient) | [Master](https://github.com/gimlichael/Cuemon/tree/master/src/Cuemon.Data.SqlClient)

## Cuemon.Diagnostics

The Cuemon.Diagnostics namespace contains types that provide ways for developers to describe exceptions including evidence to why an operation faulted. Also includes a flexible, generic and lambda friendly way to perform both synchronous and asynchronous time measuring operations. The namespace is an addition to the System.Diagnostics namespace.

Availability: **.NET Standard 2.0, .NET 5.0**

Assembly: Cuemon.Diagnostics

NuGet packages: [Focus](https://www.nuget.org/packages/Cuemon.Diagnostics) | [Productivity](https://www.nuget.org/packages/Cuemon.Core.App)

Github: [Development](https://github.com/gimlichael/Cuemon/tree/development/src/Cuemon.Diagnostics) | [Release](https://github.com/gimlichael/Cuemon/tree/release/src/Cuemon.Diagnostics) | [Master](https://github.com/gimlichael/Cuemon/tree/master/src/Cuemon.Diagnostics)

## Cuemon.IO

The Cuemon.IO namespace contains types primarily focusing on configuration options for IO related operations. The namespace is an addition to the System.IO namespace.

Availability: **.NET Standard 2.0, .NET Standard 2.1, .NET 5.0**

Assembly: Cuemon.IO

NuGet packages: [Focus](https://www.nuget.org/packages/Cuemon.IO) | [Productivity](https://www.nuget.org/packages/Cuemon.Core.App)

Github: [Development](https://github.com/gimlichael/Cuemon/tree/development/src/Cuemon.IO) | [Release](https://github.com/gimlichael/Cuemon/tree/release/src/Cuemon.IO) | [Master](https://github.com/gimlichael/Cuemon/tree/master/src/Cuemon.IO)

## Cuemon.Net

The Cuemon.Net namespace contains types that provides a simple programming interface for HTTP and SMTP protocols. The namespace is an addition to the System.Net namespace.

The Cuemon.IO namespace contains types primarily focusing on configuration options for IO related operations. The namespace is an addition to the System.IO namespace.

Availability: **.NET Standard 2.0, .NET 5.0**

Assembly: Cuemon.Net

NuGet packages: [Focus](https://www.nuget.org/packages/Cuemon.Net) | [Productivity](https://www.nuget.org/packages/Cuemon.Core.App)

Github: [Development](https://github.com/gimlichael/Cuemon/tree/development/src/Cuemon.Net) | [Release](https://github.com/gimlichael/Cuemon/tree/release/src/Cuemon.Net) | [Master](https://github.com/gimlichael/Cuemon/tree/master/src/Cuemon.Net)

## Cuemon.Resilience

The Cuemon.Resilience namespace contains types related to applying transient fault handling to existing code using intuitively named methods taking both Action{..} and Func{..} delegates to provide a lightweight resilience framework.

Availability: **.NET Standard 2.0, .NET 5.0**

Assembly: Cuemon.Resilience

NuGet packages: [Focus](https://www.nuget.org/packages/Cuemon.Resilience) | [Productivity](https://www.nuget.org/packages/Cuemon.Core.App)

Github: [Development](https://github.com/gimlichael/Cuemon/tree/development/src/Cuemon.Resilience) | [Release](https://github.com/gimlichael/Cuemon/tree/release/src/Cuemon.Resilience) | [Master](https://github.com/gimlichael/Cuemon/tree/master/src/Cuemon.Resilience)

## Cuemon.Runtime.Caching

The Cuemon.Runtime.Caching namespace contains types related to interfaces for generic caching in applications while providing a concrete in-memory cache implementation named SlimMemoryCache. The namespace is an addition to the System.Runtime.Caching namespace.

Availability: **.NET Standard 2.0, .NET 5.0**

Assembly: Cuemon.Runtime.Caching

NuGet packages: [Focus](https://www.nuget.org/packages/Cuemon.Runtime.Caching) | [Productivity](https://www.nuget.org/packages/Cuemon.Core.App)

Github: [Development](https://github.com/gimlichael/Cuemon/tree/development/src/Cuemon.Runtime.Caching) | [Release](https://github.com/gimlichael/Cuemon/tree/release/src/Cuemon.Runtime.Caching) | [Master](https://github.com/gimlichael/Cuemon/tree/master/src/Cuemon.Runtime.Caching)

## Cuemon.Security.Cryptography

The Cuemon.Security.Cryptography namespace contains types related to cryptographic hashing (both keyed and non-keyed) and a ready-to-use implementation of the Advanced Encryption Standard (AES) symmetric algorithm. The namespace is an addition to the System.Security.Cryptography namespace.

Availability: **.NET Standard 2.0, .NET 5.0**

Assembly: Cuemon.Security.Cryptography

NuGet packages: [Focus](https://www.nuget.org/packages/Cuemon.Security.Cryptography) | [Productivity](https://www.nuget.org/packages/Cuemon.Core.App)

Github: [Development](https://github.com/gimlichael/Cuemon/tree/development/src/Cuemon.Security.Cryptography) | [Release](https://github.com/gimlichael/Cuemon/tree/release/src/Cuemon.Security.Cryptography) | [Master](https://github.com/gimlichael/Cuemon/tree/master/src/Cuemon.Security.Cryptography)

## Cuemon.Threading

The Cuemon.Threading namespace contains types related to working with long-running concurrent loops and regions that utilizes both synchronous and asynchronous delegates. The namespace is an addition to the System.Threading namespace.

Availability: **.NET Standard 2.0, .NET 5.0**

Assembly: Cuemon.Threading

NuGet packages: [Focus](https://www.nuget.org/packages/Cuemon.Threading) | [Productivity](https://www.nuget.org/packages/Cuemon.Core.App)

Github: [Development](https://github.com/gimlichael/Cuemon/tree/development/src/Cuemon.Threading) | [Release](https://github.com/gimlichael/Cuemon/tree/release/src/Cuemon.Threading) | [Master](https://github.com/gimlichael/Cuemon/tree/master/src/Cuemon.Threading)

## Cuemon.Xml

The Cuemon.Xml namespace contains types related to encoding, converting and serialization. The included lightweight XML serializer framework offers same flexibility as the one provided by the JSON equivalent from Newtonsoft. The namespace is an addition to the System.Xml namespace.

Availability: **.NET Standard 2.0, .NET 5.0**

Assembly: Cuemon.Xml

NuGet packages: [Focus](https://www.nuget.org/packages/Cuemon.Xml) | [Productivity](https://www.nuget.org/packages/Cuemon.Core.App)

Github: [Development](https://github.com/gimlichael/Cuemon/tree/development/src/Cuemon.Xml) | [Release](https://github.com/gimlichael/Cuemon/tree/release/src/Cuemon.Xml) | [Master](https://github.com/gimlichael/Cuemon/tree/master/src/Cuemon.Xml)