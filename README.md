# Blueprint Backend NET Core

---

## IDE Setup

### VSCode
### Visual Studio 2019 / Visual Studio Mac

* VS Code . see [VS Code](https://code.visualstudio.com/)

* Visual Studio . see [Visual Studio setup](https://visualstudio.microsoft.com/vs/)

* dotnet windows powershell

    dotnet-install.ps1 [-Architecture <ARCHITECTURE>] [-AzureFeed]
    [-Channel <CHANNEL>] [-DryRun] [-FeedCredential]
    [-InstallDir <DIRECTORY>] [-JSonFile <JSONFILE>]
    [-NoCdn] [-NoPath] [-ProxyAddress] [-ProxyBypassList <LIST_OF_URLS>]
    [-ProxyUseDefaultCredentials] [-Quality <QUALITY>] [-Runtime <RUNTIME>]
    [-SkipNonVersionedFiles] [-UncachedFeed] [-Verbose]
    [-Version <VERSION>]

    Get-Help ./dotnet-install.ps1

* dotnet Linux/MacOS

    dotnet-install.sh  [--architecture <ARCHITECTURE>] [--azure-feed]
    [--channel <CHANNEL>] [--dry-run] [--feed-credential]
    [--install-dir <DIRECTORY>] [--jsonfile <JSONFILE>]
    [--no-cdn] [--no-path] [--quality <QUALITY>]
    [--runtime <RUNTIME>] [--runtime-id <RID>]
    [--skip-non-versioned-files] [--uncached-feed] [--verbose]
    [--version <VERSION>]

    dotnet-install.sh --help

## Local development

* Copy file `config.example.yml` to `config.yml`
* Setup database (local) to create database schema equal with config `(schema: "MyDB")`
* Setup Makefile param for migration connection database ex: `Data Source=127.0.0.1;Initial Catalog=MyDB;User ID=sa;Password=EmpatTH3010;Connect Timeout=3600;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False`
* run `dotnet ef database update` to update database
* Run `WITNetCoreProject`

## API Docs
### [Postman API Docs](https://documenter.getpostman.com/view/19475237/Uz5MEDXv "Click")

## License
[© 2022 WIT.ID](https://wit.id)
