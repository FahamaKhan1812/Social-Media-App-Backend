using SocialMedia.Application.UserProfile.Queries;
using MediatR;
using SocialMedia.Application.Posts.Queries;

namespace SocialMedia.Api.Registrars;
public class BogardRegistrar : IWebApplicationBuilderRegistrar
{
    public void RegisterServices(WebApplicationBuilder builder)
    {
        builder.Services.AddAutoMapper(typeof(Program), typeof(GetAllUserProfiles));
        builder.Services.AddMediatR(typeof(GetAllUserProfiles));
    }
}
