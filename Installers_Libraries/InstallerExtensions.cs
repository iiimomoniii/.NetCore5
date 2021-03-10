using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hero_Project.NetCore5.Installers_Libraries
{
    public static class InstallerExtensions
    {
        public static void InstallServiceInAssembly(this IServiceCollection services, IConfiguration configulation){
            //find service in Startup.cs by where and typeof  
            var installers = typeof(Startup).Assembly.ExportedTypes.Where(x => 
                // service is not Interface and Abstract Class
                typeof(IInstallers).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                //select Inheritance IInstallers List
                .Select(Activator.CreateInstance).Cast<IInstallers>().ToList();

            //register services
            installers.ForEach(installer => installer.InstallServices(services, configulation));
        }
    }
}