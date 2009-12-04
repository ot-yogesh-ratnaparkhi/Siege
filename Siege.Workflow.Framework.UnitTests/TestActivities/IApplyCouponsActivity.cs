using System;
using Siege.ServiceLocation;
using Siege.Workflow.Framework.UnitTests.TestContracts;

namespace Siege.Workflow.Framework.UnitTests.TestActivities
{
    public interface IApplyCouponsActivity : IWorkflowActivity
    {
    }

    public class ApplyCouponsActivity : AbstractWorkflowActivity<ICouponContract>, IApplyCouponsActivity
    {
        public ApplyCouponsActivity(IContextualServiceLocator kernel) : base(kernel)
        {
        }

        protected override void Invoke(ICouponContract contract)
        {
        }
    }
}
