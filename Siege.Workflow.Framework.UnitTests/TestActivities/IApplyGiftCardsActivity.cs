using System;
using Siege.ServiceLocation;
using Siege.Workflow.Framework.UnitTests.TestContracts;

namespace Siege.Workflow.Framework.UnitTests.TestActivities
{
    public interface IApplyGiftCardsActivity : IWorkflowActivity
    {
    }

    public class ApplyGiftCardsActivity : AbstractWorkflowActivity<IGiftCardContract>, IApplyGiftCardsActivity
    {
        public ApplyGiftCardsActivity(IContextualServiceLocator kernel) : base(kernel)
        {
        }

        protected override void Invoke(IGiftCardContract contract)
        {
            
        }
    }

}
