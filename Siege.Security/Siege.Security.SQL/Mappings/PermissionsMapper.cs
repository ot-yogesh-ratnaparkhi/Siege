using System.Data.Entity.ModelConfiguration;

namespace Siege.Security.SQL.Mappings
{
    public class PermissionsMapper : DbContextMapper<Permission>
    {
        public override void Build(EntityTypeConfiguration<Permission> configuration)
        {
            configuration.HasKey(x => x.ID);
            configuration.Property(x => x.ID).HasColumnName("PermissionID");
            configuration.HasRequired(x => x.Application).WithMany(x => x.Permissions).Map(m => m.MapKey("ApplicationID"));

            configuration.HasMany(x => x.Roles)
                .WithMany(x => x.Permissions)
                .Map(map =>
                {
                    map.ToTable("PermissionsInRoles");
                    map.MapLeftKey("PermissionID");
                    map.MapRightKey("RoleID");
                });
        }
    }
}