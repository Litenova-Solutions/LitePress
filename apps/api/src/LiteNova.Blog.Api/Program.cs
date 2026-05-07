using Amazon.S3;
using LiteBus.CQRS;
using LiteNova.Blog.Api.Mappers;
using LiteNova.Blog.Api.Middleware;
using LiteNova.Blog.Application.Common.Interfaces;
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

builder.Services.AddSingleton<IMessageBus, NoOpMessageBus>();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

app.MapAllEndpoints();

app.Run();

public sealed class NoOpMessageBus : IMessageBus
{
    public Task PublishAsync(object domainEvent, CancellationToken cancellationToken = default) => Task.CompletedTask;
    public Task SendAsync(ICommand command, CancellationToken cancellationToken = default) => Task.CompletedTask;
    public Task<TResult> SendAsync<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default) => Task.FromResult(default(TResult)!);
    public Task<TResult> QueryAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default) => Task.FromResult(default(TResult)!);
}

public static class EndpointRegistration
{
    public static IEndpointRouteBuilder MapAllEndpoints(this IEndpointRouteBuilder app)
    {
        LiteNova.Blog.Api.Endpoints.Posts.GetPublishedPostsEndpoint.MapEndpoints(app);
        LiteNova.Blog.Api.Endpoints.Posts.GetPostBySlugEndpoint.MapEndpoints(app);
        LiteNova.Blog.Api.Endpoints.Posts.GetAllPostsEndpoint.MapEndpoints(app);
        LiteNova.Blog.Api.Endpoints.Posts.CreatePostEndpoint.MapEndpoints(app);
        LiteNova.Blog.Api.Endpoints.Posts.UpdatePostEndpoint.MapEndpoints(app);
        LiteNova.Blog.Api.Endpoints.Posts.PublishPostEndpoint.MapEndpoints(app);
        LiteNova.Blog.Api.Endpoints.Posts.SchedulePostEndpoint.MapEndpoints(app);
        LiteNova.Blog.Api.Endpoints.Posts.DeletePostEndpoint.MapEndpoints(app);
        LiteNova.Blog.Api.Endpoints.Tags.GetAllTagsEndpoint.MapEndpoints(app);
        LiteNova.Blog.Api.Endpoints.Tags.CreateTagEndpoint.MapEndpoints(app);
        LiteNova.Blog.Api.Endpoints.Tags.DeleteTagEndpoint.MapEndpoints(app);
        return app;
    }
}
