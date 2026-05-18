using LiteBus.Commands.Abstractions;
using LiteNova.Blog.Api.Models.Requests;
using LiteNova.Blog.Application.Posts.UpdatePost;
using Mapster;

namespace LiteNova.Blog.Api.Endpoints.Posts.UpdatePost;

public static class UpdatePostEndpoint
{
    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPut("/api/admin/posts/{id:guid}", async (Guid id, UpdatePostRequest request, ICommandMediator commandMediator, CancellationToken ct) =>
            Results.Ok(await commandMediator.SendAsync((id, request).Adapt<UpdatePostCommand>(), ct))).RequireAuthorization();
        return app;
    }
}
