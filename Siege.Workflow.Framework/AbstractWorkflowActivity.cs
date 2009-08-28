using Siege.ServiceLocation;

namespace Siege.Workflow.Framework
{
    public interface IWorkflowActivity
    {
        Workflow SetWorkflow(Workflow workflow);
        void Process();
    }

    public abstract class AbstractWorkflowActivity : AbstractWorkflowActivity<IContract>
    {
        protected AbstractWorkflowActivity(IContextualServiceLocator kernel, IContract contract)
            : base(kernel, contract)
        {
        }
    }

    public abstract class AbstractWorkflowActivity<T> : Workflow, IWorkflowActivity
        where T : class, IContract
    {
        protected abstract void Invoke(T contract);

        protected AbstractWorkflowActivity(IContextualServiceLocator kernel, IContract contract)
            : base(kernel, contract)
        {
        }

        public virtual void Process(T contract)
        {
            Invoke(contract);

            if (shouldBreak) return;

            foreach (Workflow workflow in this.sequence)
            {
                workflow.Process();
            }
        }

        public override void Process()
        {
            Process(contract as T);
        }
    }
}