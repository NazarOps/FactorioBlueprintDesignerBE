# Factorio Blueprint Designer API — Coding Standards & Git Workflow

**Scope:** Backend repository only (`FactorioBlueprintDesignerAPI.csproj`)  
**Stack:** C# / .NET 10, ASP.NET Core, EF Core, PostgreSQL, Vertical Slice Architecture (VSA), CQRS with MediatR, FluentValidation, Result pattern, Carter (where used).  
**Status:** Aligned with the supplied November 2026 GitHub Projects timeline and Board screenshots, including verified workflow statuses.

> **Project board:** https://github.com/users/NazarOps/projects/10  
> **Verification note:** The supplied screenshots show the timeline (issues #4–#11) and the actual Board status columns: **Todo**, **In Progress**, **In Review**, **Done**. Other custom fields have not been verified. Issue titles are abbreviated where screenshots truncate them.

## 1. Architecture

- Organize API code **by feature**, not by generic `Controllers/Services/Repositories` layers.
- Use **Vertical Slice Architecture**: keep each feature's endpoint, command/query, validation, and handler together where practical.
- Apply **CQRS**: commands change state; queries read state.
- Use **MediatR** to dispatch commands and queries.
- Inject `AppDbContext` **directly into handlers**. Do not introduce repository or generic repository abstractions without a concrete need.
- Use **FluentValidation** for input validation and the project's **Result pattern** for expected success/failure outcomes.
- Keep business/domain models separate from API request/response contracts and Factorio import/export DTOs.
- Do not add abstractions, interfaces, or services solely for hypothetical future requirements.

### Suggested layout

```text
Database/
  AppDbContext.cs
Entities/
  Account.cs
  Blueprint.cs
  BlueprintBook.cs
  BlueprintEntity.cs
  EntityDefinition.cs
  Enum/
Features/
  Blueprints/
    CreateBlueprint.cs
    GetBlueprint.cs
  Accounts/
Shared/
  Result.cs
  Errors/
Extensions/                 # Only if extension methods are needed
.github/
  workflows/
  PULL_REQUEST_TEMPLATE.md  # Optional
.editorconfig
```

Feature filenames are examples, not a statement that these slices already exist. Use the established spelling `Extensions` rather than `Extentions` for any new folder.

## 2. C# naming and style

| Element | Convention | Example |
|---|---|---|
| Namespace, class, record, enum, method, property | `PascalCase` | `CreateBlueprint`, `BlueprintId` |
| Local variable, parameter | `camelCase` | `blueprintId` |
| Private instance field | `_camelCase` | `_dbContext` |
| Interface | `I` + PascalCase | `IValidator<T>` |
| Async method | `Async` suffix | `GetBlueprintAsync` |
| Constant | `PascalCase` | `MaximumNameLength` |
| Feature folder | Plural domain name | `Features/Blueprints/` |
| Feature file | Action + domain entity | `CreateBlueprint.cs` |

- Enable nullable reference types and implicit usings (already configured in the project).
- Prefer clear names over abbreviations. Keep methods focused and small enough to understand.
- Use `required` for properties that must be supplied by object initialization when appropriate; use nullable types only when `null` is meaningful.
- Avoid `null!` as a way to bypass missing validation; it is acceptable for EF Core navigation properties when the relationship guarantees initialization after loading.
- Prefer asynchronous EF Core operations (`SaveChangesAsync`, `FirstOrDefaultAsync`, etc.) and propagate `CancellationToken`.
- Use consistent braces, indentation (4 spaces), and formatting enforced by `.editorconfig` / `dotnet format`.
- Avoid committing commented-out code, unused imports, debug prints, or secrets.

## 3. API and data rules

- Keep endpoint paths resource-oriented and consistent; use appropriate HTTP verbs and status codes.
- Validate incoming commands before persistence. Return predictable validation and domain errors using the established Result pattern.
- Never return EF Core entities directly as public API contracts when that exposes internal fields or navigation graphs.
- Use explicit DTOs for Factorio blueprint JSON and string serialization. Do not assume the internal database model matches Factorio's schema.
- Treat Factorio import input as untrusted: reject malformed input, bound decompressed payload size, and provide understandable errors.
- For V1 Factorio import, unsupported entity types may be skipped **with a visible warning**; never silently discard them.
- Do not log passwords, connection strings, access tokens, or sensitive user data.
- Keep development connection strings in `appsettings.Development.json` or user secrets; never commit real credentials.
- Use EF Core migrations for schema changes. Commit migration files together with the related model changes.
- Use the project's agreed primary/foreign key types consistently; verify relationships and migration output when changing identifiers.

## 4. Git branches

**Recommended branches:**

- `main` — stable, merge-ready code (if the repository currently uses `master`, keep `master` until an intentional rename).
- Short-lived task branches created from the current default branch.
- No permanent `develop` branch is required for this small backend project.

**Branch naming format:**

`<type>/<issue-number>-<short-kebab-case-description>`

| Type | Purpose | Example |
|---|---|---|
| `feat/` | New feature | `feat/9-account-authentication` |
| `fix/` | Bug fix | `fix/18-correct-db-connection` |
| `refactor/` | Restructure without behavior change | `refactor/21-simplify-blueprint-handler` |
| `test/` | Add or improve tests | `test/7-api-postgres-integration` |
| `docs/` | Documentation | `docs/4-offline-first-progress-tracking` |
| `chore/` | Dependencies, tooling, maintenance | `chore/6-local-postgres-setup` |
| `ci/` | CI workflow changes | `ci/10-api-pr-build-test` |

**Important:** Numbers above are illustrative. Use the **actual linked GitHub Issue number** when one exists; GitHub Projects draft items do not automatically have issue numbers. If a board item has no issue, create/link an issue or use `<type>/<short-description>` (e.g., `docs/coding-standards`).

Branch names must be lowercase, use hyphens, and avoid spaces or special characters.

## 5. Commits

Use **Conventional Commits**:

`<type>(<optional-scope>): <imperative summary>`

Examples:

- `feat(blueprints): add create blueprint command`
- `fix(database): correct connection string lookup`
- `test(blueprints): cover empty blueprint name`
- `docs: add backend coding standards`
- `ci: verify dotnet formatting on pull requests`

Keep commits focused. Do not mix unrelated refactoring with feature work unless necessary.

## 6. GitHub Project #10 — visible backend tasks

The November 2026 **timeline screenshot** shows these existing issues. Use their real issue numbers in branch names and PR references rather than creating duplicate tickets. Titles ending in `…` are truncated in the screenshot; consult the issue itself for the complete title and acceptance criteria.

| Issue | Visible task title (abbreviated where needed) | Suggested branch |
|---|---|---|
| #4 | `docs: align progress tracking with the offline-first road…` | `docs/4-offline-first-progress-tracking` |
| #5 | `design: define the cloud blueprint contract and revisio…` | `docs/5-cloud-blueprint-contract` |
| #6 | `chore: document and verify repeatable local PostgreS…` | `chore/6-local-postgres-setup` |
| #7 | `test: establish API and PostgreSQL integration test infr…` | `test/7-api-postgres-integration` |
| #8 | `feat: add request validation and consistent API error re…` | `feat/8-validation-api-errors` |
| #9 | `feat: implement account registration, login, and owner…` | `feat/9-account-authentication` |
| #10 | `ci: build and test the API on pull requests` | `ci/10-api-pr-build-test` |
| #11 | `feat: provide an entity-definition catalog for cloud bl…` | `feat/11-entity-definition-catalog` |

These branches are **suggested names**, not names verified in GitHub. The screenshots confirm the timeline and the four board statuses, but do **not** confirm Priority, Size, or Milestone field values. Keep frontend IndexedDB work independent of these backend tasks. Cloud APIs and authentication can be developed in the backend without making them prerequisites for the guest editor's first milestone.

## 7. GitHub Projects workflow — verified board statuses

The supplied Board screenshot confirms **four** status columns, in this order. Use these exact names; do not introduce a separate Backlog or Ready status unless the board is intentionally changed.

| Actual board status | When to use it | Expected action |
|---|---|---|
| **Todo** | Issue is identified but work has not started | Confirm scope and acceptance criteria; choose the next issue |
| **In Progress** | Active implementation or documentation work has started | Create a branch tied to the issue, implement, and commit |
| **In Review** | A pull request is open and ready for review/CI checks | Review changes, confirm CI and acceptance criteria |
| **Done** | Changes have been merged and verified | Close the issue and confirm project item completion |

**Transitions:** `Todo → In Progress → In Review → Done`. If review identifies changes, move back to **In Progress** and return to **In Review** when ready. For documentation-only work, still use the same statuses and review process.

**Snapshot from the screenshot:** Todo **15**, In Progress **0**, In Review **0**, Done **0**. These are observed counts, not permanent targets.

**Issue/task minimum:** descriptive title; backend scope; expected behavior; acceptance criteria; relevant milestone; and links to dependencies when needed. Use the board's existing priority/size/iteration fields if they exist rather than inventing duplicates.

**Suggested issue title:** `API: Create blueprint endpoint`  
**Suggested PR title:** `feat(blueprints): create blueprint endpoint`

For work that is not part of the backend (React editor, IndexedDB), track it in the appropriate frontend repository and link cross-repository issues where relevant. **Backend cloud account/save APIs are later-phase scope**, so backend implementation does not block the V1 guest editor.

## 8. Pull requests and reviews

1. Select an actionable issue on the project board.
2. Create a short-lived branch from the default branch using the naming convention.
3. Implement the smallest complete slice, with validation and tests appropriate to the change.
4. Run formatting, build, and tests locally.
5. Push and open a PR targeting the default branch. Link the issue (e.g., `Closes #12` when appropriate).
6. Confirm CI is green, review the diff, and merge. Update/close the project item.

**PR description checklist:**

- [ ] Linked issue/project item and short summary
- [ ] Scope and acceptance criteria addressed
- [ ] `dotnet format ... --verify-no-changes --no-restore` passes
- [ ] `dotnet build ... --configuration Release` passes
- [ ] `dotnet test` passes when tests exist
- [ ] Validation and expected errors covered where relevant
- [ ] EF Core migration included and reviewed if schema changed
- [ ] No secrets, debug output, or unrelated changes
- [ ] Breaking API or database changes documented

A solo developer may self-review; CI should still pass before merge. Prefer squash merging to keep the default branch history readable.

## 9. Testing

- **Unit tests:** validation and isolated business behavior.
- **Integration tests:** request/handler/database interactions and real persistence behavior; use a suitable test database when behavior depends on PostgreSQL.
- Test successful paths, invalid input, not-found results, and relevant failure paths.
- For import/export, include malformed strings, unsupported entities, and round-trip cases.
- Add a regression test for each bug when practical.
- Don't substitute FluentValidation rules for tests: tests verify that rules and end-to-end behavior work as intended.

## 10. CI requirements

The backend CI workflow should, at minimum:

1. Restore the .NET 10 project dependencies.
2. Verify code formatting with `dotnet format`.
3. Build the API in Release configuration.
4. Run tests when a test project exists.

Use the correct project filename: **`FactorioBlueprintDesignerAPI.csproj`**. CI should trigger for pull requests to the default branch and relevant backend changes. Branch protection is recommended when practical.

## 11. Definition of Done

A backend board item is **Done** when:

- Its acceptance criteria are met.
- Code follows the VSA/CQRS and naming conventions above.
- Validation, expected errors, and relevant tests are addressed.
- CI passes and the PR is merged into the default branch.
- Required migrations and documentation are included.
- The linked GitHub Project item reflects completion.

---

**Verified:** Board status names and order, plus the visible issue numbers and truncated titles. **Not verified:** any other custom fields (such as Priority, Size, Iteration, or Milestone).
