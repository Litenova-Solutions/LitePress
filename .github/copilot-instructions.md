# GitHub Copilot Instructions

The canonical agent guide is `AGENTS.md` at the repository root.
Read `standards/AGENTS.md` when this file is used as a submodule.

**Product:** LitePress — reference implementation of Litenova Engineering Standards.

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

- **Bounded context.** All namespaces use the `LitePress.*` prefix.
- **Author identity.** Read `AuthorId` from the JWT claim, never from the request body.
- **Terminology.** Use `Post` not Article/Content/Entry. Use `Author` not Writer/Creator.
  Use `Tag` not Category/Label.
- **No standards edits.** Never modify files inside `standards/` from this repo; propose changes upstream.

## Project-Specific Context

Read `docs/domain/README.md` and the relevant feature README and use case docs under
`docs/domain/{feature}/` before generating domain or application code.
