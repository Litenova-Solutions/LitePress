# Dual Next.js Apps

| Field | Value |
|:---|:---|
| Status | Accepted |
| Date | 2026-05-23 |

---

## Context

Engineering standards describe a typical monorepo with one or more frontends under `apps/`. LitePress separates public reading (SEO, anonymous) from authenticated authoring (TipTap, CRUD) into two apps today. Additional frontends MAY be added later under `apps/` without changing this decision.

---

## Decision

Run independent Next.js apps in the monorepo:

| App | Path | Purpose |
|:---|:---|:---|
| Web | `apps/web` | Public blog: lists, detail, tags, Giscus, SEO |
| Admin | `apps/admin` | Authenticated dashboard: post/tag CRUD, TipTap editor |

Each app follows `domain/{feature}/{use-case}/` layout independently. No app imports from another app's `domain/` folder.

**UI:** shadcn/ui is the default in both apps. Each app owns `components/ui/` (CLI-generated). Shared **CSS theme tokens** live in `@litepress/config-tailwind`; React components are not shared. A future app MAY override UI choices via its own ADR and README.

---

## Consequences

- CI runs separate workflows per frontend app.
- API types package (`packages/api-types`) is consumed by all frontends.
- Turbo pipeline builds every app under `apps/` that defines a build script.
- Playwright lives in `apps/web/e2e/` today; additional apps add their own E2E folders when needed.

---

## References

- `standards/docs/conventions/shared/monorepo-structure.md`
- `docs/domain/README.md`
- [scalar-api-docs.md](scalar-api-docs.md)
