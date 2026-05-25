# @litepress/config-tailwind

Shared **CSS theme tokens** for LitePress Next.js apps. Not a React component library.

Both `apps/web` and `apps/admin` use shadcn/ui with the same default neutral theme. This package holds `:root` variables and `@theme inline` mappings so tokens stay in one place.

---

## Exports

| Import | Contents |
|:---|:---|
| `@litepress/config-tailwind/theme.css` | Design tokens (`:root`, `.dark`, `@theme inline`, base layer) |
| `@litepress/config-tailwind/styles.css` | Full stack import for apps that do not split entry CSS (optional) |

---

## Per-app setup

Each app MUST still own:

- `app/globals.css` with `@import "tailwindcss"` and `@source` for its files
- `postcss.config.mjs` with `@tailwindcss/postcss`
- `components/ui/` via shadcn CLI (components are not shared)

Example `app/globals.css`:

```css
@import "tailwindcss";
@import "tw-animate-css";
@import "@litepress/config-tailwind/theme.css";

@source "../app/**/*.{js,ts,jsx,tsx}";
@source "../components/**/*.{js,ts,jsx,tsx}";
@source "../domain/**/*.{js,ts,jsx,tsx}";
```

An app MAY override tokens in its own `globals.css` when documented in a use-case or ADR.

---

## Related

- Engineering Standards: `docs/conventions/frontend/02-components.md` (shared theme vs shared components)
- [Development guide](../../docs/technical/development.md#frontend-ui-shadcnui)
