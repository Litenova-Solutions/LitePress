using Amazon.S3;
using LiteBus.Commands;
using LiteBus.Commands.Abstractions;
using LiteBus.Events;
using LiteBus.Events.Abstractions;
using LiteBus.Extensions.Microsoft.DependencyInjection;
using LiteBus.Queries;
using LiteBus.Queries.Abstractions;
using LiteNova.Blog.Api.Mappers;
using LiteNova.Blog.Api.Middleware;
using LiteNova.Blog.Application.Common.Interfaces;
using LiteNova.Blog.Application.Posts.CreatePost;
using LiteNova.Blog.Infrastructure.Persistence;
using LiteNova.Blog.Infrastructure.Storage;
using Mapster;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication("Cookies").AddCookie("Cookies");
builder.Services.AddAuthorization();

builder.Services.AddDbContext<BlogDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection") ?? builder.Configuration["DATABASE_URL"]));
builder.Services.AddScoped<IBlogDbContext>(sp => sp.GetRequiredService<BlogDbContext>());

builder.Services.AddSingleton(new TypeAdapterConfig());
builder.Services.AddMapster();
PostMapper.Register(TypeAdapterConfig.GlobalSettings);
TagMapper.Register(TypeAdapterConfig.GlobalSettings);

builder.Services.AddSingleton<IAmazonS3>(_ =>
{
    var config = new AmazonS3Config
    {
        ServiceURL = $"https://{builder.Configuration["CLOUDFLARE_R2_ACCOUNT_ID"]}.r2.cloudflarestorage.com",
        ForcePathStyle = true
    };
    return new AmazonS3Client(
        builder.Configuration["CLOUDFLARE_R2_ACCESS_KEY_ID"],
        builder.Configuration["CLOUDFLARE_R2_SECRET_ACCESS_KEY"],
        config);
});
builder.Services.AddScoped<CloudflareR2StorageService>();

builder.Services.AddLiteBus(liteBus =>
{
    var appAssembly = typeof(CreatePostCommand).Assembly;
    liteBus.AddCommandModule(module => module.RegisterFromAssembly(appAssembly));
    liteBus.AddQueryModule(module => module.RegisterFromAssembly(appAssembly));
    liteBus.AddEventModule(module => module.RegisterFromAssembly(appAssembly));
});

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

app.MapAllEndpoints();

app.Run();

public static class EndpointRegistration
{
    public static IEndpointRouteBuilder MapAllEndpoints(this IEndpointRouteBuilder app)
    {
        LiteNova.Blog.Api.Endpoints.Posts.GetPublishedPosts.GetPublishedPostsEndpoint.MapEndpoints(app);
        LiteNova.Blog.Api.Endpoints.Posts.GetPostBySlug.GetPostBySlugEndpoint.MapEndpoints(app);
        LiteNova.Blog.Api.Endpoints.Posts.GetAllPosts.GetAllPostsEndpoint.MapEndpoints(app);
        LiteNova.Blog.Api.Endpoints.Posts.CreatePost.CreatePostEndpoint.MapEndpoints(app);
        LiteNova.Blog.Api.Endpoints.Posts.UpdatePost.UpdatePostEndpoint.MapEndpoints(app);
        LiteNova.Blog.Api.Endpoints.Posts.PublishPost.PublishPostEndpoint.MapEndpoints(app);
        LiteNova.Blog.Api.Endpoints.Posts.SchedulePost.SchedulePostEndpoint.MapEndpoints(app);
        LiteNova.Blog.Api.Endpoints.Posts.DeletePost.DeletePostEndpoint.MapEndpoints(app);
        LiteNova.Blog.Api.Endpoints.Tags.GetAllTags.GetAllTagsEndpoint.MapEndpoints(app);
        LiteNova.Blog.Api.Endpoints.Tags.CreateTag.CreateTagEndpoint.MapEndpoints(app);
        LiteNova.Blog.Api.Endpoints.Tags.DeleteTag.DeleteTagEndpoint.MapEndpoints(app);
        return app;
    }
}
