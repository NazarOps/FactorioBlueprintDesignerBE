# Project Progress

Last reviewed: 2026-11-14. Based on the current directory and Git history through `61b81c7`.

## Current Status

The project is an early .NET 10 ASP.NET Core backend. Its persistence model, initial PostgreSQL migration, and first CQRS command handler are implemented. Blueprint creation is not yet accessible through HTTP, and database persistence has not been verified during this review.

## Completed Work

- **API foundation:** Web SDK project, nullable reference checking, local HTTP/HTTPS launch profiles, HTTPS redirection, and development OpenAPI/Scalar documentation.
- **Persistence setup:** EF Core with Npgsql, dependency injection for `AppDbContext`, and five database sets.
- **Domain models:** Accounts, blueprints, blueprint books, placed blueprint entities, and entity definitions. Models include ownership relationships, optional book membership, positions, and cardinal direction values.
- **Schema migration:** Initial migration and model snapshot define tables, foreign keys, indexes, and PostgreSQL timestamp columns. Migration files exist; application to a live database is unverified.
- **First feature slice:** `CreateBlueprint.Command` accepts name, optional description, and version. Its handler generates a GUID, assigns `DateTime.UtcNow`, calls `SaveChangesAsync` with cancellation support, and returns the ID.
- **Contributor documentation:** `AGENTS.md` describes repository conventions and development commands; it is currently untracked.

## Git Milestones

All four commits are dated 2026-11-14:

| Commit | Change |
| --- | --- |
| `e65248b` | Initial API project, entities, database context, and PostgreSQL migration. |
| `f5e617a` | First vertical slice using MediatR and CQRS. |
| `ef768a5` | Renamed the project to `FactorioBlueprintDesignerAPI.csproj` and updated the solution. |
| `61b81c7` | Changed blueprint creation timestamps to UTC. |

## Directory Structure

| Path | Purpose |
| --- | --- |
| `Program.cs` | Service registration and HTTP application setup. |
| `Features/Blueprints/` | Blueprint operations; currently only `CreateBlueprint.cs`. |
| `Entities/`, `Entities/Enum/` | Persistence models and direction enumeration. |
| `Database/` | EF Core database context. |
| `Migrations/` | Initial schema migration and model snapshot. |
| `Properties/` | Development launch profiles. |
| `Extentions/` | Empty placeholder directory. |

## Known Gaps

- `Program.cs` does not register MediatR handlers or map business endpoints.
- Blueprint creation leaves `AccountId` at its default value of `0`. The schema requires an existing account, so persistence will fail unless that account exists.
- No request validation, authentication, authorization, or application error handling is implemented.
- Read, update, delete, book management, and entity definition operations are absent. Blueprint import/export and a visual designer are also absent.
- No test project, automated coverage, or CI workflow is checked in.
- The `.http` file still requests `/weatherforecast/`, which is not mapped. Tracked configuration omits the connection string; local database configuration was not inspected.

## Recommended Next Steps

1. Register MediatR and expose blueprint creation through an HTTP endpoint with a documented response.
2. Resolve account ownership before saving: provide a valid development account initially and establish authenticated ownership before multi-user use. Validate input and return appropriate errors.
3. Configure `ConnectionStrings:FactorioBlueprintDesignerDb` locally, apply the migration to a development database, and verify creation end to end.
4. Add handler and API integration tests covering persistence, invalid input, missing accounts, and UTC timestamps; replace the stale HTTP example.
5. Implement blueprint retrieval, listing, editing, and deletion, followed by book and placed-entity operations.
6. Define requirements for Factorio import/export and the visual editor before implementing those features; add CI build and test checks.

## Verification

Source files, migrations, directory structure, and all four commits were inspected. `dotnet build FactorioBlueprintDesignerAPI.csproj --no-restore` succeeded with zero warnings and errors. Specify the project or solution explicitly: bare `dotnet build` returned MSB1011 in this directory. No API requests, database migrations, or automated tests were run.
