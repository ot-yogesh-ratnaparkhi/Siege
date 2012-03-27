//using FluentNHibernate.Mapping;
//using Siege.Security.Entities;

//namespace Siege.Security.SQL.Mappings
//{
//    public class GroupMap : ClassMap<Group>
//    {
//        public GroupMap()
//        {
//            Table("aspnet_Groups");
//            Id(x => x.ID, "GroupId").GeneratedBy.Identity();
//            Map(x => x.Name, "GroupName");
//            Map(x => x.Description);
//            Map(x => x.IsActive);
//            Cache.ReadWrite();

//            References(x => x.Consumer);
            
//            HasManyToMany(x => x.Roles)
//                .Table("aspnet_RolesInGroups")
//                .ParentKeyColumn("GroupId")
//                .ChildKeyColumn("RoleId")
//                .Cascade.SaveUpdate();
//        }
//    }
//}