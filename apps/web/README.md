# LitePress — Public Web

Next.js 16 public-facing blog. Server Components fetch published content from the API via typed `getApiClient()`.

User guide: [docs/how-it-works.md](../../docs/how-it-works.md) · SEO: [docs/decisions/seo-public-web.md](../../docs/decisions/seo-public-web.md)

---

## Stack

- Next.js 16 · React 19.2 · TypeScript 6
- Tailwind CSS 4 · shadcn/ui (`components/ui/`)
- Shared theme: `@litepress/config-tailwind/theme.css`
- `@litepress/api-client` + `@litepress/api-types` (OpenAPI)
- Giscus comments (optional)
- ProseMirror JSON → HTML via `@tiptap/html`

LitePress project docs override engineering standards when they specify app-specific UI or routing behavior.

---

## Run

### With Aspire

```bash
dotnet run --project apps/api/src/LitePress.AppHost
```

Check Aspire dashboard for the web app URL.

### Standalone

```bash
pnpm --filter web dev
# → http://localhost:3000
```

Optional `apps/web/.env.local`:

```env
API_URL=http://localhost:5000
SITE_URL=http://localhost:3000
NEXT_PUBLIC_SITE_URL=http://localhost:3000
```

See [docs/technical/environment.md](../../docs/technical/environment.md).

---

## Routes

| Route | Description |
|:---|:---|
| `/` | Published posts (paginated; optional `?tag=` filter) |
| `/[slug]` | Post detail + JSON-LD + Giscus |
| `/tags` | Tag index |
| `/tags/[slug]` | Posts by tag |
| `/sitemap.xml` | Dynamic sitemap |
| `/robots.txt` | Crawler rules |

---

## Code layout

```
apps/web/
├── app/                    # Thin route shells
├── components/ui/          # shadcn/ui components (owned in this app)
├── domain/                 # Feature UI by use case
│   ├── posts/list-published-posts/
│   ├── posts/view-post-by-slug/
│   └── tags/list-tags/
├── shared/prosemirror/     # JSON → HTML renderer
├── lib/api/client.ts       # getApiClient() for Server Components
├── postcss.config.mjs      # Tailwind v4 PostCSS
└── e2e/                    # Playwright tests
```

---

## UI setup

After clone, `pnpm install` and bootstrap verify Tailwind + shadcn scaffolding. To add a component:

```bash
cd apps/web
npx shadcn@latest add <component-name>
```

See [Development guide — Frontend UI](../../docs/technical/development.md#frontend-ui-shadcnui).

---

## Build and test

```bash
pnpm --filter web lint
pnpm --filter web type-check
pnpm --filter web test
pnpm --filter web build
pnpm exec playwright test --config apps/web/playwright.config.ts
```

---

## SEO

All public routes use `generateMetadata`. Post pages emit Open Graph, Twitter cards, and `BlogPosting` JSON-LD. See [seo-public-web.md](../../docs/decisions/seo-public-web.md).
