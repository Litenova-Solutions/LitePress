# Admin — shell

| Field | Value |
|:---|:---|
| App | `apps/admin` |
| Layouts | [app/layout.tsx](../../../apps/admin/app/layout.tsx), [app/(dashboard)/layout.tsx](../../../apps/admin/app/(dashboard)/layout.tsx) |
| Last updated | 2026-05-25 |

Shared chrome wraps authenticated dashboard routes. Page docs describe route-specific content inside `main`.

---

## Regions

| Region | Component | Role |
|:---|:---|:---|
| Root layout | [app/layout.tsx](../../../apps/admin/app/layout.tsx) | Theme, fonts, global toasts (`Toaster`) |
| Auth layout | [app/(auth)/login/page.tsx](../../../apps/admin/app/(auth)/login/page.tsx) | Centered sign-in card; no sidebar |
| Sidebar | [Sidebar.tsx](../../../apps/admin/components/Sidebar.tsx) | Nav links to Dashboard, Posts, Tags; active route highlight via `usePathname` |
| Main | `(dashboard)/**/page.tsx` → domain components or inline content | Scrollable content area with padding |

---

## Layout contract

| Rule | Verification |
|:---|:---|
| Sidebar fixed width; main fills remaining viewport width | Code review |
| Unauthenticated access to `(dashboard)` routes redirects to `/login` | [admin-auth ADR](../../decisions/admin-auth.md) |
| Active nav item matches current pathname (including nested routes under `/posts` and `/tags`) | Code review of [Sidebar.tsx](../../../apps/admin/components/Sidebar.tsx) |

Implementation: `(dashboard)/layout.tsx` uses flex row with `min-h-screen`; sidebar is `w-56 shrink-0`; main is `flex-1 overflow-auto`.

---

## Presentation defaults

| Concern | Source |
|:---|:---|
| Component library | shadcn/ui in `components/ui/` |
| Theme tokens | `@litepress/config-tailwind/theme.css` |
| Status badges | [PostStatusBadge.tsx](../../../apps/admin/components/PostStatusBadge.tsx) |
| Feedback | `sonner` toasts on mutations |

See [dual-nextjs-apps ADR](../../decisions/dual-nextjs-apps.md).

---

## Auth shell

All `(dashboard)` routes require session. Unauthenticated users redirect to `/login`. See [admin-auth ADR](../../decisions/admin-auth.md).

---

## Related

- [register-author.md](../../domain/authors/register-author.md) — author row created on first authenticated API call
