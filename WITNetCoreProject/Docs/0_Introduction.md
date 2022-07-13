# Blueprint Backend .NET Core 3.1

## Introduction

The main purpose of this repository is to make backend developer team is easier to initiate developing the project, inside this repository contain 2 projects: Main Blueprint, and Unit Test Blueprint. This structure is easy to read and more friendly for changes as long as you have knowledge or experience especially in .NET Core technology.

## Getting Started

Before you can use this blueprint, you need to follow a few steps such as install IDE and some depedencies on your system.

### IDE Setup

#### VSCode

#### Visual Studio 2019 / Visual Studio Mac

- VS Code . see [VS Code](https://code.visualstudio.com/)

- Visual Studio . see [Visual Studio setup](https://visualstudio.microsoft.com/vs/)

- dotnet windows powershell

```

   dotnet-install.ps1 [-Architecture <ARCHITECTURE>] [-AzureFeed]

   [-Channel <CHANNEL>] [-DryRun] [-FeedCredential]

   [-InstallDir <DIRECTORY>] [-JSonFile <JSONFILE>]

   [-NoCdn] [-NoPath] [-ProxyAddress] [-ProxyBypassList <LIST_OF_URLS>]

   [-ProxyUseDefaultCredentials] [-Quality <QUALITY>] [-Runtime <RUNTIME>]

   [-SkipNonVersionedFiles] [-UncachedFeed] [-Verbose]

   [-Version <VERSION>]


   Get-Help ./dotnet-install.ps1

```

- dotnet Linux/MacOS

```

    dotnet-install.sh  [--architecture <ARCHITECTURE>] [--azure-feed]

    [--channel <CHANNEL>] [--dry-run] [--feed-credential]

    [--install-dir <DIRECTORY>] [--jsonfile <JSONFILE>]

    [--no-cdn] [--no-path] [--quality <QUALITY>]

    [--runtime <RUNTIME>] [--runtime-id <RID>]

    [--skip-non-versioned-files] [--uncached-feed] [--verbose]

    [--version <VERSION>]


    dotnet-install.sh --help

```

- Check whether dotnet is installed on your system

```
dotnet  --version

```

#### Depedencies

##### Dependencies on Main Blueprint

- AutoMapper

AutoMapper is a simple little library built to solve a deceptively complex problem - getting rid of code that mapped one object to another.

```
Install-Package AutoMapper -Version 11.0.1

```

- Fluent Validation

FluentValidation is validation library for .NET that uses a fluent interface and lambda expressions for building strongly-typed validation rules.

```
Install-Package FluentValidation.AspNetCore -Version 11.0.3

```

- Jwt Bearer

ASP.NET Core middleware that enables an application to receive an OpenID Connect bearer token.

```
Install-Package Microsoft.AspNetCore.Authentication.JwtBearer -Version 7.0.0-preview.4.22251.1

```

- NewtonSoft Json

Json.NET is a popular high-performance JSON framework for .NET

```
Install-Package Newtonsoft.Json -Version 13.0.2-beta1

```

- Microsoft Entity Framework

Entity Framework Core is a modern object-database mapper for .NET. It supports LINQ queries, change tracking, updates, and schema migrations. EF Core works with SQL Server, Azure SQL Database, SQLite, Azure Cosmos DB, MySQL, PostgreSQL, and other databases through a provider plugin API.

```
Install-Package Microsoft.EntityFrameworkCore -Version 7.0.0-preview.4.22229.2

```

- Nlog

NLog is a flexible and free logging platform for various .NET platforms, including .NET standard. NLog makes it easy to write to several targets. (database, file, console) and change the logging configuration on-the-fly.

```
Install-Package NLog -Version 5.0.0

```

- NpgSql Entity Framework PostgreSQL

Npgsql.EntityFrameworkCore.PostgreSQL is the open source EF Core provider for PostgreSQL. It allows you to interact with PostgreSQL via the most widely-used .NET O/RM from Microsoft, and use familiar LINQ syntax to express queries. It's built on top of Npgsql.

```
Install-Package Npgsql.EntityFrameworkCore.PostgreSQL -Version 7.0.0-preview.4

```

- Swashbuckle AspNetCore Swagger

Middleware to expose Swagger JSON endpoints from APIs built on ASP.NET Core

```
Install-Package Swashbuckle -Version 5.6.0

```

##### Dependencies on Unit Test Blueprint

- Bogus

A simple and sane data generator for populating objects that supports different locales. A delightful port of the famed faker.js and inspired by FluentValidation. Use Bogus to create UIs with fake data or seed databases. Get started by using Faker class or a DataSet directly.

```
Install-Package Bogus -Version 34.0.2

```

- Coverlet Collector

Coverlet is a cross platform code coverage library for .NET, with support for line, branch and method coverage.

```
Install-Package coverlet.collector -Version 3.1.2

```

- Fluent Assertions

A very extensive set of extension methods that allow you to more naturally specify the expected outcome of a TDD or BDD-style unit tests. Targets .NET Framework 4.7, .NET Core 2.1 and 3.0, .NET 6, as well as .NET Standard 2.0 and 2.1. Supports the unit test frameworks MSTest2, NUnit3, XUnit2, MSpec, and NSpec3.

```
Install-Package FluentAssertions -Version 6.7.0

```

- Microsoft AspNetCore MVC Testing

Support for writing functional tests for MVC applications.

```
Install-Package Microsoft.AspNetCore.Mvc.Testing -Version 7.0.0-preview.4.22251.1

```

- Microsoft Entity Framework In Memory

In-memory database provider for Entity Framework Core (to be used for testing purposes).

```
Install-Package Microsoft.EntityFrameworkCore.InMemory -Version 7.0.0-preview.4.22229.2

```

- Microsoft SDK NET Test

The MSbuild targets and properties for building .NET test projects.

```
Install-Package Microsoft.NET.Test.Sdk -Version 17.3.0-preview-20220530-08

```

- Moq

Moq is the most popular and friendly mocking framework for .NET.

```
Install-Package Moq -Version 4.18.1

```

- XUnit

xUnit.net is a developer testing framework, built to support Test Driven Development, with a design goal of extreme simplicity and alignment with framework features.

```
Install-Package xunit -Version 2.4.2-pre.12

```
