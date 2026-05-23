# LitePress Documentation

Documentation for [LitePress](https://github.com/Litenova-Solutions/LitePress), the reference implementation of [Litenova Engineering Standards](https://github.com/Litenova-Solutions/Engineering-Standards).

**License:** Personal noncommercial use is free under [PolyForm Noncommercial](../LICENSE). Commercial use requires a [commercial license](../COMMERCIAL-LICENSE.md).

---

## For everyone

| Document | Description |
|:---|:---|
| [How LitePress works](how-it-works.md) | Plain-language guide: reading posts, writing, publishing, tags, comments |
| [v1 release notes](v1-release-notes.md) | What shipped in v1 and what is deferred to v2+ |

---

## For developers

| Document | Description |
|:---|:---|
| [Technical overview](technical/README.md) | Architecture, stack, and doc map |
| [Architecture](technical/architecture.md) | System diagram, apps, data flow, clean architecture |
| [Development guide](technical/development.md) | Local setup, verification gates, CI, debugging |
| [Environment variables](technical/environment.md) | All env vars for API, web, and admin |
| [API reference](technical/api-reference.md) | HTTP endpoints and OpenAPI |

---

## Domain-driven design (ADDD)

Use-case and feature docs are the source of truth for product behavior:

| Path | Contents |
|:---|:---|
| [domain/README.md](domain/README.md) | System map: posts, tags, authors |
| [domain/posts/](domain/posts/) | Post aggregate and use cases |
| [domain/tags/](domain/tags/) | Tag aggregate and use cases |
| [domain/authors/](domain/authors/) | Author registration |

---

## Project decisions (ADRs)

LitePress-specific decisions that extend (not replace) [Engineering Standards ADRs](https://github.com/Litenova-Solutions/Engineering-Standards/tree/main/docs/adr):

| Document | Topic |
|:---|:---|
| [decisions/README.md](decisions/README.md) | Index |
| [admin-auth.md](decisions/admin-auth.md) | GitHub OAuth + JWT API access |
| [dual-nextjs-apps.md](decisions/dual-nextjs-apps.md) | Separate web and admin apps |
| [prosemirror-json-storage.md](decisions/prosemirror-json-storage.md) | Rich text storage format |
| [giscus-comments.md](decisions/giscus-comments.md) | Public comments |
| [seo-public-web.md](decisions/seo-public-web.md) | SEO strategy |
| [licensing.md](decisions/licensing.md) | PolyForm Noncommercial + commercial license |
| [v1-scope-deferrals.md](decisions/v1-scope-deferrals.md) | Out-of-scope features |

---

## Related repositories

| Repository | Role |
|:---|:---|
| [Litenova-Solutions/LitePress](https://github.com/Litenova-Solutions/LitePress) | This application |
| [Litenova-Solutions/Engineering-Standards](https://github.com/Litenova-Solutions/Engineering-Standards) | Shared engineering standards (git submodule at `standards/`) |
