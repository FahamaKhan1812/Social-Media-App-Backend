using Microsoft.EntityFrameworkCore;
using SocialMedia.Dal.Data;

namespace SocialMedia.Api.Registrars;
public class DbRegistrar : IWebApplicationBuilderRegistrar
{
    public void RegisterServices(WebApplicationBuilder builder)
    {
        var cs = builder.Configuration.GetConnectionString("MSSQLConnector");
        builder.Services.AddDbContext<DataContext>(opt =>
        {
            opt.UseSqlServer(cs);
        });
    }
}
