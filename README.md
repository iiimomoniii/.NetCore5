# .NetCore Version 5

https://dotnet.microsoft.com/download/dotnet/5.0
installation
- dotnet tool install --global dotnet-ef

# Database SQL server on Docker
- docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=Tel1234!" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2019-CU9-ubuntu-16.04

## How to connect
- dotnet ef dbcontext scaffold "Server=localhost,1433;user id=sa; password=Tel1234!; Database=istock;" Microsoft.EntityFrameworkCore.SqlServer -o Entities -c DatabaseContext --context-dir Data

