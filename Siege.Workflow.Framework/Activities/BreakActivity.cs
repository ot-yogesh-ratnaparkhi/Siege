using Siege.ServiceLocation;

namespace Siege.Workflow.Framework.Activities
{
    public class BreakActivity : AbstractWorkflowActivity
    {
        public BreakActivity(IContextualServiceLocator serviceLocator, IContract request)
            : base(serviceLocator, request)
        {
        }

        protected override void Invoke(IContract request)
        {
            Break();
        }
    }
}