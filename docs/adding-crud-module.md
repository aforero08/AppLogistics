# Adding a CRUD module

This guide describes the expected vertical slice for an entity-backed AppLogistics feature. It replaces the retired Genny scaffold. Existing code is the source of implementation patterns; this guide identifies the pieces that must be considered rather than prescribing identical CRUD for every domain.

## 1. Choose the closest reference

Start with a feature that resembles the requested behavior:

| Feature | Use it as a reference for |
| --- | --- |
| `Configuration/DocumentTypes` or `Configuration/VehicleTypes` | Conventional reference-data CRUD, localized fields, duplicate checks, and protected deletion |
| `Operation/Rates` | Relationships, lookup-backed fields, and distinct read/create-edit models |
| `Operation/Employees` | Larger forms, relational validation, and domain-specific delete rules |
| `Operation/Services` | Multi-value lookups, filtered lookups, custom actions, and stateful workflows |

Copy intent and conventions, not entire files. Confirm the domain rules before writing code; a generated five-action controller is not automatically correct.

## 2. Define the behavior first

Document the following before implementation:

- The area, route, controller name, and navigation location.
- The fields displayed on index, details, create, and edit screens.
- Required fields, lengths, formats, uniqueness rules, and allowed state transitions.
- Relationships and whether each is selected through a lookup.
- Whether read and create/edit screens require different view models.
- What should happen when a referenced record is deleted.
- Required roles or actions and whether a custom workflow action is needed.

## 3. Implement the vertical slice

### Domain model and mapping

- Add or update the entity under `src/AppLogistics.Objects/Models/<Area>/<Feature>/`.
- Use data annotations and existing custom attributes for structural constraints and indexes.
- Add navigation properties on both sides when the relationship is navigable in the domain.
- Add a `DbSet` to `src/AppLogistics.Data/Core/Context.cs` when introducing an entity.
- Add view models under `src/AppLogistics.Objects/Views/<Area>/<Feature>/`.
- Derive conventional mapped views from `BaseView<TEntity>`. Use explicit mappings in `MappingProfile` when names, shapes, collections, or workflows differ.
- Prefer separate read and create/edit views when exposing the entity directly would over-post fields or make the UI model ambiguous.

### Migration

Restore the repository's version-matched local EF Core CLI, then create a migration after the model is complete:

```powershell
dotnet tool restore
dotnet ef migrations add <MigrationName> --project src/AppLogistics.Data --startup-project src/AppLogistics.Web
```

Review both the migration and model snapshot. Check column types and lengths, nullability, indexes, foreign keys, and delete behavior. Do not accept destructive data changes without an explicit migration strategy.

### Service

- Add `I<Feature>Service` and `<Feature>Service` under `src/AppLogistics.Services/<Area>/<Feature>/`.
- Follow the existing `IService`/`BaseService` conventions so dependency injection discovers the implementation automatically.
- Keep persistence queries and unit-of-work operations in the service.
- Return the appropriate view shape for each screen; do not force one view model onto incompatible read and edit scenarios.
- Commit each completed write operation consistently with neighboring services.

### Validation

- Add `I<Feature>Validator` and `<Feature>Validator` under `src/AppLogistics.Validators/<Area>/<Feature>/`.
- Check `ModelState` and all business invariants for create and edit.
- Implement `CanDelete` when foreign-key relationships, active usage, or business policy can block deletion.
- Add validation resource keys instead of hard-coded user-facing messages.
- Consider database-enforced uniqueness in addition to friendly pre-checks where concurrent writes matter.

### Controller and authorization

- Add the controller under `src/AppLogistics.Controllers/<Area>/<Feature>/`.
- Apply `[Area]` consistently with neighboring controllers.
- Derive from the appropriate base controller and inject the matching service and validator interfaces.
- Apply `[BindExcludeId]` or a purpose-built input model on create actions to prevent identifier over-posting.
- Return the same view model on validation failures so validation messages can render.
- Validate delete and custom workflow actions before changing state.
- Add custom actions only when they represent real domain behavior; update tests and navigation/authorization metadata with them.

Authorization inspects controller actions at runtime, but database permissions are seeded explicitly. For every action that should be grantable:

- Add a `Permission` with a stable, unique ID to `DatabaseConfiguration.GetSeedPermissions()` in `src/AppLogistics.Data/Migrations/DatabaseConfiguration.cs`.
- Add the controller and action labels to both `src/AppLogistics.Resources/Resources/Shared/Permission.json` and `Permission.es.json`.
- Add the permission case to `test/AppLogistics.Tests/Unit/Data/Migrations/InitialDataTests.cs` and update its exact permission count.

Add the feature and its navigable actions to `src/AppLogistics.Web/mvc.sitemap` so authorized users can reach them.

### Views and lookups

- Add Razor views under `src/AppLogistics.Web/Views/<Area>/<Feature>/`.
- Follow the current layout, grid, form-group, validation, authorization-tag-helper, and Cancel-action conventions.
- Keep index columns intentional; do not expose every scalar property automatically.
- Use MVC Lookup for relationships instead of free-form identifiers.
- Add or extend an endpoint in `src/AppLogistics.Controllers/Lookup/LookupController.cs` when the related entity must be selectable.
- Use filtered or multi-value lookups only where the domain requires them, following `Rates` and `Services`.
- Add feature-specific JavaScript or CSS under the existing Application asset structure and rebuild bundles when client behavior changes.

### Localization and resources

- Add paired English and Spanish JSON files under `src/AppLogistics.Resources/Resources/Views/<Area>/<Feature>/` for every new view model.
- Add shared, validation, lookup, page, or sitemap resource keys in the matching resource files when required.
- Use `Resource` and `Validation` helpers in application code rather than embedding user-facing strings.

### Tests

Add focused tests under the matching `test/AppLogistics.Tests/Unit/` paths:

- Controller tests for returned models, failed validation, successful writes, redirects, over-posting protection, delete protection, and custom actions.
- Service tests for projections, filters, mappings, writes, and relationship changes.
- Validator tests for invalid model state, uniqueness, relationship constraints, protected deletion, and valid cases.
- Model/view tests when attributes, mapping, or computed properties carry behavior.

Update `test/AppLogistics.Tests/Helpers/ObjectsFactory.cs` with valid default objects. If the entity participates in database tests, update `TestFixture.Drop` in dependency order so shared SQLite cleanup remains reliable.

## 4. Validate completion

Run the standard checks from the repository root:

```powershell
dotnet restore AppLogistics.sln --property:Configuration=Debug
dotnet build AppLogistics.sln --configuration Debug --no-restore
dotnet test test/AppLogistics.Tests/AppLogistics.Tests.csproj --configuration Debug --no-build
```

When browser assets changed:

```powershell
Push-Location src/AppLogistics.Web
npm ci
npm run build
Pop-Location
```

Finally, exercise the authorized UI flow against a disposable local database:

1. Open the index and verify localization, grid columns, and authorization-aware actions.
2. Create a valid record and try each important invalid case.
3. View and edit the record, including relationship changes.
4. Test allowed and blocked deletion paths.
5. Exercise custom actions and confirm the resulting persisted state.
6. Confirm the migration works against both a new database and the intended upgrade path.

A feature is complete only when its database, application, UI, localization, authorization, and test behavior agree.
