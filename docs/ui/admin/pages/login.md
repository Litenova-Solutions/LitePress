# Login (`/login`)

| Field | Value |
|:---|:---|
| App | `apps/admin` |
| Route | `/login` |
| Route shell | [app/(auth)/login/page.tsx](../../../apps/admin/app/(auth)/login/page.tsx) |

---

## Use cases

GitHub OAuth via Auth.js. No domain command. After sign-in, [register-author.md](../../domain/authors/register-author.md) runs on first API request.

---

## Visible states

| State | User sees |
|:---|:---|
| Unauthenticated | GitHub sign-in button |
| Authenticated | Redirect to dashboard |

---

## Shell

[Auth layout](../shell.md). No dashboard chrome.

---

## Related

[admin-auth ADR](../../decisions/admin-auth.md)
