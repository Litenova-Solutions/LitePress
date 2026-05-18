using LiteBus.CQRS;
namespace LiteNova.Blog.Application.Tags.Commands.CreateTag;
public sealed record CreateTagCommand(string Name) : ICommand<CreateTagResult>;
