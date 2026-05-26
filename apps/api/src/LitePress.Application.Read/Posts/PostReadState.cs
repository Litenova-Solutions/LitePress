namespace LitePress.Application.Read.Posts;

internal static class PostReadState
{
    internal static string ResolveLabel(PostState state) =>
        state switch
        {
            ArchivedPostState => "Archived",
            PublishedPostState => "Published",
            DraftPostState => "Draft",
            _ => throw new InvalidOperationException($"Unknown post state: {state.GetType().Name}")
        };
}
