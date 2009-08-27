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
        protected AbstractWorkflowActivity(IContextualServiceLocator kernel, IContract request)
            : base(kernel, request)
        {
        }
    }

    public abstract class AbstractWorkflowActivity<T> : Workflow, IWorkflowActivity
        where T : class, IContract
    {
        protected abstract void Invoke(T request);

        protected AbstractWorkflowActivity(IContextualServiceLocator kernel, IContract request)
            : base(kernel, request)
        {
        }

        public virtual void Process(T request)
        {
            Invoke(request);

            if (shouldBreak) return;

            foreach (Workflow workflow in this.sequence)
            {
                workflow.Process();
            }
        }

        public override void Process()
        {
            Process(request as T);
        }
    }
}