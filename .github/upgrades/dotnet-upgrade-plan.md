# .NET 6.0 Upgrade Plan

## Execution Steps

Execute steps below sequentially one by one in the order they are listed.

1. Validate that an .NET 6.0 SDK required for this upgrade is installed on the machine and if not, help to get it installed.
2. Ensure that the SDK version specified in global.json files is compatible with the .NET 6.0 upgrade.
3. Upgrade src/AppLogistics.Data/AppLogistics.Data.csproj
4. Upgrade src/AppLogistics.Web/AppLogistics.Web.csproj
5. Upgrade test/AppLogistics.Tests/AppLogistics.Tests.csproj
6. Run unit tests to validate upgrade in the projects listed below:
  (No test projects discovered)

## Settings

This section contains settings and data used by execution steps.

### Excluded projects

Table below contains projects that do belong to the dependency graph for selected projects and should not be included in the upgrade.

| Project name                                   | Description                 |
|:-----------------------------------------------|:---------------------------:|

### Aggregate NuGet packages modifications across all projects

NuGet packages used across all selected projects or their dependencies that need version update (add / remove / upgrade).

| Package Name                             | Current Version | New / Action | Description                                                     |
|:-----------------------------------------|:---------------:|:------------:|:----------------------------------------------------------------|
| Microsoft.AspNetCore                     | 2.2.0           | remove       | In-box via shared framework / deprecated                        |
| Microsoft.AspNetCore.Authentication.Cookies | 2.2.0        | 2.3.0        | Deprecated; interim baseline before consolidating auth refs     |
| Microsoft.AspNetCore.Diagnostics         | 2.2.0           | remove       | In-box via shared framework / deprecated                        |
| Microsoft.AspNetCore.Hosting             | 2.2.0           | remove       | In-box via shared framework / deprecated                        |
| Microsoft.AspNetCore.HttpsPolicy         | 2.2.0           | remove       | In-box via shared framework / deprecated                        |
| Microsoft.AspNetCore.Mvc                 | 2.2.0           | 2.3.0        | Deprecated; interim baseline prior to .NET 6 implicit reference |
| Microsoft.AspNetCore.Mvc.Core            | 2.2.2           | 2.3.0        | Deprecated; interim baseline                                    |
| Microsoft.AspNetCore.Mvc.DataAnnotations | 2.2.0           | 2.3.0        | Deprecated; interim baseline                                    |
| Microsoft.AspNetCore.Mvc.TagHelpers      | 2.2.0           | remove       | TagHelpers provided by framework                                |
| Microsoft.AspNetCore.Server.IIS          | 2.2.6           | remove       | In-box via shared framework                                     |
| Microsoft.AspNetCore.Server.IISIntegration | 2.2.1         | remove       | In-box via shared framework                                     |
| Microsoft.AspNetCore.Server.Kestrel      | 2.2.0           | remove       | In-box via shared framework                                     |
| Microsoft.AspNetCore.Session             | 2.2.0           | remove       | In-box via shared framework / deprecated                        |
| Microsoft.AspNetCore.StaticFiles         | 2.2.0           | remove       | In-box via shared framework                                     |
| Microsoft.EntityFrameworkCore.Design     | 2.2.4           | 6.0.36       | Align with EF Core 6                                            |
| Microsoft.EntityFrameworkCore.InMemory   | 2.2.4           | 6.0.36       | Align with EF Core 6 (tests)                                    |
| Microsoft.EntityFrameworkCore.Proxies    | 2.2.4           | 6.0.36       | Align with EF Core 6                                            |
| Microsoft.EntityFrameworkCore.SqlServer  | 2.2.4           | 6.0.36       | Align with EF Core 6                                            |
| Microsoft.EntityFrameworkCore.Tools      | 2.2.0           | 6.0.36       | Align with EF Core 6 tooling                                    |
| Microsoft.Extensions.Configuration.Json  | 2.2.0           | 6.0.1        | Align with .NET 6 BCL                                           |
| Microsoft.Extensions.Logging.Console     | 2.2.0           | 6.0.1        | Align with .NET 6 logging APIs                                  |
| Microsoft.NETCore.Platforms              | 2.2.0           | 7.0.4        | Updated platform definitions                                    |
| Microsoft.VisualStudio.Web.CodeGeneration.Design | 2.2.3   | 6.0.18       | Update scaffolding tooling                                      |
| Newtonsoft.Json                          | 13.0.1          | 13.0.4       | Patch update                                                     |
| Microsoft.VisualStudio.Web.BrowserLink   | 2.2.0           | remove       | Deprecated / not needed                                         |

