using Galaxy.Configuration;
using Galaxy.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Galaxy.EntityFrameworkCore
{
    /* This class is needed to run EF Core PMC commands. Not used anywhere else */
    public class GalaxyDbContextFactory : IDesignTimeDbContextFactory<GalaxyDbContext>
    {
        public GalaxyDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<GalaxyDbContext>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());

            DbContextOptionsConfigurer.Configure(
                builder,
                configuration.GetConnectionString(GalaxyConsts.ConnectionStringName)
            );

            return new GalaxyDbContext(builder.Options);
        }
    }
}