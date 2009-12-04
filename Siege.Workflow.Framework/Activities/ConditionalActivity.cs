using System;
using Siege.ServiceLocation;

namespace Siege.Workflow.Framework.Activities
{
    public class ConditionalActivity : AbstractWorkflowActivity
    {
        private Func<bool> evaluation;
        private Workflow activeWorkflow;
        private Workflow successWorkflow;
        private Workflow failWorkflow;

        public ConditionalActivity(IContextualServiceLocator serviceLocator)
            : base(serviceLocator)
        {
        }

        public ConditionalActivity ForCondition(Func<bool> evaluation)
        {
            this.evaluation = evaluation;

            successWorkflow = Create(sub => sub);
            failWorkflow = Create(sub => sub);

            successWorkflow.SetWorkflow(this);
            failWorkflow.SetWorkflow(this);
            successWorkflow.SetRootWorkflow(this);
            failWorkflow.SetRootWorkflow(this);

            activeWorkflow = successWorkflow;

            return this;
        }

        public ConditionalActivity Then(Workflow subWorkflow)
        {
            subWorkflow.SetWorkflow(this.activeWorkflow);

            return this;
        }

        public new ConditionalActivity Then(Action action)
        {
            Workflow subWorkflow = Create(sub => sub.First(action));
            subWorkflow.SetWorkflow(this.activeWorkflow);

            return this;
        }

        public ConditionalActivity Else
        {
            get
            {
                activeWorkflow = failWorkflow;

                return this;
            }
        }

        public Workflow EndIf
        {
            get { return this.parentWorkflow; }
        }

        protected override void Invoke(IContract contract)
        {
            if (evaluation.Invoke())
            {
                this.successWorkflow.Process(contract);
            }
            else
            {
                this.failWorkflow.Process(contract);
            }
        }

        public override void Process(IContract contract)
        {
            Invoke(contract);
        }
    }
}