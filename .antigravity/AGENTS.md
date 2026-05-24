# Antigravity Agent Instructions

The canonical agent guide for this project is `AGENTS.md` at the repository root.

## Before Generating Code

1. Read `AGENTS.md` in full.
2. Read `standards/AGENTS.md` in full — this is the shared Litenova Solutions
   engineering standards that governs all layers, conventions, and non-negotiable rules.
3. Identify the layer you are editing: Domain, Application.Write, Application.Read,
   Application.Reactions, Infrastructure, WebApi, or a frontend app.
4. Read the corresponding convention file under `standards/docs/conventions/` before
   writing a single line of code.
5. Read the project-specific domain files in `docs/domain/` for context.

## Non-Negotiable Rules

All rules from `standards/AGENTS.md` apply. Additional project rules:

- Bounded context name is `LitePress`. Use it in all namespaces.
- `AuthorId` comes from the JWT claim only, never from the request body.
- Use `Post`, `Author`, `Tag` — not synonyms (Article, Writer, Category, etc.).
- Never edit files inside `standards/`.

## Project Domain Files

| File | Contents |
|:---|:---|
| `docs/domain/ubiquitous-language.md` | Glossary of domain terms. |
| `docs/domain/aggregate-inventory.md` | All aggregates, states, events, repositories. |
| `docs/domain/feature-inventory.md` | All use cases with handler class names. |
| `docs/domain/exception-inventory.md` | All custom exceptions with HTTP status codes. |
| `docs/domain/read-model-inventory.md` | `IDatabaseContext` properties and query handlers. |
| `docs/domain/frontend-feature-inventory.md` | Frontend routes and use cases. |
| `docs/domain/frontend-api-endpoints.md` | Backend endpoints consumed by the frontend. |
