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
| Scheduled publishing | Orphan `PostStatus.Scheduled`, `PostScheduledEvent`, `PostAlreadyScheduledException` | Remove orphan code or leave unused; no endpoints |
| Cover image upload (R2) | Env vars may exist | URL string only; no upload UI |
| Umami analytics | Env vars may exist | Not wired |
| Outbox / worker | Reactions log-only OK | No outbox table |
| Multi-author permissions | Single `GITHUB_OWNER_ID` | Owner only |
| VPS production deploy | Aspire + docker-compose Postgres | Local dev sufficient |

---

## Consequences

- Domain docs and acceptance criteria exclude deferred features.
- Orphan scheduled-post code should be removed in a cleanup PR to avoid agent confusion.

---

## References

- `docs/domain/README.md`
