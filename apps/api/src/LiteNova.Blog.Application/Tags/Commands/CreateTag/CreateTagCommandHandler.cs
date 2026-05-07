using LiteBus.CQRS;
using LiteNova.Blog.Application.Common.Interfaces;
using LiteNova.Blog.Domain.Tags;

namespace LiteNova.Blog.Application.Tags.Commands.CreateTag;

public sealed class CreateTagCommandHandler(IBlogDbContext dbContext) : ICommandHandler<CreateTagCommand, CreateTagResult>
{
    public Task<CreateTagResult> HandleAsync(CreateTagCommand command, CancellationToken cancellationToken)
    {
        var tag = Tag.Create(command.Name);
        dbContext.Tags.Add(tag);
        return Task.FromResult(new CreateTagResult(tag.Id));
    }
}
