using System;
using Siege.ServiceLocation;

namespace Siege.Workflow.Framework.Activities
{
    public class WorkflowActivity : WorkflowActivity<ActionActivity>
    {
        public WorkflowActivity(IContextualServiceLocator serviceLocator, IContract request)
            : base(serviceLocator, request)
        {
        }

        public Workflow For(Action action)
        {
            activity = serviceLocator.GetInstance<ActionActivity, Context<IContract>>(With.Context(request));

            activity.SetWorkflow(this);
            activity.With(action);

            return this.parentWorkflow;
        }
    }

    public class WorkflowActivity<TActivityType> : AbstractWorkflowActivity
        where TActivityType : IWorkflowActivity
    {
        protected TActivityType activity;

        public WorkflowActivity(IContextualServiceLocator locator, IContract request)
            : base(locator, request)
        {
            activity = locator.GetInstance<TActivityType, Context<IContract>>(With.Context(request));
        }

        public Workflow Then()
        {
            this.parentWorkflow.Then<TActivityType>();

            return this.parentWorkflow;
        }

        public Workflow CaptureResult(Action<TActivityType> action)
        {
            CaptureResultActivity<TActivityType> captureResult = serviceLocator.GetInstance<CaptureResultActivity<TActivityType>, Context<IContract>>(With.Context(request));

            captureResult.For(activity, action);
            captureResult.SetWorkflow(this);

            return this.parentWorkflow;
        }

        protected override void Invoke(IContract contract)
        {
            try
            {
                activity.Process();
            }
            catch (Exception ex)
            {
                if (!exceptionCases.ContainsKey(ex.GetType())) throw;

                ((ExceptionActivity) exceptionCases[ex.GetType()]).Process();
            }
        }
    }
}