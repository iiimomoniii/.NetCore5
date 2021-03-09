using System.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Hero_Project.Data;
using Hero_Project.NetCore5.Interfaces;
using Hero_Project.NetCore5.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace Hero_Project
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Hero_Project", Version = "v1" });
            });
            // use Microsoft.EntityFrameworkCore;
            // DI Dependency Injection
            services.AddDbContext<DatabaseContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("ConnectionSQLServer")));


        }

        //last name of Interfaces filename has Service word will DI to Regiter by Autofac (6.1.0)
        public void ConfigureContainer(ContainerBuilder builder){
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
            .Where(t => t.Name.EndsWith("Services"))
            .AsImplementedInterfaces();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //Development
            if (env.IsDevelopment())
            {
                //exception on dev
                app.UseDeveloperExceptionPage();
                //Document for API
                //Type Json
                app.UseSwagger();
                //UI
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Hero_Project v1"));
            }
            //Production
            //Middleware 
            //redirect http to https
            app.UseHttpsRedirection();
            //routing
            app.UseRouting();
            //authen
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
