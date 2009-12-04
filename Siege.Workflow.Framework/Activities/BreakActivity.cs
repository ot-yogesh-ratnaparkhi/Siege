using Siege.ServiceLocation;

namespace Siege.Workflow.Framework.Activities
{
    public class BreakActivity : AbstractWorkflowActivity
    {
        public BreakActivity(IContextualServiceLocator serviceLocator)
            : base(serviceLocator)
        {
        }

        protected override void Invoke(IContract contract)
        {
            Break();
        }
    }
}