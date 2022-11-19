global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.Design;
global using Microsoft.Extensions.Configuration;

namespace SeerbitHackaton.Core.DataAccess.EfCore.Context
{
    public class AppDbContextFactory<T> : IDesignTimeDbContextFactory<T> where T : DbContext
    {
        public virtual T CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
              .AddEnvironmentVariables()
             .Build();

            IConfigurationRoot configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
               .AddJsonFile("appsettings.Development.json", optional: true)
               .AddJsonFile($"appsettings.{config["ASPNETCORE_ENVIRONMENT"]}.json", optional: true)
               .Build();

            var builder = new DbContextOptionsBuilder<T>();
            builder.EnableSensitiveDataLogging(true);
            var connectionString = configuration["ConnectionStrings:Default"];
            //var connectionString = "Data source=52.247.216.167,1433; Initial Catalog=mint.dev.messaging; integrated security=true;MultipleActiveResultSets=true;Trusted_Connection=false;User Id=sa;Password=microsoft_;";
            builder.UseSqlServer(connectionString, b => b.MigrationsAssembly(this.GetType().Assembly.FullName));
            var dbContext = (T)Activator.CreateInstance(typeof(T), builder.Options);
            return dbContext;
        }
    }
}
