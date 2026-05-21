# LiteNova Blog — API

ASP.NET Core 10 Minimal API following Clean Architecture with CQRS via LiteBus.

## Solution Layout

```
apps/api/
├── src/
│   ├── LiteNova.Blog.AppHost/           # .NET Aspire orchestration entry point
│   ├── LiteNova.Blog.Domain/            # Aggregates, value objects, domain events
│   ├── LiteNova.Blog.Application.Write.Contracts/   # Command/result DTOs
│   ├── LiteNova.Blog.Application.Write/             # Command handlers (LiteBus)
│   ├── LiteNova.Blog.Application.Read.Contracts/    # Query/result DTOs
│   ├── LiteNova.Blog.Application.Read/              # Query handlers (LiteBus)
│   ├── LiteNova.Blog.Application.Reactions/         # Event handlers (LiteBus)
│   ├── LiteNova.Blog.Infrastructure/                # EF Core, repositories
│   └── LiteNova.Blog.WebApi/                        # Minimal API endpoints, middleware
├── LiteNova.Blog.slnx    # SDK-style solution file
├── Directory.Build.props # Shared build properties (net10.0, nullable, etc.)
└── Directory.Packages.props  # Central NuGet package versions
```

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) — for PostgreSQL (via Aspire or manual)
- [.NET Aspire workload](https://learn.microsoft.com/en-us/dotnet/aspire/fundamentals/setup-tooling): `dotnet workload install aspire`

## Running the API

### With Aspire (recommended)

From the repo root or `apps/api/`:

```bash
dotnet run --project src/LiteNova.Blog.AppHost
```

Aspire starts PostgreSQL automatically and injects the connection string. The Aspire dashboard at `https://localhost:15888` shows all resource URLs and live logs.

### Standalone (without Aspire)

Ensure PostgreSQL is running (see root README), then:

```bash
cd apps/api
dotnet run --project src/LiteNova.Blog.WebApi
# → http://localhost:5000
```

Required environment variables when running standalone (can also go in `appsettings.Development.json`):

```bash
ConnectionStrings__Database="Host=localhost;Port=5433;Database=blog;Username=blog;Password=blog"
JwtSettings__Secret="dev-secret-key-must-be-at-least-32-characters-long!"
```

## Debugging in VS Code

1. Open the repo root folder in VS Code.
2. Install [C# Dev Kit](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csdevkit).
3. Open the **Run and Debug** panel (`Ctrl+Shift+D`).
4. Select the `LiteNova.Blog.WebApi` launch configuration and press **F5**.

Alternatively, launch the **AppHost** launch configuration to debug via Aspire with all services attached.

## Database Migrations

EF Core migrations are in `src/LiteNova.Blog.Infrastructure/Migrations/`.

### Apply existing migrations

```bash
cd apps/api

# Against locally running PostgreSQL (port 5433):
dotnet ef database update \
  --project src/LiteNova.Blog.Infrastructure \
  --startup-project src/LiteNova.Blog.WebApi

# Connection string override (if port differs):
ConnectionStrings__Database="Host=localhost;Port=5433;Database=blog;Username=blog;Password=blog" \
dotnet ef database update \
  --project src/LiteNova.Blog.Infrastructure \
  --startup-project src/LiteNova.Blog.WebApi
```

### Add a new migration

```bash
cd apps/api
dotnet ef migrations add <MigrationName> \
  --project src/LiteNova.Blog.Infrastructure \
  --startup-project src/LiteNova.Blog.WebApi \
  --output-dir Migrations
```

### Rollback

```bash
dotnet ef database update <PreviousMigrationName> \
  --project src/LiteNova.Blog.Infrastructure \
  --startup-project src/LiteNova.Blog.WebApi
```

## Building

```bash
cd apps/api
dotnet build LiteNova.Blog.slnx
```

## Key Conventions

| Convention | Detail |
|-----------|--------|
| Domain events | Plain C# records — no framework interface (`IDomainEvent` is a pure marker) |
| Event dispatch | `IEventPublisher.PublishAsync((dynamic)domainEvent)` — DLR resolves concrete type |
| Connection string key | `"Database"` (maps to `ConnectionStrings__Database` env var) |
| Snake_case columns | Manual conversion in `BlogDbContext.OnModelCreating` |
| Auth | JWT Bearer — claims carry `AuthorId`; never read from request body |

## API Endpoints

| Method | Path | Auth | Description |
|--------|------|------|-------------|
| `GET` | `/api/posts` | Public | List published posts (paginated) |
| `GET` | `/api/posts/{slug}` | Public | Get published post by slug |
| `GET` | `/api/tags` | Public | List all tags with post counts |
| `GET` | `/api/tags/{slug}/posts` | Public | Posts for a tag |
| `POST` | `/api/posts` | Bearer | Create a draft post |
| `PUT` | `/api/posts/{id}` | Bearer | Update a post |
| `POST` | `/api/posts/{id}/publish` | Bearer | Publish a post |
| `DELETE` | `/api/posts/{id}` | Bearer | Delete a post |

Full OpenAPI spec available at `http://localhost:5000/openapi/v1.json` when running.
