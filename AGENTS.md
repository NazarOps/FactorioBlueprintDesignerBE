# Repository Guidelines

> Applies to the **Factorio Blueprint Designer backend repository** (`FactorioBlueprintDesignerAPI.csproj`). Follow `docs/CODING_STANDARDS.md` for the full coding and Git workflow policy. The GitHub Project #10 board uses **Todo → In Progress → In Review → Done**.

## Project Structure & Architecture

This repository contains a **.NET 10 ASP.NET Core API** with **PostgreSQL**, **Entity Framework Core**, **Vertical Slice Architecture (VSA)**, and **CQRS with MediatR**. `Program.cs` registers services, database access, and development-only OpenAPI/Scalar documentation.

- `Features/<Domain>/<Operation>.cs` — group each feature's endpoint, MediatR command/query, handler, and validator together where practical. `Features/Blueprints/CreateBlueprint.cs` is the initial pattern; do not assume every planned endpoint is implemented.
- `Database/AppDbContext.cs` — EF Core context. Inject `AppDbContext` **directly into handlers**; do not add repositories or generic repository layers without a concrete need.
- `Entities/` and `Entities/Enum/` — persistence models and enums.
- `Migrations/` — EF Core migrations and model snapshot; commit migrations alongside model changes.
- `Shared/` — shared Result/error types where present. Use **FluentValidation** for request validation and the **Result pattern** for expected outcomes.
- `Properties/launchSettings.json` — local launch profiles.
- `.github/workflows/` — CI workflows, where present.

Keep persistence entities, API contracts, and Factorio import/export DTOs separate. Do not add unnecessary abstractions. The **React/TypeScript editor and IndexedDB autosave belong to a separate frontend repository**. Its guest/offline-first Milestone 1 must not depend on this API; cloud accounts and saves are later-phase functionality.

## Build, Test & Development Commands

Run from the backend repository root using the .NET 10 SDK:

```powershell
dotnet restore ./FactorioBlueprintDesignerAPI.csproj
dotnet build ./FactorioBlueprintDesignerAPI.csproj --configuration Release
dotnet run --project ./FactorioBlueprintDesignerAPI.csproj --launch-profile https
dotnet ef migrations add <DescriptiveName>
dotnet ef database update
dotnet format ./FactorioBlueprintDesignerAPI.csproj --verify-no-changes --no-restore
```

The EF commands require the EF Core CLI tooling and a configured local PostgreSQL connection. Run `dotnet test` when test projects exist. Do not claim tests, endpoints, or CI jobs exist until verified in the repository. OpenAPI and Scalar are available in Development when configured.

## C# Coding Standards

- Four-space indentation, braces on separate lines, and consistent formatting; follow `.editorconfig` when present.
- `PascalCase` for types, properties, methods, enums, and constants; `camelCase` for parameters/locals; `_camelCase` for private fields; prefix interfaces with `I`.
- Name feature files by operation, e.g. `CreateBlueprint.cs`; use plural feature folders such as `Features/Blueprints/`.
- Preserve nullable reference annotations. Avoid suppressing nullability except where EF Core navigation initialization warrants it.
- Prefer asynchronous EF Core calls and pass `CancellationToken` through handlers. Persist timestamps in UTC.
- Validate input before persistence. Return consistent HTTP status codes and Result-based errors; do not expose EF entities directly as public contracts.
- Treat imported Factorio blueprint strings as untrusted input. Bound decompressed payload sizes; reject malformed data; **warn visibly when unsupported entities are skipped**.
- Do not commit unused code, debug prints, credentials, or generated build artifacts.

## Testing & CI

Test validation and business behavior with unit tests; use integration tests for API/database flows, including actual PostgreSQL persistence where relevant. Cover success, invalid input, not-found, and regression cases. Import/export tests should include malformed input, unsupported entities, and round trips. FluentValidation does not replace tests.

Before a PR, run restore, formatting verification, Release build, and any available tests. Backend CI should perform the same checks on PRs to the default branch. Reference the **exact** project filename `FactorioBlueprintDesignerAPI.csproj` in workflows. No coverage threshold is assumed unless separately established.

## Branches, Commits & Pull Requests

Use the repository's current default branch (**`master` if that is still configured**); do not rename it implicitly. Create short-lived branches using:

`<type>/<issue-number>-<short-kebab-case-description>`

Allowed types: `feat`, `fix`, `refactor`, `test`, `docs`, `chore`, `ci`. Examples tied to the existing project issues:

| Issue | Suggested branch |
|---|---|
| #4 Offline-first progress documentation | `docs/4-offline-first-progress-tracking` |
| #5 Cloud blueprint contract/revisions | `docs/5-cloud-blueprint-contract` |
| #6 Local PostgreSQL setup | `chore/6-local-postgres-setup` |
| #7 API/PostgreSQL integration tests | `test/7-api-postgres-integration` |
| #8 Validation and API errors | `feat/8-validation-api-errors` |
| #9 Account registration/login/ownership | `feat/9-account-authentication` |
| #10 API PR CI | `ci/10-api-pr-build-test` |
| #11 Entity-definition catalog | `feat/11-entity-definition-catalog` |

These are **suggested** branch names based on visible GitHub Project issues, not confirmed existing branches. Use the actual issue number for new tasks. If no issue exists, create/link one or omit the number.

Use **Conventional Commits**, e.g. `feat(blueprints): add create blueprint command`, `fix(database): correct connection string lookup`, `ci: verify formatting on pull requests`. Keep commits focused.

PRs should target the default branch, link the issue (`Closes #N` when appropriate), summarize behavior and acceptance criteria, report validation, and identify migrations or breaking changes. Review the diff and require passing CI before merging; squash merge is preferred.

## GitHub Project Workflow & Definition of Done

Project board: `https://github.com/users/NazarOps/projects/10`.

1. **Todo:** issue scoped; implementation not started.
2. **In Progress:** branch created and work underway.
3. **In Review:** PR open, review and CI checks underway.
4. **Done:** acceptance criteria met, PR merged, changes verified, and issue/project item closed or completed.

Move an item back to **In Progress** when review requires changes. Do not introduce unverified statuses such as Backlog or Ready. Do not assume board fields beyond the four confirmed statuses.

A task is **Done** only when its acceptance criteria, relevant validation/tests, formatting, CI, migration/documentation updates, and merge requirements are satisfied.

## Local Configuration & Security

Configure `ConnectionStrings:FactorioBlueprintDesignerDb` in `appsettings.Development.json`, .NET user secrets, or environment variable `ConnectionStrings__FactorioBlueprintDesignerDb`. The configuration **key** need not match the actual PostgreSQL database name; use the exact real database name in the connection string. Keep secrets out of Git and logs. Exclude `bin/`, `obj/`, `.vs/`, and local credential files from commits. Ensure the Development environment is selected when relying on development-only settings during migrations.
