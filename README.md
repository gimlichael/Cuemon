![Cuemon](https://nblcdn.net/themes/cuemon.net/img/core/128x128x.png)

Cuemon
--------------------
Cuemon is a free and flexible assembly package for the Microsoft .NET ecosystem. It was built to extend and boost your codebelt - providing vast ways of possibilities for all breeds of coders, programmers, developers and the likes thereof. Ideal for .NET, .NET Standard, .NET Core, Universal Windows Platform and .NET Framework 4.6.1 and newer.

![License](https://img.shields.io/github/license/gimlichael/cuemon)

This development branch contains the latest version which has been completely refactored and updated to suport .NET Core 3.1.
All CI and CD will be runned on Azure DevOps and is currently in process of being tweaked.

Once fully automated and tested thoroughly, it will be pushed to a new branch, release, and hereafter again tested and lastly to master and Nuget packages.

Another big change for this upcoming release is the versioning; the world has spoken - and chosen semantic versioning.

The release for now is planned to be 6.0.0.

[![Build Status](https://dev.azure.com/gimlichael/Cuemon/_apis/build/status/gimlichael.Cuemon?branchName=development)](https://dev.azure.com/gimlichael/Cuemon/_build/latest?definitionId=9&branchName=development)

[![codecov](https://codecov.io/gh/gimlichael/Cuemon/branch/development/graph/badge.svg)](https://codecov.io/gh/gimlichael/Cuemon)

[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=Cuemon&metric=coverage)](https://sonarcloud.io/dashboard?id=Cuemon)

[![Quality gate](https://sonarcloud.io/api/project_badges/quality_gate?project=Cuemon)](https://sonarcloud.io/dashboard?id=Cuemon)

[![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=Cuemon&metric=sqale_rating)](https://sonarcloud.io/dashboard?id=Cuemon)

[![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=Cuemon&metric=reliability_rating)](https://sonarcloud.io/dashboard?id=Cuemon)

[![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=Cuemon&metric=security_rating)](https://sonarcloud.io/dashboard?id=Cuemon)

[![Lines of Code](https://sonarcloud.io/api/project_badges/measure?project=Cuemon&metric=ncloc)](https://sonarcloud.io/dashboard?id=Cuemon)

[![Code Smells](https://sonarcloud.io/api/project_badges/measure?project=Cuemon&metric=code_smells)](https://sonarcloud.io/dashboard?id=Cuemon)

[![Technical Debt](https://sonarcloud.io/api/project_badges/measure?project=Cuemon&metric=sqale_index)](https://sonarcloud.io/dashboard?id=Cuemon)

[![Bugs](https://sonarcloud.io/api/project_badges/measure?project=Cuemon&metric=bugs)](https://sonarcloud.io/dashboard?id=Cuemon)

[![Vulnerabilities](https://sonarcloud.io/api/project_badges/measure?project=Cuemon&metric=vulnerabilities)](https://sonarcloud.io/dashboard?id=Cuemon)

Want to try out the new and improved Cuemon?

To consume a CI build, create a `NuGet.Config` in your root solution directory and add following content:

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <packageSources>
    <clear />
    <!-- Cuemon CI build feed -->
    <add key="cuemon" value="https://nuget.cuemon.net/v3/index.json" /> 
    <!-- Defaul nuget feed -->
    <add key="nuget" value="https://api.nuget.org/v3/index.json" /> 
  </packageSources>
</configuration>
```

Stay tuned!

Useful links for this project (will soon be changed for the forthcoming release):

* [Cuemon .NET Standard](http://www.cuemon.net/)
* Cuemon .NET Standard Package on [Nuget](https://www.nuget.org/packages/Cuemon.Core.Package/)
* Cuemon ASP.NET Core Package on [Nuget](https://www.nuget.org/packages/Cuemon.AspNetCore.Package/)
* Cuemon.AspNetCore on [Nuget](https://www.nuget.org/packages/Cuemon.AspNetCore/)
* Cuemon.AspNetCore.Authentication on [Nuget](https://www.nuget.org/packages/Cuemon.AspNetCore.Authentication/)
* Cuemon.AspNetCore.Mvc on [Nuget](https://www.nuget.org/packages/Cuemon.AspNetCore.Mvc/)
* Cuemon.AspNetCore.Mvc.Formatters.Json on [Nuget](https://www.nuget.org/packages/Cuemon.AspNetCore.Mvc.Formatters.Json/)
* Cuemon.AspNetCore.Mvc.Formatters.Xml on [Nuget](https://www.nuget.org/packages/Cuemon.AspNetCore.Mvc.Formatters.Xml/)
* Cuemon.Collections.Specialized on [Nuget](https://www.nuget.org/packages/Cuemon.Collections.Specialized/)
* Cuemon.Core on [Nuget](https://www.nuget.org/packages/Cuemon.Core/)
* Cuemon.Data on [Nuget](https://www.nuget.org/packages/Cuemon.Data/)
* Cuemon.Data.XmlClient on [Nuget](https://www.nuget.org/packages/Cuemon.Data.XmlClient/)
* Cuemon.Integrity on [Nuget](https://www.nuget.org/packages/Cuemon.Integrity/)
* Cuemon.IO on [Nuget](https://www.nuget.org/packages/Cuemon.IO/)
* Cuemon.Net on [Nuget](https://www.nuget.org/packages/Cuemon.Net/)
* Cuemon.Net.Mail on [Nuget](https://www.nuget.org/packages/Cuemon.Net.Mail/)
* Cuemon.Reflection on [Nuget](https://www.nuget.org/packages/Cuemon.Reflection/)
* Cuemon.Runtime on [Nuget](https://www.nuget.org/packages/Cuemon.Runtime/)
* Cuemon.Runtime.Caching on [Nuget](https://www.nuget.org/packages/Cuemon.Runtime.Caching/)
* Cuemon.Security on [Nuget](https://www.nuget.org/packages/Cuemon.Security/)
* Cuemon.Serialization on [Nuget](https://www.nuget.org/packages/Cuemon.Serialization/)
* Cuemon.Serialization.Json on [Nuget](https://www.nuget.org/packages/Cuemon.Serialization.Json/)
* Cuemon.Serialization.Xml on [Nuget](https://www.nuget.org/packages/Cuemon.Serialization.Xml/)
* Cuemon.Threading on [Nuget](https://www.nuget.org/packages/Cuemon.Threading/)
* Cuemon.Web on [Nuget](https://www.nuget.org/packages/Cuemon.Web/)
* Cuemon.Xml on [Nuget](https://www.nuget.org/packages/Cuemon.Xml/)

* My profile on [LinkedIn](http://dk.linkedin.com/in/gimlichael)
* My profile on [Twitter](https://twitter.com/gimlichael)
* My profile on [StackOverflow](http://stackoverflow.com/users/175073/michael-mortensen)
* My [blog](http://www.cuemon.net/blog/)
