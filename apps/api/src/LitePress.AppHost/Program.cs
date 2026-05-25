// Run order: postgres -> api -> (web + admin in parallel)

var builder = DistributedApplication.CreateBuilder(args);

// Optional admin OAuth parameters. Override via AppHost user secrets or
// apps/admin/.env.local (Next.js reads .env.local when not overridden by Aspire).
var apiJwtSecret = builder.AddParameter("api-jwt-secret", secret: true);
var authSecret = builder.AddParameter("auth-secret", secret: true);
var authGithubId = builder.AddParameter("auth-github-id", secret: true);
var authGithubSecret = builder.AddParameter("auth-github-secret", secret: true);
var githubOwnerId = builder.AddParameter("github-owner-id");

// PostgreSQL — data volume persists across restarts.
// Aspire injects ConnectionStrings__Database into the API automatically.
var postgres = builder.AddPostgres("postgres")
    .WithDataVolume("litepress-postgres-data");

var database = postgres.AddDatabase("Database");

// Next.js frontends — registered first so their endpoint references
// can be passed into the API's CORS config below.
// install: false — bootstrap runs `pnpm install` at the repo root. Aspire's
// *-installer resources fail on Windows when pnpm is a .cmd shim (dotnet/aspire#14880).
var web = builder.AddNextJsApp("web", "../../../web")
    .WithPnpm(install: false)
    .WithHttpEndpoint(env: "PORT")
    .WithExternalHttpEndpoints()
    .DisableBuildValidation();

var admin = builder.AddNextJsApp("admin", "../../../admin")
    .WithPnpm(install: false)
    .WithHttpEndpoint(env: "PORT")
    .WithExternalHttpEndpoints()
    .DisableBuildValidation()
    .WithEnvironment("API_JWT_SECRET", apiJwtSecret)
    .WithEnvironment("AUTH_SECRET", authSecret)
    .WithEnvironment("AUTH_GITHUB_ID", authGithubId)
    .WithEnvironment("AUTH_GITHUB_SECRET", authGithubSecret)
    .WithEnvironment("GITHUB_OWNER_ID", githubOwnerId);

// ASP.NET Core API
// CORS origins and connection string are injected from Aspire-allocated endpoints.
var api = builder.AddProject<Projects.LitePress_WebApi>("api")
    .WithReference(database)
    .WaitFor(database)
    .WithEnvironment("Cors__WebOrigin", web.GetEndpoint("http"))
    .WithEnvironment("Cors__AdminOrigin", admin.GetEndpoint("http"))
    .WithEnvironment("JwtSettings__Secret", apiJwtSecret)
    .WithExternalHttpEndpoints();

// Inject the API's HTTP URL into both frontends.
web.WithReference(api)
   .WithEnvironment("API_URL", api.GetEndpoint("http"));

admin.WithReference(api)
     .WithEnvironment("API_URL", api.GetEndpoint("http"));

builder.Build().Run();
