using LiteNova.Blog.Api.Models.Requests;
using LiteNova.Blog.Api.Models.Responses;
using LiteNova.Blog.Application.Tags.Commands.CreateTag;
using LiteNova.Blog.Application.Tags.Queries.GetAllTags;
using Mapster;

namespace LiteNova.Blog.Api.Mappers;

public static class TagMapper
{
    public static void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateTagRequest, CreateTagCommand>();
        config.NewConfig<GetAllTagsQueryResult, TagResponse>();
    }
}
