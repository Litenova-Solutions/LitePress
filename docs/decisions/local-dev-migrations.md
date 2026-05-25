# Local Development Migrations

| Field | Value |
|:---|:---|
| Status | Accepted |
| Date | 2026-05-25 |

---

## Context

LitePress uses EF Core migrations. Production deployments must use reviewed migration artifacts (SQL scripts or bundles), not startup migration. Local development had friction: Aspire manages its own Postgres container, so running `docker compose` migrations before AppHost targeted the wrong database.

---

## Decision

Use a **dual local strategy**:

1. **Development auto-migrate (Aspire path):** When `ASPNETCORE_ENVIRONMENT=Development` and `Database:ApplyMigrationsOnStartup` is not `false`, the API applies pending migrations on startup via `DatabaseMigrationExtensions.ApplyDevelopmentMigrationsAsync`. Integration tests set `Database:ApplyMigrationsOnStartup=false`. Never runs in Production.

2. **Explicit CLI migrate (manual / CI path):** `pnpm db:migrate` (or `dotnet ef database update`) against docker-compose Postgres on port **5433**. Used for manual multi-terminal dev, E2E scripts, and CI.

3. **Separate Postgres instances:** Do not run docker-compose Postgres and Aspire Postgres in the same session unless you know which connection string each command uses.

---

## Consequences

- First-time Aspire startup applies migrations automatically when the API starts.
- Manual path and E2E continue to use `docker compose` on port 5433.
- Production behavior unchanged: no startup migration.

---

## References

- [Development guide](../technical/development.md)
- [Environment variables](../technical/environment.md)
- `standards/docs/conventions/backend/13-deployment-and-migrations.md`
