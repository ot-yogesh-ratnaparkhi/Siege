using System;
using Siege.ServiceLocation;

namespace Siege.Workflow.Framework.Activities
{
    public class WorkflowActivity : WorkflowActivity<ActionActivity>
    {
        public WorkflowActivity(IContextualServiceLocator serviceLocator)
            : base(serviceLocator)
        {
        }

        public Workflow For(Action action)
        {
            activity = serviceLocator.GetInstance<ActionActivity>();

            activity.SetWorkflow(this);
            activity.With(action);

            return parentWorkflow;
        }
    }

    public class WorkflowActivity<TActivityType> : AbstractWorkflowActivity
        where TActivityType : IWorkflowActivity
    {
        protected TActivityType activity;

        public WorkflowActivity(IContextualServiceLocator locator)
            : base(locator)
        {
            activity = locator.GetInstance<TActivityType>();
        }

        public Workflow Then()
        {
            parentWorkflow.Then<TActivityType>();

            return parentWorkflow;
        }

        public Workflow CaptureResult(Action<TActivityType> action)
        {
            CaptureResultActivity<TActivityType> captureResult = serviceLocator.GetInstance<CaptureResultActivity<TActivityType>>();

            captureResult.For(activity, action);
            captureResult.SetWorkflow(this);

            return parentWorkflow;
        }

        protected override void Invoke(IContract contract)
        {
            try
            {
                activity.Process(contract);
            }
            catch (Exception ex)
            {
                if (!exceptionCases.ContainsKey(ex.GetType())) throw;

                ((ExceptionActivity) exceptionCases[ex.GetType()]).Process(contract);
            }
        }
    }
}