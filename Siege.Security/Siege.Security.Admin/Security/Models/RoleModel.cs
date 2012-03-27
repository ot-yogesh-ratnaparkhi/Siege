namespace Siege.Security.Admin.Security.Models
{
    public class RoleModel
    {
        public int? ApplicationID { get; set; }
        public int? RoleID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }

        public bool IsNew
        {
            get { return RoleID == null; }
        }
    }
}