# LiteNova Blog — Public Web

Next.js 15 (App Router) public-facing blog that fetches content from the API.

## Tech Stack

- **Next.js 15** with App Router and React Server Components
- **Tailwind CSS v4** + shadcn/ui
- **Giscus** for GitHub-based comments

## Running

### With Aspire (recommended)

Start from `apps/api/`:

```bash
dotnet run --project src/LiteNova.Blog.AppHost
```

The web app starts automatically. Check the Aspire dashboard (`https://localhost:15888`) for the URL.

### Standalone

```bash
cd apps/web
pnpm dev
# → http://localhost:3000
```

Set the API URL if the API is not on port 5000:

```bash
NEXT_PUBLIC_API_URL=http://localhost:5000 pnpm dev
```

### Using a `.env.local` file

Create `apps/web/.env.local`:

```env
NEXT_PUBLIC_API_URL=http://localhost:5000
```

## Environment Variables

| Variable | Default | Description |
|----------|---------|-------------|
| `NEXT_PUBLIC_API_URL` | `http://localhost:5000` | API base URL |

## Debugging

1. Run `pnpm dev` — Next.js hot reloads on file save.
2. Open Chrome DevTools or attach VS Code's **JavaScript Debugger** to `http://localhost:3000`.
3. Server-side code (Server Components, `page.tsx`) can be debugged by adding `--inspect` to the Next.js dev command:

```bash
NODE_OPTIONS='--inspect' pnpm dev
```

Then attach VS Code using the **Node.js: Attach** launch config pointing at port `9229`.

## Routes

| Route | Description |
|-------|-------------|
| `/` | Home — list of latest posts |
| `/[slug]` | Individual post page |
| `/tags` | All tags |
| `/tags/[slug]` | Posts for a tag |

## Building for Production

```bash
pnpm build
pnpm start
```

> For containerised deployment, add `output: "standalone"` to `next.config.ts` and use `AddNextJsApp` in the Aspire AppHost (standalone mode is validated at publish time).
