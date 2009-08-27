using System;
using Siege.ServiceLocation;

namespace Siege.Workflow.Framework.Activities
{
    public class ExceptionActivity : AbstractWorkflowActivity
    {
        public ExceptionActivity(IContextualServiceLocator serviceLocator, IContract request)
            : base(serviceLocator, request)
        {
        }

        public ExceptionActivity Do<TActivity>()
            where TActivity : IWorkflowActivity
        {
            TActivity activity = serviceLocator.GetInstance<TActivity, Context<IContract>>(With.Context(request));

            activity.SetWorkflow(this);

            return this;
        }

        public Workflow Do(Workflow subWorkflow)
        {
            subWorkflow.SetWorkflow(this);
            return this.parentWorkflow;
        }

        public Workflow Do(Action action)
        {
            Create(sub => sub.First(action).SetWorkflow(this));

            return this.parentWorkflow;
        }

        public Workflow Break(Workflow subWorkflow)
        {
            Do(subWorkflow);

            BreakActivity activity = serviceLocator.GetInstance<BreakActivity, Context<IContract>>(With.Context(request));
            activity.SetWorkflow(this);

            return this.parentWorkflow;
        }

        public Workflow Break(Action action)
        {
            Do(action);

            BreakActivity activity = serviceLocator.GetInstance<BreakActivity, Context<IContract>>(With.Context(request));
            activity.SetWorkflow(this);

            return this.parentWorkflow;
        }

        public override Workflow SetWorkflow(Workflow workflow)
        {
            this.parentWorkflow = workflow;
            this.rootWorkflow = workflow.RootWorkflow ?? workflow;

            return workflow;
        }

        public override WorkflowActivity<TActivityType> Then<TActivityType>()
        {
            return this.parentWorkflow.Then<TActivityType>();
        }

        protected override void Invoke(IContract contract)
        {
            foreach (Workflow activity in this.sequence)
            {
                activity.Process();
            }
        }
    }
}