using Hero_Project.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hero_Project.NetCore5.Installers_Libraries
{
    public class DatabaseInstaller : IInstallers
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
             // use Microsoft.EntityFrameworkCore;
            // DI Dependency Injection
            services.AddDbContext<DatabaseContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("ConnectionSQLServer")));
        }
    }
}