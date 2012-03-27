using System.Data.Entity.ModelConfiguration;

namespace Siege.Security.SQL.Mappings
{
    public class UserMapper : DbContextMapper<User>
    {
        public override void Build(EntityTypeConfiguration<User> configuration)
        {
            configuration.HasKey(x => x.ID);
            configuration.Property(x => x.ID).HasColumnName("UserID");
            configuration.Property(x => x.Name).HasColumnName("UserName");
            configuration.Property(x => x.Salt).HasColumnName("PasswordSalt");
            configuration.Property(x => x.SecretQuestion).HasColumnName("SecretQuestion");
            configuration.Property(x => x.SecretAnswer).HasColumnName("SecretAnswer");
            configuration.HasRequired(x => x.Consumer).WithMany(x => x.Users).Map(m => m.MapKey("ConsumerID"));
            configuration.Ignore(x => x.IsAuthenticated);
            configuration.Ignore(x => x.Permissions);

            configuration.HasMany(x => x.Roles)
                .WithMany(x => x.Users)
                .Map(map =>
                {
                    map.ToTable("UsersInRoles");
                    map.MapLeftKey("UserID");
                    map.MapRightKey("RoleID");
                });

            configuration.HasMany(x => x.Groups)
                .WithMany(x => x.Users)
                .Map(map =>
                {
                    map.ToTable("UsersInGroups");
                    map.MapLeftKey("UserID");
                    map.MapRightKey("GroupID");
                });

            configuration.HasMany(x => x.Applications)
                .WithMany(x => x.Users)
                .Map(map =>
                {
                    map.ToTable("UsersInApplications");
                    map.MapLeftKey("UserID");
                    map.MapRightKey("ApplicationID");
                });
        }
    }
}