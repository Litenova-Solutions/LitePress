using LiteBus.CQRS;
using LiteNova.Blog.Api.Models.Requests;
using LiteNova.Blog.Application.Posts.Commands.SchedulePost;

namespace LiteNova.Blog.Api.Endpoints.Posts;

public static class SchedulePostEndpoint
{
    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/admin/posts/{id:guid}/schedule", async (Guid id, SchedulePostRequest request, IMessageBus bus, CancellationToken ct) =>
            Results.Ok(await bus.SendAsync(new SchedulePostCommand(id, request.ScheduledFor), ct))).RequireAuthorization();
        return app;
    }
}
