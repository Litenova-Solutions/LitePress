using LiteBus.CQRS;
using LiteNova.Blog.Api.Models.Requests;
using LiteNova.Blog.Application.Posts.Commands.UpdatePost;
using Mapster;

namespace LiteNova.Blog.Api.Endpoints.Posts;

public static class UpdatePostEndpoint
{
    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPut("/api/admin/posts/{id:guid}", async (Guid id, UpdatePostRequest request, IMessageBus bus, CancellationToken ct) =>
            Results.Ok(await bus.SendAsync((id, request).Adapt<UpdatePostCommand>(), ct))).RequireAuthorization();
        return app;
    }
}
