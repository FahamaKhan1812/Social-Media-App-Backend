using AutoMapper;
using SocialMedia.Api.Contracts.UserProfile.Requests;
using SocialMedia.Api.Contracts.UserProfile.Responses;
using SocialMedia.Application.UserProfile.Commands;
using SocialMedia.Domain.Aggregates.UserProfileAggregate;

namespace SocialMedia.Api.MappingProfiles;
public class UserProfileMappings : Profile
{
    public UserProfileMappings()
    {
        CreateMap<UserProfileCreateUpdate, CreateUserCommand>();
        CreateMap<UserProfile, UserProfileResponse>();
        CreateMap<BasicInfo, BasicInformation>();
        CreateMap<UserProfileCreateUpdate, UpdateUserCommand>();
    }
}
