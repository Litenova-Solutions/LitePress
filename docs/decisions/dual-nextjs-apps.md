# Dual Next.js Apps

| Field | Value |
|:---|:---|
| Status | Accepted |
| Date | 2026-05-23 |

---

## Context

Standards assume a single `apps/web` frontend. The blog separates public reading (SEO, anonymous) from authenticated authoring (TipTap, CRUD).

---

## Decision

Run two independent Next.js apps in the monorepo:

| App | Path | Purpose |
|:---|:---|:---|
| Web | `apps/web` | Public blog: lists, detail, tags, Giscus, SEO |
| Admin | `apps/admin` | Authenticated dashboard: post/tag CRUD, TipTap editor |

Both apps follow `domain/{feature}/{use-case}/` layout independently. Neither app imports from the other's `domain/` folder. Shared UI lives per-app under `components/ui/` (shadcn CLI), not a workspace package.

---

## Consequences

- CI runs separate workflows for web and admin.
- API types package (`packages/api-types`) is consumed by both apps.
- Turbo pipeline builds both apps.

---

## References

- `standards/docs/adr/0011-turborepo-as-monorepo-tool.md`
- `docs/domain/README.md`
