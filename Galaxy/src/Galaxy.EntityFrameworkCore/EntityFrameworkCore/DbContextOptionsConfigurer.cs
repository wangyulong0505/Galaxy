using Microsoft.EntityFrameworkCore;

namespace Galaxy.EntityFrameworkCore
{
    public static class DbContextOptionsConfigurer
    {
        public static void Configure(
            DbContextOptionsBuilder<GalaxyDbContext> dbContextOptions, 
            string connectionString
            )
        {
            /* This is the single point to configure DbContextOptions for GalaxyDbContext */
            dbContextOptions.UseSqlServer(connectionString);
        }

        public static void ConfigureMySql(
            DbContextOptionsBuilder<GalaxyDbContext> dbContextOptions,
            string connectionString
            )
        {
            /* This is the single point to configure DbContextOptions for GalaxyDbContext */
            dbContextOptions.UseMySQL(connectionString);
        }
    }
}
