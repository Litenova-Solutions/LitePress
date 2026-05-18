using LiteBus.Commands.Abstractions;
namespace LiteNova.Blog.Application.Tags.DeleteTag;
public sealed record DeleteTagCommand(Guid Id) : ICommand;
