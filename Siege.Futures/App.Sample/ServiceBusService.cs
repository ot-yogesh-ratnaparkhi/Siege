using System.Web.Security;
using App.Sample.Services;
using App.Sample.Subscribers;
using Siege.ServiceLocator.Extensions.ExtendedRegistrationSyntax;
using Web.Sample.Messages;
using Siege.Integration.WCF;
using Siege.Eventing.WindowsServices;

namespace App.Sample
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

            //MapMessage<LogOnAccountMessage, NativeAdapter>();

            AddSubscriber<AccountSubscriber, LogOnAccountMessage>();

            StartService<WCFService>();
        }
    }
}