using System.Data.Entity.ModelConfiguration;

namespace Siege.Security.SQL.Mappings
{
    public class GroupMapper : DbContextMapper<Group>
    {
        public override void Build(EntityTypeConfiguration<Group> configuration)
        {
            configuration.HasKey(x => x.ID);
            configuration.Property(x => x.ID).HasColumnName("GroupID");
            configuration.HasRequired(x => x.Consumer).WithMany(x => x.Groups).Map(m => m.MapKey("ConsumerID"));
            configuration.Ignore(x => x.Permissions);
            configuration.HasMany(x => x.Roles)
                .WithMany(x => x.Groups)
                .Map(map =>
                         {
                             map.ToTable("RolesInGroups");
                             map.MapLeftKey("GroupID");
                             map.MapRightKey("RoleID");
                         });

            configuration.HasMany(x => x.Users)
                .WithMany(x => x.Groups)
                .Map(map =>
                         {
                             map.ToTable("UsersInGroups");
                             map.MapLeftKey("GroupID");
                             map.MapRightKey("UserID");
                         });
        }
    }
}