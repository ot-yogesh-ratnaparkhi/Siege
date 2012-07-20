using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;

namespace Siege.Security.SQL.Mappings
{
    public abstract class DbContextMapper<T> where T : class
    {
        public abstract void Build(EntityTypeConfiguration<T> configuration);
        public void Map(DbModelBuilder modelBuilder)
        {
            Build(modelBuilder.Entity<T>());
        }
    }
}