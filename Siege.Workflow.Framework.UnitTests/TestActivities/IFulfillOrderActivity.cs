using Siege.ServiceLocation;
using Siege.Workflow.Framework.UnitTests.TestContracts;

namespace Siege.Workflow.Framework.UnitTests.TestActivities
{
    public interface IFulfillOrderActivity : IWorkflowActivity
    {
        OrderStatus Status { get; }
    }

    public class FulfillOrderActivity : AbstractWorkflowActivity<IOrderContract>, IFulfillOrderActivity
    {
        public FulfillOrderActivity(IContextualServiceLocator kernel, IContract contract) : base(kernel, contract)
        {
        }

        protected override void Invoke(IOrderContract contract)
        {

        }

        public OrderStatus Status { get; set; }
    }

    public enum OrderStatus
    {
        Processing,
        BackOrder,
        Complete
    }
}
