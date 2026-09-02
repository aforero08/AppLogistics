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

The browser build reads managed third-party sources from `node_modules`, generates ignored Development copies under `wwwroot/*/Dependencies`, and produces the minified Staging/Production bundles. Do not edit generated dependency copies or bundles directly.

### Visual Studio 2026

Open `AppLogistics.sln`, set `AppLogistics.Web` as the startup project, select the `IIS Express (Development)` launch profile, and press F5 or Ctrl+F5. The profile runs the application at `http://localhost:5101/domain/` and loads the Development user secrets configured above.

The application applies pending Entity Framework migrations and seeds the initial administrator when the database starts for the first time. Make sure `MSSQLLocalDB` is available and the `Data:Connection` user secret includes `TrustServerCertificate=True`.

### Command line

Start LocalDB if it is not already running, then run the web project with an explicit local URL:

```powershell
sqllocaldb start MSSQLLocalDB
$env:ASPNETCORE_ENVIRONMENT = "Development"
$env:ASPNETCORE_URLS = "http://127.0.0.1:5099"
dotnet run --project src/AppLogistics.Web --configuration Debug --no-launch-profile
```

Open `http://127.0.0.1:5099` and sign in with the administrator account stored in user secrets. Stop the application with Ctrl+C.

### Browser smoke testing with a disposable database

For repeatable browser testing without modifying the normal development database, override configuration only for the current PowerShell process. Supply a temporary strong password rather than committing one:

```powershell
sqllocaldb start MSSQLLocalDB
$env:ASPNETCORE_ENVIRONMENT = "Development"
$env:ASPNETCORE_URLS = "http://127.0.0.1:5099"
$env:APPLOGISTICS_Data__Connection = "Server=(localdb)\MSSQLLocalDB;Database=AppLogisticsSmokeTests;Trusted_Connection=True;TrustServerCertificate=True;"
$env:APPLOGISTICS_UserAdmin__UserName = "smokeadmin"
$env:APPLOGISTICS_UserAdmin__Password = "<temporary-strong-password>"
$env:APPLOGISTICS_UserAdmin__Email = "smokeadmin@example.test"
dotnet run --project src/AppLogistics.Web --configuration Debug --no-launch-profile
```

The administrator values are used only when the disposable database is first seeded. If different credentials are needed later, choose a new disposable database name or recreate the existing smoke-test database.

For uniqueness-validation smoke tests, sign in and verify both paths:

1. Edit an existing record without changing its unique value and confirm that saving succeeds.
2. Edit it to use another record's unique value and confirm that the duplicate validation message is shown.

Run the automated test suite after the browser checks. Browser testing is supplemental and should not replace the validator tests.

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
