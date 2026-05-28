# LitePress — API

ASP.NET Core 10 Minimal API with Clean Architecture and CQRS (LiteBus).

Full reference: [docs/technical/api-reference.md](../../docs/technical/api-reference.md) · Architecture: [docs/technical/architecture.md](../../docs/technical/architecture.md)

---

## Solution layout

```
apps/api/
├── src/
│   ├── LitePress.AppHost/              # .NET Aspire orchestration
│   ├── LitePress.ServiceDefaults/      # OTel, health checks, service discovery
│   ├── LitePress.Domain/
│   ├── LitePress.Application.Write.Contracts/
│   ├── LitePress.Application.Write/
│   ├── LitePress.Application.Read.Contracts/
│   ├── LitePress.Application.Read/
│   ├── LitePress.Application.Reactions/
│   ├── LitePress.Infrastructure/       # Marten, repositories
│   └── LitePress.WebApi/               # IEndpoint classes, middleware
├── tests/
│   ├── LitePress.Domain.Tests/
│   ├── LitePress.Application.Tests/
│   ├── LitePress.Architecture.Tests/
│   ├── LitePress.Integration.Tests/
│   └── LitePress.AcceptanceTests/      # Reqnroll BDD; maps to docs/domain use cases
├── LitePress.slnx
├── Directory.Build.props
└── Directory.Packages.props
```

---

## Run

### With Aspire (recommended)

From repo root:

```bash
pnpm dev:aspire
```

Or from `apps/api`:

```bash
dotnet run --project src/LitePress.AppHost
```

Marten storage schema applies automatically in Development on API startup.

### Standalone (manual path)

Requires docker-compose PostgreSQL (port 5432):

```bash
docker compose up -d
pnpm db:migrate
dotnet run --project src/LitePress.WebApi
# → http://localhost:5000
```

Environment:

```bash
ConnectionStrings__DefaultConnection="Host=127.0.0.1;Port=5432;Database=litepress;Username=litepress;Password=litepress"
JwtSettings__Secret="dev-secret-key-must-be-at-least-32-characters-long!"
```

---

## Database schema

Serialization wiring lives in `src/LitePress.Infrastructure/Marten/Serialization/` (`Abstractions/` for contracts and bases, `Internal/Aggregates/` for per-aggregate configuration).

```bash
# Apply (manual / CI path)
pnpm db:migrate

# Apply only (no HTTP server)
dotnet run --project src/LitePress.WebApi -- --apply-schema-only
```

Aspire path: schema runs automatically in Development. See [local-dev-migrations ADR](../../docs/decisions/local-dev-migrations.md) and [martendb-persistence ADR](../../docs/decisions/martendb-persistence.md).

---

## Build and test

```bash
dotnet build LitePress.slnx --configuration Release
dotnet test LitePress.slnx --configuration Release --no-build
```

Integration and acceptance tests require Docker (PostgreSQL Testcontainers).

```bash
# Critical acceptance scenarios only
dotnet test tests/LitePress.AcceptanceTests --filter "Category=critical"
```

Feature file validation:

```bash
pwsh ../../scripts/validate-feature-files.ps1
```

---

## Conventions

| Topic | Detail |
|:---|:---|
| Endpoints | `IEndpoint` only — no MVC controllers |
| Auth | JWT Bearer; `AuthorId` from JWT, never request body |
| Queries | `IReadDatabase` LINQ over Marten documents; no repository injection |
| Persistence | Marten 9 on PostgreSQL; domain aggregate roots stored as JSON |
| OpenAPI | `/openapi/v1.json` (machine-readable spec) |
| API docs (dev) | `/scalar/v1` (Scalar UI, Development only) |
| Health (dev) | `/health`, `/alive` via ServiceDefaults |
| Domain docs | `docs/domain/` — update with code changes |

Engineering standards: [standards/AGENTS.md](../../standards/AGENTS.md) (submodule). LitePress overrides the default EF Core stack; see [martendb-persistence ADR](../../docs/decisions/martendb-persistence.md).

---

## OpenAPI

When running locally:

- Spec: `http://localhost:5000/openapi/v1.json`
- Scalar UI (Development): `http://localhost:5000/scalar/v1`

Regenerate TypeScript types from repo root:

```bash
pnpm generate:api-types
```
