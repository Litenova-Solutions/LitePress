# LitePress - Agent Context

<!-- Last updated: 2026-05-25 -->

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

Standards submodule tracks [`main`](https://github.com/Litenova-Solutions/Engineering-Standards) on Engineering-Standards. After clone, run `git submodule update --init --recursive`. To pull the latest standards: `git submodule update --remote standards`.

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
| Product name | **LitePress** |
| Solution file | `apps/api/LitePress.slnx` |
| Frontends | Two Next.js apps: `apps/web` (public) and `apps/admin` (authoring). Next.js 16.2.x, React 19.2.x, TypeScript 6.x. |
| Namespaces | `LitePress.Domain`, `LitePress.Application.*`, `LitePress.Infrastructure`, `LitePress.WebApi` |

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

- MUST use `LitePress.*` namespaces (not legacy `Blog.*` or `LiteNova.*` prefixes).
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
# Bootstrap (submodule, dotnet tools, pnpm install)
pwsh scripts/bootstrap.ps1

# Full stack (recommended)
pnpm dev:aspire

# Manual path
pwsh scripts/dev-manual.ps1   # Postgres + API
pnpm dev:web
pnpm dev:admin

# Frontends only (API + Postgres must already run)
pnpm dev

# Database
dotnet tool restore
pnpm db:migrate
dotnet ef migrations add {MigrationName} \
  --project apps/api/src/LitePress.Infrastructure \
  --startup-project apps/api/src/LitePress.WebApi

# Build / test
dotnet build apps/api/LitePress.slnx
dotnet test apps/api/LitePress.slnx

# Monorepo gates
pnpm lint && pnpm type-check && pnpm test && pnpm build

# Local E2E
pwsh scripts/e2e-local.ps1
```
