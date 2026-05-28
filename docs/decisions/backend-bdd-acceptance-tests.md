# ADDD Executable Acceptance Tests (Reqnroll)

| Status | Accepted |
|:---|:---|
| Date | 2026-05-28 |

## Context

LitePress use-case docs in `docs/domain/` are the normative behavior source. Integration tests cover HTTP plumbing; they do not always trace to acceptance criterion IDs. Critical publish, authorization, and conflict flows benefit from stakeholder-readable executable scenarios without duplicating domain glossaries in Gherkin.

## Decision

1. Add `LitePress.AcceptanceTests` with Reqnroll and xUnit for selected use cases.
2. Map Reqnroll scenarios to use-case docs via `@usecase:` and `@ac:` tags.
3. Keep `LitePress.Integration.Tests` for endpoint smoke, OpenAPI, and technical coverage.
4. Use PostgreSQL Testcontainers in acceptance tests (same as integration tests), not PostgreSQL from the default standards blueprint.
5. Start with `Features/Posts/PublishPost.feature` traced to `docs/domain/posts/publish-post.md`.

Reqnroll packages are conditional in `standards/standards.manifest.json`. LitePress adds them only in the acceptance test project.

## Consequences

**Positive**

- Executable traceability from `publish-post.md` acceptance criteria to CI.
- Domain language in feature files aligned with `docs/domain/posts/README.md`.

**Negative**

- Extra test project maintenance (hooks, steps, feature files).
- CI time increases for `@critical` acceptance scenarios; mitigated by tag filtering.

## Related

- [standards/docs/conventions/backend/20-api-acceptance-tests.md](../../standards/docs/conventions/backend/20-api-acceptance-tests.md)
- [standards/docs/adr/0022-addd-executable-acceptance-tests.md](../../standards/docs/adr/0022-addd-executable-acceptance-tests.md)
- [docs/domain/posts/publish-post.md](../domain/posts/publish-post.md)
