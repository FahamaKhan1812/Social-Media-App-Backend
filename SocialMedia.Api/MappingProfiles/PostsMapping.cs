using AutoMapper;
using SocialMedia.Api.Contracts.Posts.Responses;
using SocialMedia.Domain.Aggregates.PostAggregate;
using SocialMedia.Domain.Aggregates.PostAggregates;

namespace SocialMedia.Api.MappingProfiles;
public class PostsMapping : Profile
{
    public PostsMapping()
    {
        CreateMap<Post, PostResponse>();
        CreateMap<PostComment, PostCommentResponse>();
    }
}
