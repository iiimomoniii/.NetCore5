using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hero_Project.NetCore5.Installers_Libraries
{
    public interface IInstallers
    {
        void InstallServices(IServiceCollection services, IConfiguration configuration);
    }
}