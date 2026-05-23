# LitePress - Agent Context

<!-- Last updated: 2026-05-23 -->

This project (**LitePress**) follows the Litenova Solutions engineering standards.
Read `standards/AGENTS.md` before editing any code. Then read the convention file
for the layer you are editing. Then read the project-specific files listed below.

---

## Standards

Read order:

1. `standards/AGENTS.md` — canonical rules, tech stack, non-negotiable rules.
2. `standards/docs/architecture/clean-architecture.md` — layer diagram and responsibilities.
3. The convention file for the layer you are editing (see index in `standards/AGENTS.md`).
4. `standards/docs/conventions/shared/agentic-guardrails.md` — strict dependency lockdowns and scaffolding constraints.
5. `standards/docs/guides/agentic-domain-driven-design.md` — domain doc tree and frontend layout.
6. The project-specific files below for domain context.

Standards submodule pinned at `afcc8d0` (ADDD baseline).

---

## Project Tech Stack

The base tech stack is defined in `standards/AGENTS.md`. This table lists only
project-specific overrides or additions.

| Technology | Version / Notes |
|:---|:---|
| Authentication | Auth.js v5 — admin dashboard only. GitHub OAuth, JWT session. API access via minted JWT + api-proxy. |
| Rich Text Editor | TipTap (admin only). Stores ProseMirror JSON. |
| Comments | Giscus (web frontend, GitHub Discussions backed). |
| Database | PostgreSQL via EF Core with `UseSnakeCaseNamingConventions()`. |
| Solution file | `apps/api/LiteNova.Blog.slnx` |
| Product name | **LitePress** (public); .NET namespaces remain `LiteNova.Blog.*` until a future migration |
| Frontends | Two Next.js apps: `apps/web` (public) and `apps/admin` (authoring). Next.js 16.2.x, React 19.2.x, TypeScript 6.x. |
| Namespaces | `LiteNova.Blog.Domain`, `LiteNova.Blog.Application.*`, `LiteNova.Blog.Infrastructure`, `LiteNova.Blog.WebApi` |

---

## Project-Specific Context

Read these files before generating any domain or application code.

| File | Contents |
|:---|:---|
| [docs/README.md](docs/README.md) | Documentation index (non-technical + technical) |
| [docs/how-it-works.md](docs/how-it-works.md) | Plain-language product guide |
| [docs/technical/](docs/technical/) | Architecture, development, env, API reference |
| `docs/domain/README.md` | System map: all features and use cases. |
| `docs/domain/{feature}/README.md` | Feature ubiquitous language, aggregates, invariants, events. |
| `docs/domain/{feature}/{use-case}.md` | Use case contract: commands, endpoints, UI, acceptance criteria. |
| `docs/decisions/` | LitePress ADRs (auth, dual apps, SEO, licensing, deferrals). |

There are no separate inventory files. Domain docs are the source of truth.

---

## Non-Negotiable Project Rules

These rules extend `standards/AGENTS.md`. They do not replace any rule there.

- MUST use `LiteNova.Blog.*` namespaces (not `Blog.*`).
- MUST read `docs/domain/{feature}/README.md` before writing domain code for that feature.
- MUST read the use case doc at `docs/domain/{feature}/{use-case}.md` before implementing a use case.
- MUST derive `AuthorId` from the authenticated user's JWT claim. Never accept `AuthorId` from the request body.
- MUST NOT use the terms "Article", "Content", or "Entry" in place of "Post" in code, comments, or documentation.
- MUST NOT use the terms "Writer" or "Creator" in place of "Author" in code, comments, or documentation.
- MUST NOT use the term "Category" or "Label" in place of "Tag" in code, comments, or documentation.
- MUST place frontend feature code in `domain/{feature}/{use-case}/` in **each** app (`apps/web`, `apps/admin`) independently. No cross-app domain imports.
- MUST NOT edit any file under `standards/`. Changes to the standards belong in the standards repository.

---

## Commands

```bash
# Build
dotnet build apps/api/LiteNova.Blog.slnx

# Test
dotnet test apps/api/LiteNova.Blog.slnx

# Run via Aspire (PostgreSQL + API)
dotnet run --project apps/api/src/LiteNova.Blog.AppHost

# Run API directly
dotnet run --project apps/api/src/LiteNova.Blog.WebApi

# Add EF migration
dotnet ef migrations add {MigrationName} \
  --project apps/api/src/LiteNova.Blog.Infrastructure \
  --startup-project apps/api/src/LiteNova.Blog.WebApi

# Apply migration
dotnet ef database update \
  --project apps/api/src/LiteNova.Blog.Infrastructure \
  --startup-project apps/api/src/LiteNova.Blog.WebApi

# Frontend (admin)
pnpm --filter admin dev

# Frontend (web)
pnpm --filter web dev

# Monorepo gates
pnpm lint && pnpm type-check && pnpm test && pnpm build

# Bootstrap submodules after clone
pwsh scripts/bootstrap.ps1
```
