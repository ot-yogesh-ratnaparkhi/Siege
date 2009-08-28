using System;
using Siege.ServiceLocation;
using Siege.Workflow.Framework.UnitTests.TestContracts;

namespace Siege.Workflow.Framework.UnitTests.TestActivities
{
    public interface IChargeCreditCardActivity : IWorkflowActivity
    {
    }

    public class ChargeCreditCardActivity : AbstractWorkflowActivity<ICreditCardContract>, IChargeCreditCardActivity
    {
        public ChargeCreditCardActivity(IContextualServiceLocator kernel, IContract contract) : base(kernel, contract)
        {
        }

        protected override void Invoke(ICreditCardContract contract)
        {
            
        }
    }
}
