# Repository instructions

These instructions apply to the entire repository.

## Before changing code

- Read `README.md` for setup and solution boundaries.
- For a new entity-backed feature or CRUD change, read `docs/adding-crud-module.md` completely.
- Inspect the closest current feature before designing a change. Prefer `VehicleTypes` or `DocumentTypes` for conventional reference data, `Rates` for relational forms, and `Services` or `Employees` for workflow-heavy features.
- Preserve unrelated working-tree changes.

## Git workflow

### Branches

- When creating a development branch, use `feature/<ShortPascalCaseDescription>` for new functionality, enhancements, dependency work, or documentation improvements.
- Use `bugfix/<ShortPascalCaseDescription>` for defect corrections.
- Keep the description concise, specific, and free of spaces or punctuation. Examples: `feature/RemoveGennyAndAddDocumentation` and `bugfix/FixServiceDeleteValidation`.
- Do not rename or switch away from an existing user-provided branch unless requested.

### Commits

- Use Conventional Commits in the form `<type>(<optional-scope>): <imperative summary>`.
- Prefer `feat`, `fix`, `docs`, `refactor`, `test`, `build`, or `chore` as the type.
- Keep each commit focused on one logical change and keep the summary concise and specific.
- Add a commit body only when the reason, migration impact, breaking behavior, or tradeoff is not obvious from the summary.
- Do not amend, squash, or rewrite existing commits unless requested.

### Opening pull requests

- Open a pull request only when explicitly requested.
- Target `develop` unless the user specifies another base branch.
- Open a draft pull request by default unless the user requests a ready pull request.
- Use a concise title and include a summary, validation performed, and any migration, configuration, or deployment impact in the description.
- Do not merge, force-push, close, or otherwise change the lifecycle of a pull request unless explicitly requested.

### Reviewing pull requests

- Treat a request to review a pull request as read-only unless the user also requests fixes or another write action.
- Prioritize correctness, regressions, security, data integrity, and missing tests.
- Report findings first, ordered by severity, with precise file and line references.
- Do not approve, request changes, or post comments on GitHub unless explicitly requested.
- If there are no findings, state that clearly and mention any remaining validation gaps or untested risks.

## Architecture and conventions

- Keep domain models and view models in `AppLogistics.Objects`, persistence in `AppLogistics.Data`, services in `AppLogistics.Services`, business validation in `AppLogistics.Validators`, controllers in `AppLogistics.Controllers`, and Razor views/static assets in `AppLogistics.Web`.
- Do not use Genny or reintroduce generic code-generation dependencies. Use existing features as references and implement the complete vertical slice required by the behavior.
- Follow the existing naming convention so services and validators are discovered automatically: `IFooService`/`FooService` and `IFooValidator`/`FooValidator`.
- Treat validation, authorization, localization, database migration, and tests as part of the feature rather than follow-up work.
- Add both English and Spanish resource files when introducing localized view or validation text.
- Protect create actions from overposting and validate delete operations when relationships or business rules can prevent deletion.
- Add or update `mvc.sitemap`, lookup endpoints, mappings, test factories, and test database cleanup when the feature needs them.
- Do not commit secrets, generated browser bundles, `bin`, `obj`, or `node_modules`.

## Documentation maintenance

- Evaluate documentation impact as part of every change and include relevant documentation updates in the same branch.
- Update `README.md` when prerequisites, setup, configuration, dependencies, solution structure, commands, or user-facing application scope changes.
- Update `AGENTS.md` when repository-wide architecture boundaries, coding conventions, workflows, branch strategy, or required validation changes.
- Update `docs/adding-crud-module.md` when the CRUD workflow, required vertical-slice components, or recommended reference features change.
- Keep documentation accurate and concise. Do not edit documentation merely to restate implementation details that are clear from the code.

## Validation

After .NET changes, run the narrowest relevant tests and then, when practical:

```powershell
dotnet restore AppLogistics.sln --property:Configuration=Debug
dotnet build AppLogistics.sln --configuration Debug --no-restore
dotnet test test/AppLogistics.Tests/AppLogistics.Tests.csproj --configuration Debug --no-build
```

After front-end source or bundling changes, also run:

```powershell
Push-Location src/AppLogistics.Web
npm ci
npm run build
Pop-Location
```

The projects treat warnings as errors. Do not suppress a warning without documenting why the underlying issue cannot be fixed.
