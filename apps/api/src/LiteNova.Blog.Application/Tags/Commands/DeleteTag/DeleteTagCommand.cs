using LiteBus.CQRS;
namespace LiteNova.Blog.Application.Tags.Commands.DeleteTag;
public sealed record DeleteTagCommand(Guid Id) : ICommand;
