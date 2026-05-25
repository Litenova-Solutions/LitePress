# UI projection docs

| Field | Value |
|:---|:---|
| Purpose | Shell and page composition for each frontend app |
| Last updated | 2026-05-25 |

UI projection docs describe **how behavior appears on screen**. They do not define business rules. Rules live in [domain docs](../domain/README.md); operations live in use-case docs.

LitePress has two separate frontend apps (`apps/web`, `apps/admin`) with separate UI doc trees under `docs/ui/web/` and `docs/ui/admin/`. Do not mix routes, feature modules, or page docs across apps.

---

## Documentation layers

| Layer | Path | Question it answers | Translates to |
|:---|:---|:---|:---|
| **Domain** | `docs/domain/{feature}/README.md` | What are the invariants and aggregates? | Domain entities, exceptions |
| **Use case** | `docs/domain/{feature}/{use-case}.md` | What is one operation and how is it verified? | Handlers, endpoints, unit/integration tests |
| **UI projection** | `docs/ui/{app}/` | How is the app framed and which operations compose on each page? | Layout, routes, feature components, Playwright |
| **Runbook** | `apps/{app}/README.md` | How do I run, configure, and build the app? | Env, scripts, CI |

Use cases and pages are **many-to-many**. One page may invoke several use cases (admin post editor). One use case may appear on several pages (list published posts on web home and tag routes). UI projection docs capture that mapping without duplicating domain rules.

---

## Tree

```text
docs/ui/
├── README.md                 # This file
├── web/                      # apps/web (public) only
│   ├── README.md             # Route index
│   ├── shell.md              # Shared layout chrome
│   └── pages/                # One file per user-facing route
└── admin/                    # apps/admin (authoring) only
    ├── README.md
    ├── shell.md
    └── pages/
```

---

## Page doc file naming

Name page docs after the route slug or a short role suffix when one route composes many operations:

| Pattern | Example file | Route |
|:---|:---|:---|
| Route slug | `posts-by-tag.md` | `/tags/[slug]` |
| Role suffix | `post-editor.md` | `/posts/[id]` (many use cases) |
| Index route | `tags-index.md` | `/tags` (web) vs `tags.md` (admin) |

Each app keeps its own `pages/` folder. The same route shape in two apps (for example tag management) still gets separate page docs under `docs/ui/web/` and `docs/ui/admin/`.

---

## Code alignment

| Layer | Path | Example |
|:---|:---|:---|
| Domain policy | `docs/domain/{feature}/{use-case}.md` | `docs/domain/posts/create-post.md` |
| Backend | `apps/api/.../{Feature}/{UseCase}/` | `Posts/Create/` |
| Frontend UI (per app) | `apps/{app}/features/{feature}/{use-case}/` | `apps/admin/features/posts/create/` |
| Route shell | `apps/{app}/app/.../page.tsx` | Thin import of feature entry |

`docs/domain/` holds business rules. `features/` holds presentation code. Do not confuse the two.

---

## Templates and examples

Copy from the **standards submodule** (not from LitePress `docs/`):

| Artifact | Path |
|:---|:---|
| Page template | [standards/docs/templates/ui-page.md](../../standards/docs/templates/ui-page.md) |
| Shell template | [standards/docs/templates/ui-shell.md](../../standards/docs/templates/ui-shell.md) |
| Page example (generic) | [standards/docs/templates/ui-page.example.md](../../standards/docs/templates/ui-page.example.md) |

LitePress-approved multi-use-case page doc: [admin/pages/post-editor.md](admin/pages/post-editor.md).

---

## What belongs here

| Include | Exclude |
|:---|:---|
| Route, app shell, layout regions | Invariants ("published post cannot be deleted") |
| Which use-case docs compose on a page | Command/query field lists (link instead) |
| Screen states and content modes on this page | HTTP status code tables |
| Links to feature modules and e2e tests | Tailwind class names, shadcn variant names |
| Presentation defaults at app level (in `shell.md`) | Duplicate acceptance criteria (link to use-case doc) |

Use-case docs hold **operation states** (loading, empty, error for a query or mutation). Page docs hold **screen states** (what the user sees on this route) and **content modes** when aggregate state changes visible actions (for example Draft vs Published on the post editor).

---

## Agent read order (UI work)

1. [docs/domain/{feature}/README.md](../domain/README.md) — invariants and language.
2. [docs/domain/{feature}/{use-case}.md](../domain/README.md) — operation behavior for each use case on the page.
3. `docs/ui/{app}/shell.md` — layout chrome when touching shared layout (`web` or `admin`, not both unless the change applies to both apps).
4. `docs/ui/{app}/pages/{page}.md` — page composition for the route you are editing.
5. `apps/{app}/README.md` — run, env, build only.
6. `standards/docs/conventions/frontend/` — framework defaults.

Update UI projection docs in the same PR as route, layout, or page composition changes. Update the matching use-case doc § UI projection with links back to the page doc.

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
- Standards: [standards/docs/guides/agentic-domain-driven-design.md](../../standards/docs/guides/agentic-domain-driven-design.md) § UI projection
