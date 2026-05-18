using LiteNova.Blog.Api.Models.Requests;
using LiteNova.Blog.Api.Models.Responses;
using LiteNova.Blog.Application.Posts.CreatePost;
using LiteNova.Blog.Application.Posts.GetPostBySlug;
using LiteNova.Blog.Application.Posts.GetAllPosts;
using LiteNova.Blog.Application.Posts.UpdatePost;
using Mapster;

namespace LiteNova.Blog.Api.Mappers;

public static class PostMapper
{
    public static void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreatePostRequest, CreatePostCommand>();
        config.NewConfig<(Guid id, UpdatePostRequest request), UpdatePostCommand>()
            .Map(dest => dest.Id, src => src.id)
            .Map(dest => dest.Title, src => src.request.Title)
            .Map(dest => dest.Excerpt, src => src.request.Excerpt)
            .Map(dest => dest.Body, src => src.request.Body)
            .Map(dest => dest.CoverImageUrl, src => src.request.CoverImageUrl)
            .Map(dest => dest.TagIds, src => src.request.TagIds);
        config.NewConfig<GetPostBySlugQueryResult, PostDetailResponse>();
        config.NewConfig<PostSummaryItem, PostSummaryResponse>();
    }
}
