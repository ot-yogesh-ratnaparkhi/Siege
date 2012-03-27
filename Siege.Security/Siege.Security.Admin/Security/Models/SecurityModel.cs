using System.Collections.Generic;

namespace Siege.Security.Admin.Security.Models
{
    public class SecurityModel
    {
        public int? UserID { get; set; }
        public IList<Application> Applications { get; set; }
        public IList<Consumer> Consumers { get; set; } 
    }
}