// Run order: postgres -> api -> (web + admin in parallel)
// pnpm install is skipped here — run `pnpm install` from the repo root first.

var builder = DistributedApplication.CreateBuilder(args);

// PostgreSQL — data volume persists across restarts.
// Aspire injects ConnectionStrings__Database into the API automatically.
var postgres = builder.AddPostgres("postgres")
    .WithDataVolume("blog-postgres-data");

var database = postgres.AddDatabase("Database");

// Next.js frontends — registered first so their endpoint references
// can be passed into the API's CORS config below.
var web = builder.AddNextJsApp("web", "../../../web")
    .WithPnpm(install: false)
    .WithHttpEndpoint(env: "PORT")
    .WithExternalHttpEndpoints()
    .DisableBuildValidation();

var admin = builder.AddNextJsApp("admin", "../../../admin")
    .WithPnpm(install: false)
    .WithHttpEndpoint(env: "PORT")
    .WithExternalHttpEndpoints()
    .DisableBuildValidation();

// ASP.NET Core API
// CORS origins and connection string are injected from Aspire-allocated endpoints.
var api = builder.AddProject<Projects.LiteNova_Blog_WebApi>("api")
    .WithReference(database)
    .WaitFor(database)
    .WithEnvironment("Cors__WebOrigin", web.GetEndpoint("http"))
    .WithEnvironment("Cors__AdminOrigin", admin.GetEndpoint("http"))
    .WithExternalHttpEndpoints();

// Inject the API's HTTP URL into both frontends.
web.WithReference(api)
   .WithEnvironment("NEXT_PUBLIC_API_URL", api.GetEndpoint("http"));

admin.WithReference(api)
     .WithEnvironment("NEXT_PUBLIC_API_URL", api.GetEndpoint("http"));

builder.Build().Run();
