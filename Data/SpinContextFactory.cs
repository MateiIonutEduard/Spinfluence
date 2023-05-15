using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Spinfluence.Data
{
    public class SpinContextFactory : IDesignTimeDbContextFactory<SpinContext>
    {
        /* This fix is needed to Entity Framework to create database migrations successful. */
        public SpinContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configurationRoot = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json").Build();

            var builderContext = new DbContextOptionsBuilder<SpinContext>();
            var connectionString = configurationRoot.GetConnectionString("SpinFluenceDB");

            ServerVersion serverVersion = ServerVersion.AutoDetect(connectionString);
            builderContext.UseMySql(connectionString, serverVersion);
            return new SpinContext(builderContext.Options);
        }
    }
}
