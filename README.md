# Factorio Blueprint Planner

A desktop-first web application for planning, creating, and editing Factorio blueprints outside the game. The goal is to provide a visual grid-based editor where players can design layouts, save their work locally, and eventually import and export blueprints compatible with Factorio.

> **Status:** Work in progress. The first milestone focuses on a usable offline-first editor; the backend and cloud features are planned for later.

## Project goals

- Create and edit blueprints on an interactive grid.
- Place, move, rotate, and remove Factorio entities.
- Automatically save blueprints locally and restore them when returning to the app.
- Import and export Factorio-compatible blueprint strings (second milestone).
- Eventually support accounts, cloud storage, and synchronization across devices.
- Android mobile application with React Native.

## Tech stack

| Area | Technology | Role |
| --- | --- | --- |
| Component | Technology | Purpose |
|---|---|---|
| Web client | React + TypeScript | Interactive blueprint editor and user interface |
| Mobile app | React Native + TypeScript | Mobile blueprint editor for Android and iOS |
| Local persistence (Web) | IndexedDB | Offline-first browser storage and autosave |
| Local persistence (Mobile) | SQLite | Offline-first mobile storage and autosave |
| API (later phase) | ASP.NET Core / .NET | Cloud-related endpoints |
| API architecture | Vertical Slice Architecture (VSA) | Organize backend code by feature |
| Request handling | CQRS + MediatR | Separate commands and queries |
| Data access | Entity Framework Core | Database access directly from handlers |
| Cloud database (later phase) | PostgreSQL | Persist accounts and cloud blueprints |

## Repositories and project structure

The client and API are maintained as **separate projects/repositories**. The following is an illustrative feature-oriented layout, not a claim that every file already exists.

### `FactorioBlueprintDesignerFE` (later phase)

```text
FactorioPlanner-Client/
├── src/
│   ├── features/
│   │   ├── editor/       # Grid and blueprint editing
│   │   ├── library/      # Saved local blueprints
│   │   └── import/       # Factorio blueprint import/export (planned)
│   ├── services/         # IndexedDB persistence and related utilities
│   └── types/            # Shared TypeScript types
└── README.md
```

### `FactorioBlueprintDesignerBE` (priority)

```text
FactorioPlanner-API/
├── Features/
│   ├── Blueprints/       # Blueprint commands, queries and endpoints
│   ├── Accounts/         # Account-related features (planned)
│   └── BlueprintBooks/   # Blueprint collections (planned)
├── Models/               # Data/domain models
├── Data/
│   └── AppDbContext.cs   # EF Core database context
└── Program.cs            # Application configuration and endpoints
```

## Backend architecture: VSA + CQRS

The planned API uses **Vertical Slice Architecture (VSA)**: instead of grouping all controllers, services, and repositories into separate technical layers, code is organized around individual features and use cases.

**CQRS (Command Query Responsibility Segregation)** separates operations that change data (**commands**, such as `CreateBlueprint`) from operations that read data (**queries**, such as `GetBlueprint`). **MediatR** dispatches these requests to their respective handlers.

Typical request flow:

```text
HTTP endpoint → MediatR command/query → Handler → AppDbContext → PostgreSQL
```

Handlers use EF Core's `AppDbContext` **directly**. The initial design intentionally avoids generic repositories, extra service layers, and unnecessary abstractions.

## Development roadmap

1. **Milestone 1 — Local editor + persistence:** Interactive blueprint editor with entity placement/movement/rotation/deletion, IndexedDB autosave, and restoring saved work on return. Both editing **and** persistence are required for this milestone.
2. **Milestone 2 — Factorio compatibility:** Import, decode, edit, encode, and export Factorio blueprint strings.
3. **Milestone 3 — Cloud features:** ASP.NET Core API, PostgreSQL storage, accounts, and synchronization.
4. **Milestone 4 — Mobile/touch support:** and more advanced entity or blueprint functionality.

## Data model (planned for the API)

- **Account:** User account information.
- **Blueprint:** A saved blueprint and its metadata.
- **BlueprintEntity:** An entity placed at a position within a blueprint, including direction.
- **EntityDefinition:** Information about an available Factorio entity (such as name, category, and icon).
- **BlueprintBook:** A collection of blueprints.

These models describe the longer-term backend design; **the first milestone does not require accounts or a database server**.

## Current scope

The immediate priority is to prove the core editing experience in the React client. Authentication, PostgreSQL, cloud saving, and synchronization are intentionally deferred until the editor and local persistence work reliably, mobile app development will start after the core experience has been implemented.

---

*Unofficial fan project; not affiliated with or endorsed by Wube Software.*
