// Run order: postgres -> api -> (web + admin in parallel)



var builder = DistributedApplication.CreateBuilder(args);



// Optional admin OAuth parameters. Override via AppHost user secrets or

// apps/admin/.env.local (Next.js reads .env.local when not overridden by Aspire).

var apiJwtSecret = builder.AddParameter("api-jwt-secret", secret: true);

var authSecret = builder.AddParameter("auth-secret", secret: true);

var authGithubId = builder.AddParameter("auth-github-id", secret: true);

var authGithubSecret = builder.AddParameter("auth-github-secret", secret: true);

var githubOwnerId = builder.AddParameter("github-owner-id");



var postgresPassword = builder.AddParameter("postgres-password", "litepress", secret: true);

var postgres = builder.AddPostgres("postgres", password: postgresPassword)

    .WithDataVolume("litepress-postgres-data")

    .AddDatabase("litepress");



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



// ASP.NET Core API — PostgreSQL connection string is injected by Aspire.

var api = builder.AddProject<Projects.LitePress_WebApi>("api")

    .WithReference(postgres)

    .WithEnvironment("Cors__WebOrigin", web.GetEndpoint("http"))

    .WithEnvironment("Cors__AdminOrigin", admin.GetEndpoint("http"))

    .WithEnvironment("JwtSettings__Secret", apiJwtSecret)

    .WithExternalHttpEndpoints();



web.WithReference(api)

   .WithEnvironment("API_URL", api.GetEndpoint("http"));



admin.WithReference(api)

     .WithEnvironment("API_URL", api.GetEndpoint("http"));



builder.Build().Run();

