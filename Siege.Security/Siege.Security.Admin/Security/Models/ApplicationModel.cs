using System;

namespace Siege.Security.Admin.Security.Models
{
    public class ApplicationModel
    {
        public int? ApplicationID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public bool IsNew { get { return ApplicationID == null; } }
    }
}