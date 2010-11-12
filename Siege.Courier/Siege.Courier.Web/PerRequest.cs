using System.Web;
using Siege.Requisitions;
using Siege.Requisitions.InternalStorage;
using Siege.Requisitions.RegistrationPolicies;
using Siege.Requisitions.Resolution.Pipeline;

namespace Siege.Courier.Web
{
    public class PerRequest : AbstractRegistrationPolicy
    {
        public override object ResolveWith(IInstanceResolver locator, IServiceLocatorStore context, PostResolutionPipeline pipeline)
        {
            if (!HttpContext.Current.Items.Contains(registration.GetMappedToType()))
            {
                HttpContext.Current.Items[registration.GetMappedToType()] = registration.ResolveWith(locator, context, pipeline);
            }

            return HttpContext.Current.Items[registration.GetMappedToType()];
        }
    }
}