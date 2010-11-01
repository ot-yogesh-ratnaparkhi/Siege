using System.ServiceModel;
using System.ServiceProcess;
using System.Web.Security;
using CourierApp.Subscribers;
using CourierSample.Security;
using Siege.Courier;
using Siege.Courier.WCF;
using Siege.Requisitions;
using Siege.Requisitions.Extensions.ExtendedRegistrationSyntax;
using Siege.Requisitions.RegistrationPolicies;
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
            locator
                .Register(Given<MembershipProvider>.Then(Membership.Provider))
                .Register(Given<IMembershipService>.Then<AccountMembershipService>())
                .Register<Singleton>(Given<IServiceBus>.Then<SimpleServiceBus>())
                .Register(Given<IWCFProtocol>.Then<WCFService>())
                .Register(Given<AccountSubscriber>.Then<AccountSubscriber>())
                .Register(Given<IServiceBus>.InitializeWith(bus => bus.Subscribe(new AccountSubscriber(locator.GetInstance<IMembershipService>(), bus))));

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