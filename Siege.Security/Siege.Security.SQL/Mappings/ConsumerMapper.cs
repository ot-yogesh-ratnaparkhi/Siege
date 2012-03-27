using System.Data.Entity.ModelConfiguration;

namespace Siege.Security.SQL.Mappings
{
    public class ConsumerMapper : DbContextMapper<Consumer>
    {
        public override void Build(EntityTypeConfiguration<Consumer> configuration)
        {
            configuration.HasKey(x => x.ID);
            configuration.Property(x => x.ID).HasColumnName("ConsumerID");

            configuration
                .HasMany(x => x.Applications)
                .WithMany(x => x.Consumers)
                .Map(map =>
                {
                    map.ToTable("ConsumersInApplications");
                    map.MapLeftKey("ConsumerID");
                    map.MapRightKey("ApplicationID");
                });

            configuration
                .HasMany(x => x.Users)
                .WithRequired(x => x.Consumer)
                .Map(map => map.MapKey("ConsumerID"));
        }
    }
}