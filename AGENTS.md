# Blog - Agent Context

<!-- Last updated: 2026-05-21 -->

This project follows the Litenova Solutions engineering standards.
Read `standards/AGENTS.md` before editing any code. Then read the convention file
for the layer you are editing. Then read the project-specific files listed below.

---

## Standards

Read order:

1. `standards/AGENTS.md` — canonical rules, tech stack, non-negotiable rules.
2. `standards/docs/architecture/clean-architecture.md` — layer diagram and responsibilities.
3. The convention file for the layer you are editing (see index in `standards/AGENTS.md`).
4. `standards/docs/conventions/shared/agentic-guardrails.md` — strict dependency lockdowns and scaffolding constraints.
5. The project-specific files below for domain context.

> **Note:** The `standards/` submodule has no published tags yet. Pin to the first
> release tag as soon as one is published: `cd standards && git checkout vX.Y.Z`.

---

## Project Tech Stack

The base tech stack is defined in `standards/AGENTS.md`. This table lists only
project-specific overrides or additions.

| Technology | Version / Notes |
|:---|:---|
| Authentication | Auth.js v5 — admin dashboard only. JWT session strategy. |
| Rich Text Editor | TipTap (admin only, post content authoring) |
| Comments | Giscus (web frontend, GitHub Discussions backed) |
| Database | PostgreSQL via EF Core with `UseSnakeCaseNamingConventions()` |
| Solution file | `apps/api/LiteNova.Blog.sln` |

---

## Project-Specific Context

Read these files before generating any domain or application code. They contain
the project's actual terms, aggregates, features, exceptions, and read models.

| File | Contents |
|:---|:---|
| `docs/domain/ubiquitous-language.md` | Glossary of domain terms and their code mappings. |
| `docs/domain/aggregate-inventory.md` | All aggregates, states, domain events, and repository interfaces. |
| `docs/domain/feature-inventory.md` | All implemented and planned use cases with handler class names. |
| `docs/domain/exception-inventory.md` | All custom exception types with categories and HTTP status codes. |
| `docs/domain/read-model-inventory.md` | `IDatabaseContext` properties, query handlers, and approved denormalized read models. |
| `docs/domain/frontend-feature-inventory.md` | All frontend routes and use cases (web + admin). |
| `docs/domain/frontend-api-endpoints.md` | Backend API endpoints consumed by the frontend. |
| `docs/adr/` | Project-specific architecture decisions. |

---

## Non-Negotiable Project Rules

These rules extend `standards/AGENTS.md`. They do not replace any rule there.

- MUST use `Blog` as the bounded context name in all namespaces: `Blog.Domain`, `Blog.Application.Write`, `Blog.Application.Read`, `Blog.Application.Reactions`, `Blog.Infrastructure`, `Blog.WebApi`.
- MUST read `docs/domain/ubiquitous-language.md` before writing any domain code.
- MUST read `docs/domain/aggregate-inventory.md` before creating or modifying aggregates or command handlers.
- MUST read `docs/domain/feature-inventory.md` before adding a new use case to avoid duplicates.
- MUST derive `AuthorId` from the authenticated user's JWT claim. Never accept `AuthorId` from the request body.
- MUST NOT use the terms "Article", "Content", or "Entry" in place of "Post" in code, comments, or documentation.
- MUST NOT use the terms "Writer" or "Creator" in place of "Author" in code, comments, or documentation.
- MUST NOT use the term "Category" or "Label" in place of "Tag" in code, comments, or documentation.
- MUST NOT edit any file under `standards/`. Changes to the standards belong in the standards repository.

---

## Commands

```bash
# Build
dotnet build apps/api/LiteNova.Blog.sln

# Test
dotnet test apps/api/LiteNova.Blog.sln

# Run API
dotnet run --project apps/api/src/LiteNova.Blog.Api

# Add EF migration
dotnet ef migrations add {MigrationName} \
  --project apps/api/src/LiteNova.Blog.Infrastructure \
  --startup-project apps/api/src/LiteNova.Blog.Api

# Apply migration
dotnet ef database update \
  --project apps/api/src/LiteNova.Blog.Infrastructure \
  --startup-project apps/api/src/LiteNova.Blog.Api

# Frontend (admin)
pnpm --filter admin dev

# Frontend (web)
pnpm --filter web dev

# Bootstrap submodules after clone
pwsh scripts/bootstrap.ps1
```
