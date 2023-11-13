using SocialMedia.Application.Services;

namespace SocialMedia.Api.Registrars;
public class ApplicationLayerRegistrar : IWebApplicationBuilderRegistrar
{
    public void RegisterServices(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IdentityServices>();
    }
}
