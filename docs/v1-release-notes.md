# LitePress v1 Release Notes

| Field | Value |
|:---|:---|
| Version | 1.0.0 |
| Date | 2026-05-23 |

---

## Summary

LitePress v1 delivers a dual-frontend publishing platform aligned with Litenova Engineering Standards (ADDD): ASP.NET Core 10 API, Auth.js admin dashboard, and SEO-first public web.

---

## In Scope (shipped)

### API (`apps/api`)

- Post CRUD, publish, archive, delete
- Tag CRUD and tag assignment on posts (AddTag / RemoveTag)
- JWT Bearer auth with `EnsureAuthorMiddleware` (auto author registration)
- OpenAPI at `/openapi/v1.json`
- EF Core PostgreSQL with snake_case naming
- Test projects: Domain, Application, Architecture, Integration

### Admin (`apps/admin`)

- GitHub OAuth (single owner via `GITHUB_OWNER_ID`)
- Dashboard stats
- Post create/edit with TipTap ProseMirror JSON storage
- Tag management and draft post tag assignment
- Server-side API proxy with minted JWT

### Web (`apps/web`)

- Home, post detail, tags index, posts by tag
- Full SEO: `generateMetadata`, sitemap, robots, JSON-LD `BlogPosting`
- ProseMirror JSON to HTML rendering
- Giscus comments (when env configured)

### Documentation

- ADDD domain tree under `docs/domain/`
- Blog ADRs under `docs/decisions/`
- Shared packages: `@litenova/api-client`, `@litenova/api-types`

---

## Out of Scope (v2+)

See [docs/decisions/v1-scope-deferrals.md](decisions/v1-scope-deferrals.md):

- Scheduled publishing
- Cover image upload (R2)
- Umami analytics
- Outbox / worker
- Multi-author permissions
- VPS production deploy

---

## Verification

```bash
dotnet build apps/api/LiteNova.Blog.slnx --configuration Release
dotnet test apps/api/LiteNova.Blog.slnx --configuration Release --no-build
pnpm install --frozen-lockfile
pnpm lint && pnpm type-check && pnpm test && pnpm build
pnpm exec playwright test --config apps/web/playwright.config.ts
```

E2E publish flow runs automatically in `.github/workflows/e2e.yml` (Postgres + API + web).

---

## Human Input Required for Production

| Item | Env / config |
|:---|:---|
| Public site URL | `SITE_URL`, `NEXT_PUBLIC_SITE_URL` |
| GitHub OAuth app | `AUTH_GITHUB_ID`, `AUTH_GITHUB_SECRET`, `GITHUB_OWNER_ID` |
| JWT secret (shared API + admin) | `API_JWT_SECRET`, `JwtSettings:Secret` |
| Giscus (optional) | `NEXT_PUBLIC_GISCUS_*` |

---

## Known Limitations

- OpenAPI types generated from live spec; run `pnpm generate:api-types` after API contract changes
- Full Playwright publish flow runs in the dedicated E2E workflow (Postgres + API + web)
