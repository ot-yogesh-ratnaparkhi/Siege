using Siege.ServiceLocation;

namespace Siege.Workflow.Framework.Activities
{
    public class BreakActivity : AbstractWorkflowActivity
    {
        public BreakActivity(IContextualServiceLocator serviceLocator, IContract contract)
            : base(serviceLocator, contract)
        {
        }

        protected override void Invoke(IContract contract)
        {
            Break();
        }
    }
}