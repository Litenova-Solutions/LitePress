using LiteBus.Commands.Abstractions;

namespace LiteNova.Blog.Application.Tags.DeleteTag;

/// <summary>Command to delete a tag.</summary>
public sealed record DeleteTagCommand(Guid Id) : ICommand;
