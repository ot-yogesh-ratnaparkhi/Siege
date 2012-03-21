using FluentNHibernate.Mapping;

namespace Siege.Security.SQL.Mappings
{
    public class RoleMap : ClassMap<Role>
    {
        public RoleMap()
        {
            Table("aspnet_Roles");
            Id(x => x.ID, "RoleId").GeneratedBy.Identity();
            Map(x => x.Name, "RoleName");
            Map(x => x.Description);
            Map(x => x.IsActive);
            Cache.ReadWrite();

            References(x => x.Application);

            HasManyToMany(x => x.Permissions)
                .Table("aspnet_PermissionsInRoles")
                .ParentKeyColumn("RoleId")
                .ChildKeyColumn("PermissionId")
                .Cascade.SaveUpdate();
        }
    }
}