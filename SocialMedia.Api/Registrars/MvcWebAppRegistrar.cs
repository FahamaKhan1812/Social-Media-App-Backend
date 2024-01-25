using Microsoft.AspNetCore.Mvc.ApiExplorer;
using SocialMedia.Application.Enums;
using SocialMedia.Application.Models;

namespace SocialMedia.Api.Registrars
{
    public class MvcWebAppRegistrar : IWebApplicationRegistrar
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
            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseStatusCodePages(async context =>
            {
                if (context.HttpContext.Response.StatusCode == 401)
                {
                    context.HttpContext.Response.ContentType = "application/json";

                    Error errorResponse = new()
                    {
                        Code= ErrorCode.Unauthorized,
                        Message = "Unauthorized. Please provide a valid JWT."
                    };

                    var jsonResponse = System.Text.Json.JsonSerializer.Serialize(errorResponse);
                    await context.HttpContext.Response.WriteAsync(jsonResponse);
                }
            });
            app.UseAuthorization();

            app.MapControllers();

        }
    }
}
