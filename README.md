# .NetCore Version 5

https://dotnet.microsoft.com/download/dotnet/5.0
installation
- dotnet tool install --global dotnet-ef

# Database SQL server on Docker
- docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=Tel1234!" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2019-CU9-ubuntu-16.04

# Database SQL server on Centos7
- https://docs.microsoft.com/en-us/sql/linux/quickstart-install-connect-red-hat?view=sql-server-ver15

## How to connect
- dotnet ef dbcontext scaffold "Server=localhost,1433;user id=sa; password=Tel1234!; Database=istock;" Microsoft.EntityFrameworkCore.SqlServer -o Entities -c DatabaseContext --context-dir Data
## DB Script

iiimomoniii/DB .NetCore5


## Nuget Package installed
- Autofac	6.1.0	
- Autofac.Extensions.DependencyInjection	7.1.0	
- Mapster	7.1.5	
- Microsoft.AspNetCore.Authentication.JwtBearer	5.0.4	
- Microsoft.EntityFrameworkCore.Design	5.0.3	 
- Microsoft.EntityFrameworkCore.SqlServer	5.0.3	
- Swashbuckle.AspNetCore
