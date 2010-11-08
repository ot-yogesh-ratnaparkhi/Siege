using System.ServiceModel;
using System.Web.Security;
using Courier.Sample.Messages;
using CourierApp.Services;
using CourierApp.Subscribers;
using Siege.Courier;
using Siege.Courier.WCF;
using Siege.Courier.WindowsServices;
using Siege.Requisitions.Extensions.ExtendedRegistrationSyntax;

namespace CourierApp
{
    public partial class ServiceBusService : CourierService
    {
        public ServiceBusService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            base.OnStart(args);

            ServiceLocator
                .Register(Given<MembershipProvider>.Then(Membership.Provider))
                .Register(Given<IMembershipService>.Then<NullMembershipService>())
                .Register(Given<IWCFProtocol>.Then<WCFService>());
                
            MapMessage<LogOnAccountMessage, NativeAdapter>();

            AddSubscriber<AccountSubscriber, LogOnAccountMessage>();

            StartService<WCFService>();
        }
    }
}