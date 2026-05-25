# Admin — shell

App: `apps/admin` · Layouts: [app/layout.tsx](../../../apps/admin/app/layout.tsx), [app/(dashboard)/layout.tsx](../../../apps/admin/app/(dashboard)/layout.tsx)

---

## Regions

| Region | Scope | Role |
|:---|:---|:---|
| Root layout | All routes | Theme, fonts, global toasts (`Toaster`) |
| Auth layout | `/login` | Centered sign-in card |
| Dashboard layout | Authenticated routes | Sidebar or nav, session guard |

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
