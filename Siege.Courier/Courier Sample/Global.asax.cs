using Courier_Sample.Controllers;
using Siege.Courier.Messages;
using Siege.Courier.WCF;
using Siege.Courier.Web;
using Siege.Requisitions;
using Siege.Requisitions.Extensions.Conventions;
using Siege.Requisitions.Extensions.ExtendedRegistrationSyntax;
using Siege.Requisitions.SiegeAdapter;
using Siege.Requisitions.Web.Conventions;

namespace Courier_Sample
{
    public class MvcApplication : CourierHttpApplication
    {
        protected override IServiceLocatorAdapter GetServiceLocatorAdapter()
        {
            return new SiegeAdapter();
        }

        protected override void OnApplicationStarted()
        {
            ServiceLocator
                .Register(Using.Convention<ControllerConvention<HomeController>>())
                .Register(Given<IConfigurationManager>.Then<ServiceBusConfigurationManager>())
                .Register(Given<IChannelManagerFactory>.Then<WCFChannelManagerFactory>())
                .Register(Given<IChannelManager<IWCFProtocol>>.ConstructWith(x =>
                {
                    var config = x.GetInstance<IConfigurationManager>();
                    return x.GetInstance<WCFChannelManagerFactory>().Create<IWCFProtocol>(config.ServiceBusEndPoint);
                }))
                .Register(Given<WCFProxy<IWCFProtocol>>.Then<WCFProxy<IWCFProtocol>>());
            
            AddSubcriber<WCFSubscriber, IMessage>();

            base.OnApplicationStarted();
        }
    }
}