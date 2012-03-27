//using FluentNHibernate.Mapping;

//namespace Siege.Security.SQL.Mappings
//{
//    public class UserMap : ClassMap<User>
//    {
//        public UserMap()
//        {
//            Id(x => x.ID, "UserId").GeneratedBy.Guid();
//            Map(x => x.Name, "UserName");
//            Cache.ReadWrite();

//            References(x => x.Consumer);

//            HasManyToMany(x => x.Groups)
//                .ParentKeyColumn("UserId")
//                .ChildKeyColumn("GroupId")
//                .Table("aspnet_UsersInGroups")
//                .Cascade.SaveUpdate();

//            HasManyToMany(x => x.Roles)
//                .Table("aspnet_UsersInRoles")
//                .ParentKeyColumn("UserId")
//                .ChildKeyColumn("RoleId")
//                .Cascade.SaveUpdate();
//        }
//    }
//}