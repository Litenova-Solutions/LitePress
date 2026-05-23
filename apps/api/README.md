# LitePress — API

ASP.NET Core 10 Minimal API with Clean Architecture and CQRS (LiteBus).

Full reference: [docs/technical/api-reference.md](../../docs/technical/api-reference.md) · Architecture: [docs/technical/architecture.md](../../docs/technical/architecture.md)

---

## Solution layout

```
apps/api/
├── src/
│   ├── LiteNova.LitePress.AppHost/              # .NET Aspire orchestration
│   ├── LiteNova.LitePress.Domain/
│   ├── LiteNova.LitePress.Application.Write.Contracts/
│   ├── LiteNova.LitePress.Application.Write/
│   ├── LiteNova.LitePress.Application.Read.Contracts/
│   ├── LiteNova.LitePress.Application.Read/
│   ├── LiteNova.LitePress.Application.Reactions/
│   ├── LiteNova.LitePress.Infrastructure/       # EF Core, repositories
│   └── LiteNova.LitePress.WebApi/               # IEndpoint classes, middleware
├── tests/
│   ├── LiteNova.LitePress.Domain.Tests/
│   ├── LiteNova.LitePress.Application.Tests/
│   ├── LiteNova.LitePress.Architecture.Tests/
│   └── LiteNova.LitePress.Integration.Tests/
├── LiteNova.LitePress.slnx
├── Directory.Build.props
└── Directory.Packages.props
```

---

## Run

### With Aspire (recommended)

```bash
dotnet run --project src/LiteNova.LitePress.AppHost
```

### Standalone

Requires PostgreSQL (see root [README](../../README.md) or `docker compose up -d`):

```bash
dotnet run --project src/LiteNova.LitePress.WebApi
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
# Apply
dotnet ef database update \
  --project src/LiteNova.LitePress.Infrastructure \
  --startup-project src/LiteNova.LitePress.WebApi

# Add
dotnet ef migrations add <Name> \
  --project src/LiteNova.LitePress.Infrastructure \
  --startup-project src/LiteNova.LitePress.WebApi
```

---

## Build and test

```bash
dotnet build LiteNova.LitePress.slnx --configuration Release
dotnet test LiteNova.LitePress.slnx --configuration Release --no-build
```

---

## Conventions

| Topic | Detail |
|:---|:---|
| Endpoints | `IEndpoint` only — no MVC controllers |
| Auth | JWT Bearer; `AuthorId` from JWT, never request body |
| Queries | `IDatabaseContext` projections; no repository injection |
| DB naming | `UseSnakeCaseNamingConvention()` via EFCore.NamingConventions |
| OpenAPI | `/openapi/v1.json` |
| Domain docs | `docs/domain/` — update with code changes |

Engineering standards: [standards/AGENTS.md](../../standards/AGENTS.md) (submodule).

---

## OpenAPI

When running locally: http://localhost:5000/openapi/v1.json

Regenerate TypeScript types from repo root:

```bash
pnpm generate:api-types
```
