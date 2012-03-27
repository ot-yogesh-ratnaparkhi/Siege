using System.Data.Entity;

namespace Siege.Security.SQL.Mappings
{
    public class SecurityContext : DbContext
    {
        public DbSet<Application> Applications { get; set; }
        public DbSet<Consumer> Consumers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            new ApplicationMapper().Map(modelBuilder);
            new ConsumerMapper().Map(modelBuilder);
            new GroupMapper().Map(modelBuilder);
            new PermissionsMapper().Map(modelBuilder);
            new RolesMapper().Map(modelBuilder);
            new UserMapper().Map(modelBuilder);
        }
    }
}