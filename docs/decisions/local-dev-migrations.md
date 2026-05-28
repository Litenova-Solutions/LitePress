# Local Development Schema Apply

| Field | Value |
|:---|:---|
| Status | Accepted (updated 2026-05-28 for PostgreSQL) |
| Date | 2026-05-25 |

---

## Context

LitePress stores data in PostgreSQL. Marten storage schema is applied at runtime in Development (`MartenSchemaExtensions`). JSON document shape is configured in `apps/api/src/LitePress.Infrastructure/Marten/Serialization/`. Production must use reviewed schema artifacts, not startup apply.

Local development had friction when Aspire and docker compose both ran database containers with different connection strings.

---

## Decision

Use a **dual local strategy**:

1. **Development auto-schema (Aspire path):** When `ASPNETCORE_ENVIRONMENT=Development` and `Database:ApplySchemaOnStartup` is not `false`, the API applies Marten storage schema schema on startup via `DatabaseSchemaExtensions.ApplyDevelopmentSchemaAsync`. Integration tests set `Database:ApplySchemaOnStartup=false` and apply schema in the test fixture. Never runs in Production.

2. **Explicit CLI apply (manual / CI path):** `pnpm db:migrate` runs the API with `--apply-schema-only` against docker-compose PostgreSQL on port **8000**. Used for manual multi-terminal dev and E2E scripts.

3. **Separate database instances:** Do not run docker-compose PostgreSQL and Aspire PostgreSQL in the same session unless you know which connection string each command uses.

---

## Consequences

- First-time Aspire startup applies schema automatically when the API starts.
- Manual path and E2E use `docker compose` PostgreSQL on port 5432.
- Production behavior unchanged: no startup schema apply.

---

## References

- [martendb-persistence.md](martendb-persistence.md)
- [Development guide](../technical/development.md)
- [Environment variables](../technical/environment.md)
