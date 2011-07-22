using System.Web.Mvc;
using Siege.ServiceLocator;
using Siege.ServiceLocator.InternalStorage;
using Siege.ServiceLocator.RegistrationPolicies;
using Siege.ServiceLocator.Resolution.Pipeline;

namespace Siege.Eventing.Web
{
    public class PerRequest : AbstractRegistrationPolicy
    {
        private TempDataDictionary tempData = new TempDataDictionary();

        public override object ResolveWith(IInstanceResolver locator, IServiceLocatorStore context, PostResolutionPipeline pipeline)
        {
            if (!tempData.ContainsKey(registration.GetMappedToType().ToString()))
            {
                tempData[registration.GetMappedToType().ToString()] = registration.ResolveWith(locator, context, pipeline);
            }

            return tempData[registration.GetMappedToType().ToString()];
        }
    }
}