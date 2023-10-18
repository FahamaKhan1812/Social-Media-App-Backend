using AutoMapper;
using SocialMedia.Application.UserProfile.Commands;
using SocialMedia.Domain.Aggregates.UserProfileAggregate;

namespace SocialMedia.Application.MappingProfiles;
internal class UserProfileMap : Profile
{
    public UserProfileMap()
    {
        CreateMap<CreateUserCommand, BasicInfo>();
    }
}
