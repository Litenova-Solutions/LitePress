# MartenDB persistence

## Status

Accepted (2026-05-28). Supersedes [surrealdb-persistence.md](surrealdb-persistence.md) (removed).

## Context

LitePress stores document-shaped aggregates (Post, Author, Tag) with polymorphic post lifecycle state. Engineering Standards default to PostgreSQL + EF Core. LitePress overrides that stack with **Marten** on PostgreSQL so aggregates are persisted as JSON documents without a separate persistence model layer.

## Decision

- **Database:** PostgreSQL 17 (docker compose, Aspire, Testcontainers).
- **ORM / document store:** [Marten](https://martendb.io) 9.x on `ConnectionStrings:DefaultConnection`.
- **Documents:** Domain aggregate roots (`Post`, `Author`, `Tag`) stored and loaded directly.
- **Serialization:** System.Text.Json (Marten default). Per-type rules live under `apps/api/src/LitePress.Infrastructure/Marten/Serialization/` (conventions + type configurations, similar to EF `IEntityTypeConfiguration`).
- **Writes:** Repositories use `IDocumentSession`; LiteBus pipeline behaviors commit the session and dispatch domain events after save.
- **Reads:** Query handlers use `IReadDatabase` / `IReadDatabaseContext` (LINQ over Marten `IQuerySession`).

## Consequences

- No EF Core, SurrealDB, or `dotnet ef` in LitePress.
- `pnpm db:migrate` applies Marten storage schema (`--apply-schema-only`).
- `pnpm db:reset` drops the PostgreSQL volume and reapplies schema (manual compose path).
- Integration and acceptance tests use PostgreSQL Testcontainers.

## References

- [Marten documentation](https://martendb.io)
- [local-dev-migrations.md](local-dev-migrations.md)
