using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace SocialMedia.Api.Registrars;
public class SwaggerWebAppRegistrar : IWebApplicationRegistrar
{
    public void RegisterPipelineComponents(WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(option =>
        {
            var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
            foreach (var desc in provider.ApiVersionDescriptions)
            {
                option.SwaggerEndpoint($"/swagger/{desc.GroupName}/swagger.json", desc.ApiVersion.ToString());
            }
        });
    }
}
