# Scalar for local API reference

| Field | Value |
|:---|:---|
| Status | Accepted |
| Date | 2026-05-25 |

---

## Context

.NET 10 ships native OpenAPI document generation via `Microsoft.AspNetCore.OpenApi`. Swashbuckle is no longer the default template stack. LitePress needs a browsable API reference for local development and type generation workflows, without adding legacy Swagger dependencies.

---

## Decision

Use **`Microsoft.AspNetCore.OpenApi`** for the OpenAPI 3.x spec at `/openapi/v1.json` and **`Scalar.AspNetCore`** for the interactive reference UI at `/scalar/v1`.

Scalar is mapped **only when `ASPNETCORE_ENVIRONMENT=Development`**. Production deployments expose HTTP endpoints through the WebApi project configuration; Scalar is not enabled in Production by default.

---

## Consequences

- Developers open Scalar from the Aspire dashboard API base URL + `/scalar/v1`, or `http://localhost:5000/scalar/v1` on the manual path.
- CI continues to validate `/openapi/v1.json` and committed `packages/api-types/openapi.json`.
- If LitePress later needs a public API portal, add a project ADR rather than enabling Scalar in Production by default.

---

## References

- [Microsoft Learn: Use the generated OpenAPI documents](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/openapi/using-openapi-documents)
- [apps/api/README.md](../../apps/api/README.md)
