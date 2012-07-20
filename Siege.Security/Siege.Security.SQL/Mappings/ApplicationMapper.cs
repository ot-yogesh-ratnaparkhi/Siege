using System.Data.Entity.ModelConfiguration;

namespace Siege.Security.SQL.Mappings
{
    public class ApplicationMapper : DbContextMapper<Application>
    {
        public override void Build(EntityTypeConfiguration<Application> configuration)
        {
            configuration.HasKey(x => x.ID);
            configuration.HasKey(x => x.ID);
            configuration.Property(x => x.ID).HasColumnName("ApplicationID");

            configuration.HasMany(x => x.Consumers)
                .WithMany(x => x.Applications)
                .Map(map =>
                {
                    map.ToTable("ConsumersInApplications");
                    map.MapLeftKey("ApplicationID");
                    map.MapRightKey("ConsumerID");
                });

            configuration.HasMany(x => x.Users)
                .WithMany(x => x.Applications)
                .Map(map =>
                {
                    map.ToTable("UsersInApplications");
                    map.MapLeftKey("ApplicationID");
                    map.MapRightKey("UserID");
                });

            configuration.HasMany(x => x.Permissions)
                .WithRequired(x => x.Application)
                .Map(map => map.MapKey("ApplicationID"));
        }
    }
}