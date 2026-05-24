using LiteBus.Commands;
using LiteBus.Events;
using LiteBus.Extensions.Microsoft.DependencyInjection;
using LiteBus.Queries;
using LitePress.Application.Read;
using LitePress.Application.Reactions;
using LitePress.Application.Write;
using LitePress.Infrastructure;
using LitePress.Infrastructure.DependencyInjection;
using LitePress.WebApi.Extensions;
using LitePress.WebApi.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Authentication
var jwtSecret = builder.Configuration["JwtSettings:Secret"]
    ?? throw new InvalidOperationException("JwtSettings:Secret is required.");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true
        };
    });

builder.Services.AddAuthorization();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontends", policy =>
    {
        policy.WithOrigins(
                builder.Configuration["Cors:AdminOrigin"] ?? "http://localhost:3002",
                builder.Configuration["Cors:WebOrigin"] ?? "http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

// Infrastructure (DbContext, Repositories)
builder.Services.AddInfrastructure(builder.Configuration);

// LiteBus
builder.Services.AddLiteBus(liteBus =>
{
    liteBus.AddCommandModule(module =>
    {
        module.RegisterFromAssembly(typeof(ApplicationWriteAssemblyMarker).Assembly);
        module.RegisterFromAssembly(typeof(InfrastructureAssemblyMarker).Assembly);
    });

    liteBus.AddQueryModule(module =>
    {
        module.RegisterFromAssembly(typeof(ApplicationReadAssemblyMarker).Assembly);
    });

    liteBus.AddEventModule(module =>
    {
        module.RegisterFromAssembly(typeof(ApplicationReactionsAssemblyMarker).Assembly);
    });
});

// Endpoints
builder.Services.AddEndpoints(typeof(Program).Assembly);

// Global exception handler
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// OpenAPI
builder.Services.AddOpenApi();

// HttpContextAccessor for EnsureAuthorMiddleware
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Middleware pipeline
app.UseExceptionHandler();
app.UseCors("Frontends");
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<EnsureAuthorMiddleware>();

// Endpoints
app.MapOpenApi();
app.MapEndpoints();

app.Run();

public partial class Program { }
