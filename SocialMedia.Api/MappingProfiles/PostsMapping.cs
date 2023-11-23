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
        CreateMap<PostInteraction, PostInteractionResponse>()
             .ForMember(dest
                => dest.Type, opt
                => opt.MapFrom(src
                => src.InteractionType.ToString()))
            .ForMember(dest 
                => dest.Author, opt
                => opt.MapFrom(src 
                => src.UserProfile));
    }
}
