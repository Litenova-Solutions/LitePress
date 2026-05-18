using LiteBus.Commands.Abstractions;
namespace LiteNova.Blog.Application.Tags.CreateTag;
public sealed record CreateTagCommand(string Name) : ICommand<CreateTagResult>;
