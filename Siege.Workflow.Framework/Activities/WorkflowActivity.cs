using System;
using System.Collections.Generic;
using Siege.ServiceLocation;

namespace Siege.Workflow.Framework.Activities
{
    public class WorkflowActivity : WorkflowActivity<ActionActivity>
    {
        public WorkflowActivity(IContextualServiceLocator serviceLocator, IContract contract)
            : base(serviceLocator, contract)
        {
        }

        public Workflow For(Action action)
        {
            activity = serviceLocator.GetInstance<ActionActivity, IContract>(contract, new Dictionary<string, IContract> { { "contract", contract } });

            activity.SetWorkflow(this);
            activity.With(action);

            return this.parentWorkflow;
        }
    }

    public class WorkflowActivity<TActivityType> : AbstractWorkflowActivity
        where TActivityType : IWorkflowActivity
    {
        protected TActivityType activity;

        public WorkflowActivity(IContextualServiceLocator locator, IContract contract)
            : base(locator, contract)
        {
            activity = locator.GetInstance<TActivityType, IContract>(contract, new Dictionary<string, IContract> { { "contract", contract } });
        }

        public Workflow Then()
        {
            this.parentWorkflow.Then<TActivityType>();

            return this.parentWorkflow;
        }

        public Workflow CaptureResult(Action<TActivityType> action)
        {
            CaptureResultActivity<TActivityType> captureResult = serviceLocator.GetInstance<CaptureResultActivity<TActivityType>, IContract>(contract, new Dictionary<string, IContract> { { "contract", contract } });

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