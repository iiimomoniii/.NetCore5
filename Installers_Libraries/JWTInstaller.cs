using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Hero_Project.NetCore5.Installers_Libraries
{
    public class JWTInstaller : IInstallers
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            //get param from appsettings.json
            var jwtSettings = new JWTSettings();
            //map param from appsettings.json to class jwtSettings
            configuration.Bind(nameof(jwtSettings), jwtSettings);
            //use param from appsettings.json to access service
            services.AddSingleton(jwtSettings);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => {
                options.TokenValidationParameters = new TokenValidationParameters{
                    //owner of token
                    ValidateIssuer = true,
                    //token name
                    ValidIssuer = jwtSettings.Issuer,
                    //assign token to user "yyy"
                    ValidateAudience = true,
                    //user "yyy"
                    ValidAudience = jwtSettings.Audience,
                    //duration time of token
                    ValidateLifetime = true,
                    //validate key for authen
                    ValidateIssuerSigningKey = true,
                    //key for authen
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),
                    //delay time of token in case delay time is zero
                    ClockSkew = TimeSpan.Zero
                };
            });
        }
         public class JWTSettings {
             public string Key { get; set;}
             public string Issuer { get; set; }

             public string Audience { get; set; }

             public string Expire { get; set; }
         }

    }
   }