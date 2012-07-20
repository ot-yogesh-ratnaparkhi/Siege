//using FluentNHibernate.Mapping;

//namespace Siege.Security.SQL.Mappings
//{
//    public class PermissionMap : ClassMap<Permission>
//    {
//        public PermissionMap()
//        {
//            Table("aspnet_Permissions");
//            Id(x => x.ID, "PermissionId");
//            Map(x => x.Name, "PermissionName");
//            Map(x => x.Description);
//            Map(x => x.IsActive);
//            Map(x => x.ExcludeFromAssignment);
            
//            Cache.ReadWrite();
//        }
//    }
//}