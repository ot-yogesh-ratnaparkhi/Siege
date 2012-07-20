namespace Siege.Security.Admin.Security.Models
{
    public class UserModel : SecurityModel
    {
        public virtual string Name { get; set; }
        public virtual string Password { get; set; }
        public virtual bool IsActive { get; set; }

        public virtual bool IsNew
        {
            get { return UserID == null; }
        }
    }
}