using LiteBus.Commands.Abstractions;

namespace LiteNova.Blog.Application.Tags.CreateTag;

/// <summary>Command to create a new tag.</summary>
public sealed record CreateTagCommand(string Name) : ICommand<CreateTagResult>;
