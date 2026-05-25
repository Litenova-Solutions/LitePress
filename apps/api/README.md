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
│   ├── LitePress.Infrastructure/       # EF Core, repositories
│   └── LitePress.WebApi/               # IEndpoint classes, middleware
├── tests/
│   ├── LitePress.Domain.Tests/
│   ├── LitePress.Application.Tests/
│   ├── LitePress.Architecture.Tests/
│   └── LitePress.Integration.Tests/
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

Migrations apply automatically in Development on API startup.

### Standalone (manual path)

Requires docker-compose Postgres (port 5433):

```bash
docker compose up -d
pnpm db:migrate
dotnet run --project src/LitePress.WebApi
# → http://localhost:5000
```

Environment:

```bash
ConnectionStrings__Database="Host=localhost;Port=5433;Database=litepress;Username=litepress;Password=litepress"
JwtSettings__Secret="dev-secret-key-must-be-at-least-32-characters-long!"
```

---

## Migrations

```bash
dotnet tool restore

# Apply (manual / CI path)
pnpm db:migrate

# Add
dotnet ef migrations add <Name> \
  --project src/LitePress.Infrastructure \
  --startup-project src/LitePress.WebApi
```

Aspire path: migrations run automatically in Development. See [local-dev-migrations ADR](../../docs/decisions/local-dev-migrations.md).

---

## Build and test

```bash
dotnet build LitePress.slnx --configuration Release
dotnet test LitePress.slnx --configuration Release --no-build
```

---

## Conventions

| Topic | Detail |
|:---|:---|
| Endpoints | `IEndpoint` only — no MVC controllers |
| Auth | JWT Bearer; `AuthorId` from JWT, never request body |
| Queries | `IDatabaseContext` projections; no repository injection |
| DB naming | `UseSnakeCaseNamingConvention()` via EFCore.NamingConventions |
| OpenAPI | `/openapi/v1.json` (machine-readable spec) |
| API docs (dev) | `/scalar/v1` (Scalar UI, Development only) |
| Health (dev) | `/health`, `/alive` via ServiceDefaults |
| Domain docs | `docs/domain/` — update with code changes |

Engineering standards: [standards/AGENTS.md](../../standards/AGENTS.md) (submodule).

---

## OpenAPI

When running locally:

| Resource | URL |
|:---|:---|
| OpenAPI JSON | http://localhost:5000/openapi/v1.json |
| Scalar UI (Development only) | http://localhost:5000/scalar/v1 |

The API uses `Microsoft.AspNetCore.OpenApi` for spec generation and `Scalar.AspNetCore` for the browsable reference UI. Scalar is mapped only when `ASPNETCORE_ENVIRONMENT=Development`. Production exposes the JSON endpoint only if you choose to map it; Scalar is not enabled in Production.

Regenerate TypeScript types from repo root:

```bash
pnpm generate:api-types
```
