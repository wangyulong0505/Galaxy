using Abp.EntityFrameworkCore;
using Galaxy.Entities;
using Microsoft.EntityFrameworkCore;

namespace Galaxy.EntityFrameworkCore
{
    public class GalaxyDbContext : AbpDbContext
    {
        //Add DbSet properties for your entities...
        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<Role> Roles { get; set; }

        public virtual DbSet<Organization> Organization { get; set; }

        public virtual DbSet<Document> Documents { get; set; }

        public virtual DbSet<Message> Messages { get; set; }

        public virtual DbSet<Menu> Menus { get; set; }

        public GalaxyDbContext(DbContextOptions<GalaxyDbContext> options) 
            : base(options)
        {
            //
        }
    }
}
