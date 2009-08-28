using System;
using Siege.ServiceLocation;
using Siege.Workflow.Framework.UnitTests.TestContracts;

namespace Siege.Workflow.Framework.UnitTests.TestActivities
{
    public interface ICreateInvoiceActivity : IWorkflowActivity
    {
    }

    public class CreateInvoiceActivity : AbstractWorkflowActivity<IInvoiceContract>, ICreateInvoiceActivity
    {
        public CreateInvoiceActivity(IContextualServiceLocator kernel, IContract contract) : base(kernel, contract)
        {
        }

        protected override void Invoke(IInvoiceContract contract)
        {
            
        }
    }
}
