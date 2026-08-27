# AppLogistics

AppLogistics is an ASP.NET Core MVC application for managing logistics reference data, rates, employees, service operations, and reports. The application targets .NET 8, uses Entity Framework Core with SQL Server in production, and renders server-side Razor views.

## Prerequisites

- .NET 8 SDK
- Node.js 22 or later and npm
- SQL Server or SQL Server LocalDB when running the web application

The test suite uses an in-memory SQLite database by default, so SQL Server is not required to build or test the solution.

## Configure the application

Development-only values belong in .NET user secrets or `APPLOGISTICS_` environment variables. From the repository root, configure at least a database and the initial administrator account:

```powershell
dotnet user-secrets set "Data:Connection" "Server=(localdb)\MSSQLLocalDB;Database=AppLogistics;Trusted_Connection=True;TrustServerCertificate=True;" --project src/AppLogistics.Web
dotnet user-secrets set "UserAdmin:UserName" "admin" --project src/AppLogistics.Web
dotnet user-secrets set "UserAdmin:Password" "replace-with-a-local-password" --project src/AppLogistics.Web
dotnet user-secrets set "UserAdmin:Email" "admin@example.com" --project src/AppLogistics.Web
```

Set `Mail:SendGridApiKey` as a secret as well when exercising email flows. Do not commit credentials or connection strings.

The application applies pending Entity Framework migrations and seeds authorization data when it starts. Use a disposable local database for development.

## Build and run

Restore .NET and front-end dependencies, then build the generated browser assets:

```powershell
dotnet restore AppLogistics.sln --property:Configuration=Debug
dotnet tool restore
Push-Location src/AppLogistics.Web
npm ci
npm run build
Pop-Location
dotnet build AppLogistics.sln --configuration Debug --no-restore
```

Run the web application with:

```powershell
$env:ASPNETCORE_ENVIRONMENT = "Development"
dotnet run --project src/AppLogistics.Web --configuration Debug --no-launch-profile
```

Run the automated tests with:

```powershell
dotnet test test/AppLogistics.Tests/AppLogistics.Tests.csproj --configuration Debug
```

## Solution structure

| Project | Responsibility |
| --- | --- |
| `AppLogistics.Web` | Application startup, Razor views, static assets, and runtime configuration |
| `AppLogistics.Controllers` | MVC controllers and HTTP workflows |
| `AppLogistics.Services` | Application services and persistence orchestration |
| `AppLogistics.Validators` | Business validation and user-facing validation alerts |
| `AppLogistics.Objects` | Domain models, view models, and AutoMapper configuration |
| `AppLogistics.Data` | EF Core context, unit of work, migrations, and database initialization |
| `AppLogistics.Components` | Shared MVC, security, mail, reporting, and infrastructure components |
| `AppLogistics.Resources` | Localized view and validation resources |
| `AppLogistics.Tests` | Unit and integration-style tests using SQLite by default |

## Contributing

Read [Adding a CRUD module](docs/adding-crud-module.md) before implementing a new entity-backed feature. It explains how to select a current reference module and carry a change through the model, database, service, validation, controller, UI, localization, authorization, and tests.

Read the [browser dependency baseline](docs/browser-dependency-baseline.md) before changing browser-library acquisition or versions. It records the current vendored libraries, local modifications, generated-asset hashes, and required smoke tests.

Repository-specific coding-agent instructions are in [AGENTS.md](AGENTS.md).
