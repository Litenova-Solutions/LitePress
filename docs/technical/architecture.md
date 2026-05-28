# Architecture

How LitePress is structured technically: multiple deployable apps under `apps/`, one primary API, one database, and shared TypeScript packages.

LitePress docs override engineering standards on overlapping topics. See [AGENTS.md](../../AGENTS.md#documentation-precedence).

---

## System overview

```mermaid
flowchart TB
  subgraph public [Public internet]
    Reader[Reader browser]
  end

  subgraph admin_user [Blog owner]
    Author[Author browser]
  end

  subgraph apps [Blog monorepo]
    Web[apps/web\nNext.js 16]
    Admin[apps/admin\nNext.js 16 + Auth.js]
    API[apps/api WebApi\nASP.NET Core 10]
    DB[(PostgreSQL)]
  end

  subgraph external [External services]
    GitHub[GitHub OAuth]
    Giscus[Giscus / GitHub Discussions]
  end

  Reader --> Web
  Author --> Admin
  Author --> GitHub
  Web -->|Server Components\ngetApiClient| API
  Admin -->|Server Components\ngetApiClient + JWT| API
  Admin -->|Client mutations\n/api-proxy + JWT| API
  API --> DB
  Reader --> Giscus
  Web -.->|optional embed| Giscus
```

---

## Deployable apps

LitePress currently ships two Next.js frontends and one API. The monorepo MAY add more apps under `apps/`; see [monorepo-structure in standards](../../standards/docs/conventions/shared/monorepo-structure.md) and [dual-nextjs-apps ADR](../decisions/dual-nextjs-apps.md).

| App | Port (dev) | Auth | Talks to API |
|:---|:---|:---|:---|
| `apps/web` | 3000 | None (public reads) | Server-side `getApiClient()` → `API_URL` |
| `apps/admin` | 3002 | Auth.js GitHub OAuth | Server: `getApiClient()` with minted JWT; client: `/api-proxy` |
| `apps/api` | 5000 | JWT Bearer on mutating routes | PostgreSQL via `PostgreSQL.Net` |

Each Next.js app is independent: own `features/{feature}/{use-case}/` tree, own `components/ui/` (shadcn), shared CSS tokens from `@litepress/config-tailwind`. No cross-app feature imports.

---

## API: clean architecture + CQRS

```
WebApi (IEndpoint)
    ↓ mediators
Application.Write / Application.Read / Application.Reactions
    ↓
Domain (aggregates, events, invariants)
    ↓
Infrastructure (PostgreSQL, repositories, pipeline)
```

| Project | Responsibility |
|:---|:---|
| `Domain` | `Post`, `Tag`, `Author` aggregates; domain events and exceptions |
| `Application.Write` | Command handlers (create, publish, archive, …) |
| `Application.Read` | Query handlers; projections via `IReadDatabase` (Marten storage schema) only |
| `Application.Reactions` | Event handlers (v1: log-only side effects) |
| `Infrastructure` | DbContext, repositories, naming conventions, DI |
| `WebApi` | Minimal API endpoints, JWT, CORS, OpenAPI, Scalar (Development) |

**Rules that matter:**

- Endpoints use `ICommandMediator` / `IQueryMediator`, not controllers.
- `AuthorId` comes from JWT `sub` claim only — never from request body.
- Query handlers never inject repositories; they project via `IReadDatabase`.
- No `SaveChangesAsync` in handlers; the command pipeline persists.

Full rules: [Engineering Standards — clean architecture](https://github.com/Litenova-Solutions/Engineering-Standards/blob/main/docs/architecture/clean-architecture.md).

---

## Publish flow (end to end)

```mermaid
sequenceDiagram
  participant A as Admin UI
  participant P as api-proxy
  participant API as Web API
  participant DB as PostgreSQL
  participant W as Public web

  A->>P: POST /api-proxy/posts/{id}/publish
  P->>P: Mint JWT from Auth.js session
  P->>API: POST /api/posts/{id}/publish (Bearer)
  API->>DB: Post.Publish() + save
  W->>API: GET /api/posts (public)
  API->>DB: Published posts only
  W->>W: Render home / slug page
```

The E2E CI workflow automates this path: API creates and publishes a post, then Playwright asserts it appears on the public site.

---

## Content format

Post bodies are stored as **ProseMirror JSON** (from TipTap in admin). The public web renders JSON to safe HTML server-side via `@tiptap/html`.

See [prosemirror-json-storage.md](../decisions/prosemirror-json-storage.md).

---

## Authentication model

| Layer | Mechanism |
|:---|:---|
| Admin sign-in | Auth.js v5 + GitHub OAuth; single owner via `GITHUB_OWNER_ID` |
| Admin → API | Short-lived HS256 JWT minted server-side (`API_JWT_SECRET` = API `JwtSettings:Secret`) |
| API mutating routes | `[Authorize]` + JWT validation |
| Author registration | `EnsureAuthorMiddleware` auto-registers author on first authenticated request |
| Public reads | No auth; API returns published content only |

See [admin-auth.md](../decisions/admin-auth.md).

---

## Shared TypeScript packages

| Package | Purpose |
|:---|:---|
| `@litepress/api-types` | OpenAPI-generated TypeScript types (`paths`, `components`) |
| `@litepress/api-client` | Re-export of `openapi-fetch` `createClient` |
| `@litepress/config-*` | Shared ESLint, TypeScript, Tailwind presets |

Regenerate types after API contract changes:

```bash
pnpm generate:api-types
```

Requires a running API or committed `packages/api-types/openapi.json`.

---

## Domain documentation (ADDD)

Feature behavior is documented under `docs/domain/` using Agentic Domain-Driven Design:

- Feature README: ubiquitous language, invariants, events
- Use-case doc: commands, endpoints, UI flows, acceptance criteria

When code and docs disagree, fix both in the same change.

Guide: [standards — agentic DDD](https://github.com/Litenova-Solutions/Engineering-Standards/blob/main/docs/guides/agentic-domain-driven-design.md).

---

## SEO (public web)

Server Components fetch data and emit `generateMetadata`, JSON-LD `BlogPosting`, `sitemap.xml`, and `robots.txt`.

See [seo-public-web.md](../decisions/seo-public-web.md).

---

## Out of scope (v1)

Scheduled publishing, R2 uploads, multi-author RBAC, outbox/worker, production VPS deploy scripts.

See [v1-scope-deferrals.md](../decisions/v1-scope-deferrals.md).
