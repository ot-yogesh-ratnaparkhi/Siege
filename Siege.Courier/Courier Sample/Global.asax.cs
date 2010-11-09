using Courier.Sample.Messages;
using Courier_Sample.Controllers;
using Courier_Sample.Models;
using Courier_Sample.Subscribers;
using Siege.Courier;
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
            base.OnApplicationStarted();

            ServiceLocator
                .Register(Using.Convention<ControllerConvention<HomeController>>())
                .Register(Given<WCFAdapter>.Then<WCFAdapter>())
                .Register(Given<IConfigurationManager>.Then<ServiceBusConfigurationManager>())
                .Register(Given<IChannelManagerFactory>.Then<WCFChannelManagerFactory>())
                .Register(Given<IChannelManager<IWCFProtocol>>.ConstructWith(x =>
                {
                    var config = x.GetInstance<IConfigurationManager>();
                    return x.GetInstance<WCFChannelManagerFactory>().Create<IWCFProtocol>(config.ServiceBusEndPoint);
                }))
                .Register(Given<WCFProxy<IWCFProtocol>>.Then<WCFProxy<IWCFProtocol>>())
                .Register(Given<AccountSubscriber>.Then<AccountSubscriber>())
                .Register(Given<IMessageBucket>.Then<HttpMessageBucket>())
                .Register(Given<IFormsAuthenticationService>.Then<FormsAuthenticationService>());
            
            MapMessage<LogOnAccountMessage, WCFAdapter>();
            
            AddSubscriber<AccountSubscriber, MemberAuthenticatedMessage>();
            AddSubscriber<AccountSubscriber, MemberFailedAuthenticationMessage>();
            AddSubscriber<AccountSubscriber, MessageValidationFailedMessage<LogOnAccountMessage>>();
        }
    }
}