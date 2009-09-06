using System;
using System.Collections.Generic;
using Siege.ServiceLocation;

namespace Siege.Workflow.Framework.Activities
{
    public class ExceptionActivity : AbstractWorkflowActivity
    {
        public ExceptionActivity(IContextualServiceLocator serviceLocator, IContract contract)
            : base(serviceLocator, contract)
        {
        }

        public ExceptionActivity Do<TActivity>()
            where TActivity : IWorkflowActivity
        {
            TActivity activity = serviceLocator.GetInstance<TActivity>(new { contract });

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

            BreakActivity activity = serviceLocator.GetInstance<BreakActivity>(new { contract });
            activity.SetWorkflow(this);

            return this.parentWorkflow;
        }

        public Workflow Break(Action action)
        {
            Do(action);

            BreakActivity activity = serviceLocator.GetInstance<BreakActivity>(new { contract });
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