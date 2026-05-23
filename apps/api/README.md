# LitePress — API

ASP.NET Core 10 Minimal API with Clean Architecture and CQRS (LiteBus).

Full reference: [docs/technical/api-reference.md](../../docs/technical/api-reference.md) · Architecture: [docs/technical/architecture.md](../../docs/technical/architecture.md)

---

## Solution layout

```
apps/api/
├── src/
│   ├── LiteNova.Blog.AppHost/              # .NET Aspire orchestration
│   ├── LiteNova.Blog.Domain/
│   ├── LiteNova.Blog.Application.Write.Contracts/
│   ├── LiteNova.Blog.Application.Write/
│   ├── LiteNova.Blog.Application.Read.Contracts/
│   ├── LiteNova.Blog.Application.Read/
│   ├── LiteNova.Blog.Application.Reactions/
│   ├── LiteNova.Blog.Infrastructure/       # EF Core, repositories
│   └── LiteNova.Blog.WebApi/               # IEndpoint classes, middleware
├── tests/
│   ├── LiteNova.Blog.Domain.Tests/
│   ├── LiteNova.Blog.Application.Tests/
│   ├── LiteNova.Blog.Architecture.Tests/
│   └── LiteNova.Blog.Integration.Tests/
├── LiteNova.Blog.slnx
├── Directory.Build.props
└── Directory.Packages.props
```

---

## Run

### With Aspire (recommended)

```bash
dotnet run --project src/LiteNova.Blog.AppHost
```

### Standalone

Requires PostgreSQL (see root [README](../../README.md) or `docker compose up -d`):

```bash
dotnet run --project src/LiteNova.Blog.WebApi
# → http://localhost:5000
```

Environment:

```bash
ConnectionStrings__Database="Host=localhost;Port=5433;Database=blog;Username=blog;Password=blog"
JwtSettings__Secret="dev-secret-key-must-be-at-least-32-characters-long!"
```

---

## Migrations

```bash
# Apply
dotnet ef database update \
  --project src/LiteNova.Blog.Infrastructure \
  --startup-project src/LiteNova.Blog.WebApi

# Add
dotnet ef migrations add <Name> \
  --project src/LiteNova.Blog.Infrastructure \
  --startup-project src/LiteNova.Blog.WebApi
```

---

## Build and test

```bash
dotnet build LiteNova.Blog.slnx --configuration Release
dotnet test LiteNova.Blog.slnx --configuration Release --no-build
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
