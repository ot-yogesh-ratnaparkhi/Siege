using System.Web.Mvc;
using Siege.Requisitions;
using Siege.Requisitions.InternalStorage;
using Siege.Requisitions.RegistrationPolicies;
using Siege.Requisitions.Resolution.Pipeline;

namespace Siege.Courier.Web
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