using Amazon.S3;
using System.Reflection;
using LiteBus.CQRS;
using LiteNova.Blog.Api.Mappers;
using LiteNova.Blog.Api.Middleware;
using LiteNova.Blog.Application.Common.Interfaces;
using LiteNova.Blog.Application.Posts.Commands.CreatePost;
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

ServiceRegistration.RegisterHandlers(builder.Services, typeof(CreatePostCommand).Assembly);
builder.Services.AddScoped<IMessageBus, InProcessMessageBus>();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

app.MapAllEndpoints();

app.Run();

public sealed class InProcessMessageBus(IServiceProvider serviceProvider) : IMessageBus
{
    public Task PublishAsync(object domainEvent, CancellationToken cancellationToken = default) => Task.CompletedTask;

    public Task SendAsync(ICommand command, CancellationToken cancellationToken = default)
    {
        var handlerType = typeof(ICommandHandler<>).MakeGenericType(command.GetType());
        dynamic handler = serviceProvider.GetRequiredService(handlerType);
        return handler.HandleAsync((dynamic)command, cancellationToken);
    }

    public Task<TResult> SendAsync<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default)
    {
        var handlerType = typeof(ICommandHandler<,>).MakeGenericType(command.GetType(), typeof(TResult));
        dynamic handler = serviceProvider.GetRequiredService(handlerType);
        return handler.HandleAsync((dynamic)command, cancellationToken);
    }

    public Task<TResult> QueryAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default)
    {
        var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));
        dynamic handler = serviceProvider.GetRequiredService(handlerType);
        return handler.HandleAsync((dynamic)query, cancellationToken);
    }
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

public static class ServiceRegistration
{
    public static void RegisterHandlers(IServiceCollection services, Assembly assembly)
    {
        var handlerInterfaces =
            new[]
        {
            typeof(ICommandHandler<>),
            typeof(ICommandHandler<,>),
            typeof(IQueryHandler<,>),
            typeof(ICommandValidator<>),
            typeof(IQueryValidator<>)
        };

        foreach (var type in assembly.GetTypes().Where(t => t is { IsAbstract: false, IsInterface: false }))
        {
            foreach (var serviceType in type.GetInterfaces().Where(i => i.IsGenericType && handlerInterfaces.Contains(i.GetGenericTypeDefinition())))
            {
                services.AddScoped(serviceType, type);
            }
        }
    }
}
