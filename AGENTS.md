# Repository Guidelines

## Project Structure & Module Organization

This repository contains a .NET 10 ASP.NET Core API for Factorio blueprints. `Program.cs` configures services, PostgreSQL, and development API documentation. `Features/Blueprints/CreateBlueprint.cs` establishes the vertical slice and CQRS pattern: keep each operation's MediatR command and handler together under `Features/<Domain>/`.

`Entities/` contains persistence models, with direction values in `Entities/Enum/`. `Database/AppDbContext.cs` defines EF Core sets; `Migrations/` stores generated schema changes and the model snapshot. `Properties/launchSettings.json` defines local profiles. There are currently no test projects, frontend, or static asset directories.

## Build, Test, and Development Commands

Run commands from the repository root with the .NET 10 SDK installed:

- `dotnet restore` — restore NuGet dependencies.
- `dotnet build` — compile the solution with nullable reference checking enabled.
- `dotnet run --project FactorioBlueprintDesignerAPI.csproj --launch-profile https` — start the development API at `https://localhost:7199` and `http://localhost:5284`.
- `dotnet ef migrations add <DescriptiveName>` — generate a migration after model changes; requires the EF CLI tool.
- `dotnet ef database update` — apply migrations to the configured local PostgreSQL database.

Development enables OpenAPI and Scalar. Feature endpoints and MediatR registration are not yet wired into `Program.cs`; complete that wiring when exposing handlers.

## Coding Style & Naming Conventions

Match existing C#: four-space indentation, braces on separate lines, and block-scoped namespaces under `FactorioBlueprintDesigner`. Use PascalCase for types, properties, and methods; camelCase for parameters and locals; and `_camelCase` for private fields. Match filenames to their main type or feature operation. Preserve nullable annotations, pass cancellation tokens to asynchronous database calls, and use `DateTime.UtcNow` for persisted timestamps. No formatter or lint configuration is checked in.

## Testing Guidelines

No testing framework or coverage threshold is configured. Build before submitting changes and describe manual validation. The `.http` file still targets the unregistered `/weatherforecast/` sample; update requests when adding endpoints. For future automated tests, use descriptive names such as `Handle_ValidCommand_PersistsBlueprint` and document the test project and execution command.

## Commit & Pull Request Guidelines

History uses brief, informal lowercase subjects, without a mandated prefix. Write focused, descriptive commits. Pull requests should explain behavior changes, link relevant issues, list validation performed, and call out migrations or configuration changes.

## Local Configuration

Set `ConnectionStrings:FactorioBlueprintDesignerDb` in ignored `appsettings.Development.json` or through `ConnectionStrings__FactorioBlueprintDesignerDb`. Keep credentials and generated `bin/`, `obj/`, and `.vs/` files out of commits.
