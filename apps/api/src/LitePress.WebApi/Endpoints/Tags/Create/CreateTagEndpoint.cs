namespace LitePress.WebApi.Endpoints.Tags.Create;

internal sealed record CreateTagRequest(string Name);
internal sealed record CreateTagResponse(Guid TagId, string Slug);

internal static class CreateTagApiMappings
{
    internal static CreateTagCommand ToCommand(this CreateTagRequest request)
        => new() { TagId = TagId.New(), Name = request.Name };

    internal static CreateTagResponse ToResponse(this CreateTagCommandResult result)
        => new(result.TagId, result.Slug);
}

internal sealed class CreateTagEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/tags", HandleAsync)
            .WithName("CreateTag")
            .WithTags("Tags")
            .WithSummary("Creates a new tag.")
            .Produces<CreateTagResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .RequireAuthorization();
    }

    private static async Task<IResult> HandleAsync(
        CreateTagRequest request,
        ICommandMediator commandMediator,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand();
        var result = await commandMediator.SendAsync(command, cancellationToken);
        var response = result.ToResponse();

        return Results.Created($"/api/tags/{response.TagId}", response);
    }
}
