using System.Web;
using Siege.Security.Principals;

namespace Siege.Security.SampleApplication.Areas.Security.Models
{
    public class SecurityModel
    {
        public ISecurityPrincipal User { get; private set; }

        public SecurityModel()
        {
            User = (ISecurityPrincipal)HttpContext.Current.User;
        }
    }
}