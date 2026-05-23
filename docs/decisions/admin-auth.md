# Admin Authentication

| Field | Value |
|:---|:---|
| Status | Accepted |
| Date | 2026-05-23 |

---

## Context

The blog has a public read-only site and a private authoring dashboard. Only a single GitHub account (the site owner) may access admin features in v1.

---

## Decision

1. **Admin app (`apps/admin`):** Auth.js v5 with GitHub OAuth provider. JWT session strategy. `signIn` callback rejects any GitHub user whose ID does not match `GITHUB_OWNER_ID`.
2. **API access:** Admin mints a short-lived JWT via `mintApiToken.ts` and forwards requests through `app/api-proxy/[...path]/route.ts`. The API validates Bearer tokens signed with `API_JWT_SECRET`.
3. **Author registration:** `EnsureAuthorMiddleware` on the API auto-registers the Author from JWT claims on first mutating request.
4. **Env vars (server-only):** `AUTH_SECRET`, `AUTH_GITHUB_ID`, `AUTH_GITHUB_SECRET`, `GITHUB_OWNER_ID`, `API_URL`, `API_JWT_SECRET`. No `NEXT_PUBLIC_` prefix for secrets.

---

## Consequences

- Auth route `app/api/auth/[...nextauth]/route.ts` must export Auth.js handlers (currently missing).
- Public web app has no authentication.
- Multi-author support requires a new ADR.

---

## References

- `standards/docs/conventions/frontend/06-admin-api-auth.md`
- `standards/docs/adr/0013-authjs-v5-authentication.md`
- `docs/domain/authors/register-author.md`
