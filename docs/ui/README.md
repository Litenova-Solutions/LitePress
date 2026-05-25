# UI projection docs

| Field | Value |
|:---|:---|
| Purpose | Shell and page composition for each frontend app |
| Last updated | 2026-05-25 |

UI projection docs describe **how behavior appears on screen**. They do not define business rules. Rules live in [domain docs](../domain/README.md); operations live in use-case docs.

---

## Documentation layers

| Layer | Path | Question it answers | Translates to |
|:---|:---|:---|:---|
| **Domain** | `docs/domain/{feature}/README.md` | What are the invariants and aggregates? | Domain entities, exceptions |
| **Use case** | `docs/domain/{feature}/{use-case}.md` | What is one operation and how is it verified? | Handlers, endpoints, unit/integration tests |
| **UI projection** | `docs/ui/{app}/` | How is the app framed and which operations compose on each page? | Layout, routes, domain components, Playwright |
| **Runbook** | `apps/{app}/README.md` | How do I run, configure, and build the app? | Env, scripts, CI |

Use cases and pages are **many-to-many**. One page may invoke several use cases (admin post editor). One use case may appear on several pages (list published posts on `/` and `/tags/[slug]`). UI projection docs capture that mapping without duplicating domain rules.

---

## Tree

```text
docs/ui/
├── README.md                 # This file
├── web/                      # apps/web (public)
│   ├── README.md             # Route index
│   ├── shell.md              # Shared layout chrome
│   └── pages/                # One file per user-facing route
└── admin/                    # apps/admin (authoring)
    ├── README.md
    ├── shell.md
    └── pages/
```

---

## What belongs here

| Include | Exclude |
|:---|:---|
| Route, app shell, layout regions | Invariants ("published post cannot be deleted") |
| Which use-case docs compose on a page | Command/query field lists (link instead) |
| Visible states on this screen | HTTP status code tables |
| Links to domain modules and e2e tests | Tailwind class names, shadcn variant names |
| Presentation defaults at app level (in `shell.md`) | Duplicate acceptance criteria (link to use-case doc) |

---

## Agent read order (UI work)

1. [docs/domain/{feature}/README.md](../domain/README.md) — invariants and language.
2. [docs/domain/{feature}/{use-case}.md](../domain/README.md) — operation behavior for each use case on the page.
3. `docs/ui/{app}/shell.md` — layout chrome when touching shared layout.
4. `docs/ui/{app}/pages/{page}.md` — page composition for the route you are editing.
5. `apps/{app}/README.md` — run, env, build only.
6. `standards/docs/conventions/frontend/` — framework defaults.

Update UI projection docs in the same PR as route, layout, or page composition changes.

---

## Apps

| App | UI docs | Runbook |
|:---|:---|:---|
| Public web | [ui/web/](web/README.md) | [apps/web/README.md](../../apps/web/README.md) |
| Admin | [ui/admin/](admin/README.md) | [apps/admin/README.md](../../apps/admin/README.md) |

---

## Related

- [Domain map](../domain/README.md)
- [dual-nextjs-apps ADR](../decisions/dual-nextjs-apps.md)
- [seo-public-web ADR](../decisions/seo-public-web.md) — SEO policy for public routes
- Standards: `standards/docs/guides/agentic-domain-driven-design.md` § UI projection
