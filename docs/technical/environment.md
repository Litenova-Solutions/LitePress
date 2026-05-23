# Environment Variables

Validated configuration for API, web, and admin. Server-only secrets must **not** use the `NEXT_PUBLIC_` prefix.

---

## API (`apps/api`)

Set via environment variables or `appsettings.json`. Aspire injects these in local dev.

| Variable | Default (local) | Description |
|:---|:---|:---|
| `ConnectionStrings__Database` | `Host=localhost;Port=5433;Database=litepress;Username=litepress;Password=litepress` | PostgreSQL |
| `JwtSettings__Secret` | *(must set in prod)* | HS256 signing key for API JWT (min 32 chars) |
| `Cors__WebOrigin` | `http://localhost:3000` | CORS origin for public web |
| `Cors__AdminOrigin` | `http://localhost:3002` | CORS origin for admin |

**Production:** Generate a strong `JwtSettings__Secret`. The same value must be used as `API_JWT_SECRET` in admin.

---

## Admin (`apps/admin`)

Create `apps/admin/.env.local` (never commit). Validated in `apps/admin/lib/env.ts`.

| Variable | Required | Description |
|:---|:---|:---|
| `API_URL` | Yes (prod) | Backend base URL, e.g. `http://localhost:5000` |
| `API_JWT_SECRET` | Yes (prod) | Must match API `JwtSettings__Secret` |
| `AUTH_SECRET` | Yes (prod) | Auth.js encryption secret (`openssl rand -base64 32`) |
| `AUTH_GITHUB_ID` | Yes | GitHub OAuth App client ID |
| `AUTH_GITHUB_SECRET` | Yes | GitHub OAuth App client secret |
| `GITHUB_OWNER_ID` | Yes | Numeric GitHub user ID allowed to sign in |

Dev defaults exist for local experimentation but OAuth will not work until real GitHub credentials are set.

### GitHub OAuth App settings (local)

| Field | Value |
|:---|:---|
| Homepage URL | `http://localhost:3002` |
| Authorization callback URL | `http://localhost:3002/api/auth/callback/github` |

---

## Public web (`apps/web`)

Create `apps/web/.env.local` as needed. Validated in `apps/web/lib/env.ts`.

| Variable | Default (local) | Description |
|:---|:---|:---|
| `API_URL` | `http://localhost:5000` | Server-only API base URL |
| `SITE_URL` | `http://localhost:3000` | Canonical site URL for SEO |
| `NEXT_PUBLIC_SITE_URL` | *(optional)* | Overrides public site URL when set |
| `NEXT_PUBLIC_GISCUS_REPO` | *(optional)* | Giscus: `owner/repo` |
| `NEXT_PUBLIC_GISCUS_REPO_ID` | *(optional)* | Giscus repository ID |
| `NEXT_PUBLIC_GISCUS_CATEGORY_ID` | *(optional)* | Giscus category ID |

`API_URL` is read on the server only (Server Components, sitemap). It is not exposed to the browser bundle.

---

## E2E / CI

Used by Playwright global setup and `.github/workflows/e2e.yml`:

| Variable | Example | Description |
|:---|:---|:---|
| `E2E_API_URL` | `http://localhost:5000` | API for seeding test posts |
| `API_JWT_SECRET` | *(same as API)* | JWT for E2E API calls |
| `PLAYWRIGHT_BASE_URL` | `http://localhost:3000` | Public web under test |
| `API_URL` | `http://localhost:5000` | Web app server-side fetch |
| `SITE_URL` | `http://localhost:3000` | SEO base URL in tests |

---

## Production checklist

| Item | Variables |
|:---|:---|
| Public site URL | `SITE_URL`, `NEXT_PUBLIC_SITE_URL` |
| API JWT (shared) | `JwtSettings__Secret`, `API_JWT_SECRET` |
| GitHub OAuth | `AUTH_GITHUB_ID`, `AUTH_GITHUB_SECRET`, `GITHUB_OWNER_ID` |
| Auth.js | `AUTH_SECRET` |
| Database | `ConnectionStrings__Database` |
| Giscus (optional) | `NEXT_PUBLIC_GISCUS_*` |

See also [v1 release notes — human input](../v1-release-notes.md#human-input-required-for-production).
