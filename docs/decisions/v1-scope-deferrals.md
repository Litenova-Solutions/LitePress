# v1 Scope Deferrals

| Field | Value |
|:---|:---|
| Status | Accepted |
| Date | 2026-05-23 |

---

## Context

Several capabilities exist as partial code, env vars, or inventory placeholders but are not required for a shippable v1.

---

## Decision

Explicitly defer the following to v2+ unless a new ADR promotes them:

| Feature | Current state | v1 action |
|:---|:---|:---|
| Scheduled publishing | Deferred to v2+ | No endpoints; orphan domain types removed |
| Cover image upload (R2) | Env vars may exist | URL string only; no upload UI |
| Umami analytics | Env vars may exist | Not wired |
| Outbox / worker | Reactions log-only OK | No outbox table |
| Multi-author permissions | Single `GITHUB_OWNER_ID` | Owner only |
| VPS production deploy | Aspire for local dev; docker-compose Postgres for CI/E2E/manual API debug | Local dev sufficient |

---

## Consequences

- Domain docs and acceptance criteria exclude deferred features.
- Orphan scheduled-post domain types were removed in v1 cleanup.

---

## References

- `docs/domain/README.md`
