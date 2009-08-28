using Siege.ServiceLocation;
using Siege.Workflow.Framework.UnitTests.TestContracts;

namespace Siege.Workflow.Framework.UnitTests.TestActivities
{
    public interface IRefundGiftCardsActivity : IWorkflowActivity
    {
    }

    public class RefundGiftCardsActivity : AbstractWorkflowActivity<IGiftCardContract>, IRefundGiftCardsActivity
    {
        public RefundGiftCardsActivity(IContextualServiceLocator kernel, IContract contract) : base(kernel, contract)
        {
        }

        protected override void Invoke(IGiftCardContract contract)
        {
            
        }
    }
}
