using LiteBus.CQRS;
using LiteNova.Blog.Application.Common.Exceptions;
using LiteNova.Blog.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LiteNova.Blog.Application.Tags.Commands.DeleteTag;

public sealed class DeleteTagCommandHandler(IBlogDbContext dbContext) : ICommandHandler<DeleteTagCommand>
{
    public async Task HandleAsync(DeleteTagCommand command, CancellationToken cancellationToken)
    {
        var tag = await dbContext.Tags.FirstOrDefaultAsync(t => t.Id == command.Id, cancellationToken) ?? throw new TagNotFoundException(command.Id);
        dbContext.Tags.Remove(tag);
    }
}
