dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet ef dbcontext scaffold "Server=localhost;Database=Autobarn;User=autobarn;Password=autobarn" Microsoft.EntityFrameworkCore.SqlServer -o Entities -c AutobarnDbContext