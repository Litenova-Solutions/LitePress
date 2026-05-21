# GitHub Copilot Instructions

The canonical agent guide is `AGENTS.md` at the repository root.
Read `standards/AGENTS.md` when this file is used as a submodule.

## Before Generating Code

1. Read `AGENTS.md` in full.
2. Read `standards/AGENTS.md` in full.
3. Identify the layer you are editing: Domain, Application.Write, Application.Read,
   Application.Reactions, Infrastructure, or WebApi.
4. Read the corresponding convention file under `standards/docs/conventions/` before
   writing a single line of code.
5. Read the project-specific domain files in `docs/domain/` for context.

## Non-Negotiable Rules

All rules from `standards/AGENTS.md` apply without exception. Additional project rules:

- **Bounded context.** All namespaces use the `Blog` prefix: `Blog.Domain`,
  `Blog.Application.Write`, `Blog.Application.Read`, etc.
- **Author identity.** Read `AuthorId` from the JWT claim, never from the request body.
- **Terminology.** Use `Post` not Article/Content/Entry. Use `Author` not Writer/Creator.
  Use `Tag` not Category/Label.
- **No standards edits.** Never modify files inside `standards/`.

## Project-Specific Context

Read `docs/domain/ubiquitous-language.md`, `docs/domain/aggregate-inventory.md`,
`docs/domain/feature-inventory.md`, `docs/domain/exception-inventory.md`,
`docs/domain/read-model-inventory.md`, `docs/domain/frontend-feature-inventory.md`,
and `docs/domain/frontend-api-endpoints.md` before generating domain or application code.
