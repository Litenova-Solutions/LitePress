# LitePress — API

ASP.NET Core 10 Minimal API with Clean Architecture and CQRS (LiteBus).

Full reference: [docs/technical/api-reference.md](../../docs/technical/api-reference.md) · Architecture: [docs/technical/architecture.md](../../docs/technical/architecture.md)

---

## Solution layout

```
apps/api/
├── src/
│   ├── LitePress.AppHost/              # .NET Aspire orchestration
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

```bash
dotnet run --project src/LitePress.AppHost
```

### Standalone

Requires PostgreSQL (see root [README](../../README.md) or `docker compose up -d`):

```bash
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
# Apply
dotnet ef database update \
  --project src/LitePress.Infrastructure \
  --startup-project src/LitePress.WebApi

# Add
dotnet ef migrations add <Name> \
  --project src/LitePress.Infrastructure \
  --startup-project src/LitePress.WebApi
```

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
