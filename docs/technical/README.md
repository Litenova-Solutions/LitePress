# Technical Documentation

Technical reference for the LitePress monorepo. For a non-technical overview, see [How LitePress works](../how-it-works.md).

---

## Documents

| Document | Contents |
|:---|:---|
| [Architecture](architecture.md) | System design, clean architecture layers, request flows |
| [Development guide](development.md) | Clone, run, test, debug, CI |
| [Environment variables](environment.md) | Configuration for API, web, admin, production |
| [API reference](api-reference.md) | REST endpoints, auth, OpenAPI |

---

## Quick links

| Area | Path | README |
|:---|:---|:---|
| API (.NET) | `apps/api/` | [apps/api/README.md](../../apps/api/README.md) |
| Public web | `apps/web/` | [apps/web/README.md](../../apps/web/README.md) |
| Admin | `apps/admin/` | [apps/admin/README.md](../../apps/admin/README.md) |
| OpenAPI types | `packages/api-types/` | Generated from `/openapi/v1.json` |
| Domain docs | `docs/domain/` | [domain/README.md](../domain/README.md) |
| Standards | `standards/` submodule | [Engineering Standards](https://github.com/Litenova-Solutions/Engineering-Standards) |

---

## Stack summary

| Component | Version / tool |
|:---|:---|
| .NET | 10 (see `apps/api/global.json`) |
| Next.js | 16.2.x |
| React | 19.2.x |
| TypeScript | 6.x |
| PostgreSQL | 17 |
| ORM | EF Core 10 + `UseSnakeCaseNamingConvention()` |
| CQRS | LiteBus (`ICommandMediator` / `IQueryMediator`) |
| Front-end API client | `openapi-fetch` via `@litenova/api-client` |
| Monorepo | Turborepo + pnpm 10 |

---

## Namespaces

All .NET code uses the `LiteNova.LitePress.*` prefix:

- `LiteNova.LitePress.Domain`
- `LiteNova.LitePress.Application.Write` / `.Read` / `.Reactions`
- `LiteNova.LitePress.Infrastructure`
- `LiteNova.LitePress.WebApi`

Ubiquitous language: **Post**, **Tag**, **Author** (never Article, Category, Writer).

---

## Related

- [AGENTS.md](../../AGENTS.md) — agent and contributor contract
- [v1 release notes](../v1-release-notes.md) — shipped scope
- [Project decisions](../decisions/README.md) — LitePress ADRs
