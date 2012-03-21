using FluentNHibernate.Mapping;

namespace Siege.Security.SQL.Mappings
{
    public class ApplicationMap : ClassMap<Application>
    {
        public ApplicationMap()
        {
            Id(x => x.ID, "ApplicationId").GeneratedBy.Guid();
            Map(x => x.Name, "ApplicationName");
            Map(x => x.Description);
            Map(x => x.LoweredName, "LoweredApplicationName");
            Map(x => x.IsActive);
            Cache.ReadWrite();
        }
    }
 }