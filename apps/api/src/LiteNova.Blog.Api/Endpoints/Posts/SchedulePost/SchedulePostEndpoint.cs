using LiteBus.Commands.Abstractions;
using LiteNova.Blog.Api.Models.Requests;
using LiteNova.Blog.Application.Posts.SchedulePost;

namespace LiteNova.Blog.Api.Endpoints.Posts.SchedulePost;

public static class SchedulePostEndpoint
{
    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/admin/posts/{id:guid}/schedule", async (Guid id, SchedulePostRequest request, ICommandMediator mediator, CancellationToken cancellationToken) =>
            Results.Ok(await mediator.SendAsync(new SchedulePostCommand(id, request.ScheduledFor), cancellationToken))).RequireAuthorization();

        return app;
    }
}
