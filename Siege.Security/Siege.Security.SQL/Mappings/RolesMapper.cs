using System.Data.Entity.ModelConfiguration;

namespace Siege.Security.SQL.Mappings
{
    public class RolesMapper : DbContextMapper<Role>
    {
        public override void Build(EntityTypeConfiguration<Role> configuration)
        {
            configuration.HasKey(x => x.ID);
            configuration.Property(x => x.ID).HasColumnName("RoleID");
            configuration.HasRequired(x => x.Consumer).WithMany(x => x.Roles).Map(m => m.MapKey("ConsumerID"));
            configuration.HasMany(x => x.Permissions)
                .WithMany(x => x.Roles)
                .Map(map =>
                {
                    map.ToTable("PermissionsInRoles");
                    map.MapLeftKey("RoleID");
                    map.MapRightKey("PermissionID");
                });

            configuration.HasMany(x => x.Groups)
                .WithMany(x => x.Roles)
                .Map(map =>
                {
                    map.ToTable("RolesInGroups");
                    map.MapLeftKey("RoleID");
                    map.MapRightKey("GroupID");
                });

            configuration.HasMany(x => x.Users)
                .WithMany(x => x.Roles)
                .Map(map =>
                {
                    map.ToTable("UsersInRoles");
                    map.MapLeftKey("RoleID");
                    map.MapRightKey("UserID");
                });
        }
    }
}