### Packages intentionally held at current version

| Package Name | Current Version | Reason for holding |
|:-------------|:---------------:|:-------------------|
| EPPlus       | 4.5.3.3         | User requested to keep current version for now (license/runtime considerations) |

### NonFactors packages evaluation

Current NonFactors packages (Grid / Lookup) are not flagged by automated analysis. They target MVC6 naming (original ASP.NET Core) and currently remain at:
- NonFactors.Grid.Core.Mvc6 4.1.1
- NonFactors.Grid.Mvc6 4.1.1
- NonFactors.Lookup.Core.Mvc6 3.2.1
- NonFactors.Lookup.Mvc6 3.2.1

Planned action: retain versions during initial .NET 6 upgrade. After upgrading and building, validate:
1. Compilation succeeds without obsolete API errors.
2. Runtime views using grids/lookups render correctly.
3. No missing method/type exceptions at runtime.
If issues arise, investigate newer package lines or maintained forks and schedule a follow-up upgrade (out of current scope).

### Project upgrade details

#### src/AppLogistics.Data/AppLogistics.Data.csproj modifications

Project properties changes:
  - Target frameworks should be changed from `netcoreapp2.2;netstandard2.0` to `netcoreapp2.2;netstandard2.0;net6.0`

NuGet packages changes:
  - Microsoft.NETCore.Platforms from `2.2.0` to `7.0.4` (*deprecated*)
  - Microsoft.AspNetCore.Hosting remove (framework provided)
  - Microsoft.AspNetCore.Server.Kestrel remove (framework provided)
  - Microsoft.EntityFrameworkCore.Design from `2.2.4` to `6.0.36`
  - Microsoft.EntityFrameworkCore.Proxies from `2.2.4` to `6.0.36`
  - Microsoft.EntityFrameworkCore.SqlServer from `2.2.4` to `6.0.36`
  - Microsoft.Extensions.Configuration.Json from `2.2.0` to `6.0.1`

Other changes:
  - Review EF Core 6 breaking changes (lazy loading proxies, design-time services)

#### src/AppLogistics.Web/AppLogistics.Web.csproj modifications

Project properties changes:
  - Target framework should be changed from `netcoreapp2.2` to `net6.0`

NuGet packages changes:
  - Remove packages now provided by `Microsoft.AspNetCore.App` shared framework: Microsoft.AspNetCore, Diagnostics, HttpsPolicy, StaticFiles, Session, Kestrel, IISIntegration, IIS, MVC.TagHelpers
  - Microsoft.EntityFrameworkCore.Tools from `2.2.0` to `6.0.36`
  - Microsoft.VisualStudio.Web.CodeGeneration.Design from `2.2.3` to `6.0.18`
  - Microsoft.Extensions.Logging.Console from `2.2.0` to `6.0.1`

Other changes:
  - Migrate Startup.cs to minimal hosting model (Program.cs builder pattern)
  - Replace deprecated logging configuration if any
  - Re-test static file, session, HTTPS redirection middleware via implicit framework reference
  - Keep EPPlus version unchanged initially
  - Validate NonFactors components rendering

#### test/AppLogistics.Tests/AppLogistics.Tests.csproj modifications

Project properties changes:
  - Target framework should be changed from `netcoreapp2.2` to `net6.0`

NuGet packages changes:
  - Microsoft.EntityFrameworkCore.InMemory from `2.2.4` to `6.0.36`
  - Consider removing Microsoft.DotNet.InternalAbstractions (deprecated, rarely needed on .NET 6)

Other changes:
  - Adjust any obsolete API usages in tests (EF Core DbContextOptions patterns)

