using AutoMapper;
using SocialMedia.Api.Contracts.Identity;
using SocialMedia.Application.Identity.Commands;

namespace SocialMedia.Api.MappingProfiles;

public class IdentityMappings : Profile
{
    public IdentityMappings()
    {
        CreateMap<UserRegistrationContract, RegisterCommand>();
        CreateMap<LoginContract, LoginCommand>();
    }
}
