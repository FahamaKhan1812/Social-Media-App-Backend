using AutoMapper;
using SocialMedia.Api.Contracts.Posts.Responses;
using SocialMedia.Api.Contracts.UserProfile.Requests;
using SocialMedia.Api.Contracts.UserProfile.Responses;
using SocialMedia.Application.UserProfile.Commands;
using SocialMedia.Domain.Aggregates.UserProfileAggregate;

namespace SocialMedia.Api.MappingProfiles;
public class UserProfileMappings : Profile
{
    public UserProfileMappings()
    {
        CreateMap<UserProfile, UserProfileResponse>();
        CreateMap<BasicInfo, BasicInformation>();
        CreateMap<UserProfileCreateUpdate, UpdateUserCommand>();
        CreateMap<UserProfile, InteractionUser>()
              .ForMember(dest => dest.FullName, opt
              => opt.MapFrom(src
              => src.BasicInfo.FirstName + " " + src.BasicInfo.LastName))
              .ForMember(dest => dest.City, opt
              => opt.MapFrom(src => src.BasicInfo.CurrentCity));
    }
}
