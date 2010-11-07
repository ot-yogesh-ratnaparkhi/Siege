using System.ServiceModel;
using System.ServiceProcess;
using System.Web.Security;
using Courier.Sample.Messages;
using CourierApp.Services;
using CourierApp.Subscribers;
using Siege.Courier;
using Siege.Courier.Messages;
using Siege.Courier.WCF;
using Siege.Requisitions;
using Siege.Requisitions.Extensions.ExtendedRegistrationSyntax;
using Siege.Requisitions.SiegeAdapter;

namespace CourierApp
{
    public partial class ServiceBusService : ServiceBase
    {
        public ServiceHost ServiceHost;
        private readonly IServiceLocator locator = new ThreadedServiceLocator(new SiegeAdapter());

        public ServiceBusService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            var map = new MessageMap();

            locator
                .Register(Given<MessageMap>.Then(map))
                .Register(Given<DelegateManager>.Then<DelegateManager>())
                .Register(Given<NativeAdapter>.Then<NativeAdapter>())
                .Register(Given<MembershipProvider>.Then(Membership.Provider))
                .Register(Given<IMembershipService>.Then<NullMembershipService>())
                .Register(Given<IServiceBus>.Then<SimpleServiceBus>())
                .Register(Given<IWCFProtocol>.Then<WCFService>())
                .Register(Given<AccountSubscriber>.Then<AccountSubscriber>())
                .Register(Given<IMessageTracker>.Then<ThreadedMessageTracker>())
                .Register(Given<IMessageBucket>.Then<ThreadedMessageBucket>())
                .Register(Given<IServiceBus>.InitializeWith(bus => bus.Subscribe(locator.GetInstance<AccountSubscriber>())));

            map.Map<LogOnAccountMessage>(locator.GetInstance<NativeAdapter>());

            if (ServiceHost != null)
            {
                ServiceHost.Close();
            }

            ServiceHost = new ServiceHost(locator.GetInstance<WCFService>());
            ServiceHost.Open();
        }

        protected override void OnStop()
        {
            if (ServiceHost != null)
            {
                ServiceHost.Close();
                ServiceHost = null;
            }
        }
    }
}