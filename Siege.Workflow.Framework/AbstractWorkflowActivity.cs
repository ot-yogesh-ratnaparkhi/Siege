using Siege.ServiceLocation;

namespace Siege.Workflow.Framework
{
    public interface IWorkflowActivity
    {
        Workflow SetWorkflow(Workflow workflow);
        void Process(IContract contract);
    }

    public abstract class AbstractWorkflowActivity : AbstractWorkflowActivity<IContract>
    {
        protected AbstractWorkflowActivity(IContextualServiceLocator kernel)
            : base(kernel)
        {
        }
    }

    public abstract class AbstractWorkflowActivity<T> : Workflow, IWorkflowActivity
        where T : class, IContract
    {
        protected abstract void Invoke(T contract);

        protected AbstractWorkflowActivity(IContextualServiceLocator kernel)
            : base(kernel)
        {
        }

        public override void Process(IContract contract)
        {
            Invoke((T)contract);

            if (shouldBreak) return;

            foreach (Workflow workflow in this.sequence)
            {
                workflow.Process(contract);
            }
        }
    }
}