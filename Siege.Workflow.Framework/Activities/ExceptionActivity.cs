using System;
using Siege.ServiceLocation;

namespace Siege.Workflow.Framework.Activities
{
    public class ExceptionActivity : AbstractWorkflowActivity
    {
        public ExceptionActivity(IContextualServiceLocator serviceLocator)
            : base(serviceLocator)
        {
        }

        public ExceptionActivity Do<TActivity>()
            where TActivity : IWorkflowActivity
        {
            TActivity activity = serviceLocator.GetInstance<TActivity>();

            activity.SetWorkflow(this);

            return this;
        }

        public Workflow Do(Workflow subWorkflow)
        {
            subWorkflow.SetWorkflow(this);
            return parentWorkflow;
        }

        public Workflow Do(Action action)
        {
            Create(sub => sub.First(action).SetWorkflow(this));

            return parentWorkflow;
        }

        public Workflow Break(Workflow subWorkflow)
        {
            Do(subWorkflow);

            BreakActivity activity = serviceLocator.GetInstance<BreakActivity>();
            activity.SetWorkflow(this);

            return parentWorkflow;
        }

        public Workflow Break(Action action)
        {
            Do(action);

            BreakActivity activity = serviceLocator.GetInstance<BreakActivity>();
            activity.SetWorkflow(this);

            return parentWorkflow;
        }

        public override Workflow SetWorkflow(Workflow workflow)
        {
            parentWorkflow = workflow;
            rootWorkflow = workflow.RootWorkflow ?? workflow;

            return workflow;
        }

        public override WorkflowActivity<TActivityType> Then<TActivityType>()
        {
            return parentWorkflow.Then<TActivityType>();
        }

        protected override void Invoke(IContract contract)
        {
            foreach (Workflow activity in sequence)
            {
                activity.Process(contract);
            }
        }
    }
}