using Siege.ServiceLocation;
using Siege.Workflow.Framework.UnitTests.TestContracts;

namespace Siege.Workflow.Framework.UnitTests.TestActivities
{
    public interface IEmailBackOrderNotificationActivity : IWorkflowActivity
    {
    }

    public class EmailBackOrderNotificationActivity : AbstractWorkflowActivity<INotificationContract>, IEmailBackOrderNotificationActivity
    {
        public EmailBackOrderNotificationActivity(IContextualServiceLocator kernel) : base(kernel)
        {
        }

        protected override void Invoke(INotificationContract contract)
        {
            
        }
    }
}
