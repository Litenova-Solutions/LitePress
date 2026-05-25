# Public web — shell

App: `apps/web` · Layout: [app/layout.tsx](../../../apps/web/app/layout.tsx)

Shared chrome wraps every public route. Page docs describe route-specific content inside `main`.

---

## Regions

| Region | Component | Role |
|:---|:---|:---|
| Header | [components/Header.tsx](../../../apps/web/components/Header.tsx) | Site title, nav (Posts, Tags) |
| Main | `app/**/page.tsx` → domain components | Route content; `flex-1` grows to fill viewport |
| Footer | [components/Footer.tsx](../../../apps/web/components/Footer.tsx) | Site links; sticks to bottom on short pages |

---

## Layout contract

| Rule | Verification |
|:---|:---|
| On short pages, footer bottom edge aligns with viewport bottom | [e2e/layout.spec.ts](../../../apps/web/e2e/layout.spec.ts) |
| On long pages, footer sits below main content | [e2e/layout.spec.ts](../../../apps/web/e2e/layout.spec.ts) |
| Content column max width `max-w-5xl`, centered | Visual / code review |

Implementation: `body` uses column flex with minimum viewport height; `main` uses `flex-1`; footer uses `mt-auto`.

---

## Presentation defaults

| Concern | Source |
|:---|:---|
| Component library | shadcn/ui in `components/ui/` |
| Theme tokens | `@litepress/config-tailwind/theme.css` |
| Typography | Geist via `app/layout.tsx` |
| Article prose | `prose` classes on post body ([view-post-by-slug](../../domain/posts/view-post-by-slug.md)) |

See [dual-nextjs-apps ADR](../../decisions/dual-nextjs-apps.md) and `standards/docs/conventions/frontend/02-components.md`.

---

## Related

- [SEO policy](../../decisions/seo-public-web.md) — metadata and semantic HTML on pages inside this shell
