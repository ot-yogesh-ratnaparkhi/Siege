using Siege.ServiceLocation;
using Siege.Workflow.Framework.UnitTests.TestContracts;

namespace Siege.Workflow.Framework.UnitTests.TestActivities
{
    public interface IEmailOrderConfirmationActivity : IWorkflowActivity
    {
    }

    public class EmailOrderConfirmationActivity : AbstractWorkflowActivity<INotificationContract>, IEmailOrderConfirmationActivity
    {
        public EmailOrderConfirmationActivity(IContextualServiceLocator kernel, IContract contract) : base(kernel, contract)
        {
        }

        protected override void Invoke(INotificationContract contract)
        {
            
        }
    }
}
