using System;
using System.Collections;
using System.Collections.Generic;
using Siege.ServiceLocation;
using Siege.Workflow.Framework.Activities;

namespace Siege.Workflow.Framework
{
    public class Workflow
    {
        protected IContextualServiceLocator serviceLocator;
        protected readonly IContract contract;
        protected readonly IList<Workflow> sequence = new List<Workflow>();
        protected readonly Hashtable exceptionCases = new Hashtable();
        protected Workflow rootWorkflow;
        protected Workflow parentWorkflow;
        protected bool shouldBreak;

        public Workflow RootWorkflow
        {
            get { return this.rootWorkflow; }
        }

        protected Workflow(IContextualServiceLocator serviceLocator, IContract contract)
        {
            this.serviceLocator = serviceLocator;
            this.contract = contract;
        }

        public Workflow Create(Func<Workflow, Workflow> onCreate)
        {
            Workflow newWorkflow = new Workflow(serviceLocator, contract);

            onCreate(newWorkflow);

            return newWorkflow;
        }

        public WorkflowActivity First(Action action)
        {
            IDictionary dictionary = new Dictionary<string, IContract>();
            dictionary.Add("contract", contract);

            WorkflowActivity activity = serviceLocator.GetInstance<WorkflowActivity, IContract>(contract, new Dictionary<string, IContract> { { "contract", contract } });

            activity.For(action);
            this.rootWorkflow = this;
            activity.SetWorkflow(this);

            return activity;
        }

        public virtual WorkflowActivity<TActivity> First<TActivity>()
            where TActivity : IWorkflowActivity
        {
            WorkflowActivity<TActivity> activity = serviceLocator.GetInstance<WorkflowActivity<TActivity>, IContract>(contract, new Dictionary<string, IContract> { { "contract", contract } });

            this.rootWorkflow = this;
            activity.SetWorkflow(this);

            return activity;
        }

        public virtual WorkflowActivity<TActivityType> Then<TActivityType>()
            where TActivityType : IWorkflowActivity
        {
            WorkflowActivity<TActivityType> workflowActivity = serviceLocator.GetInstance<WorkflowActivity<TActivityType>, IContract>(contract, new Dictionary<string, IContract> { { "contract", contract } });

            workflowActivity.SetWorkflow(this.rootWorkflow);

            return workflowActivity;
        }

        public WorkflowActivity Then(Action action)
        {
            WorkflowActivity workflowActivity = serviceLocator.GetInstance<WorkflowActivity, IContract>(contract, new Dictionary<string, IContract> { { "contract", contract } });

            workflowActivity.SetWorkflow(this.rootWorkflow);
            workflowActivity.For(action);

            return workflowActivity;
        }

        public ExceptionActivity OnException<TExceptionType>()
            where TExceptionType : Exception
        {
            ExceptionActivity activity = serviceLocator.GetInstance<ExceptionActivity, IContract>(contract, new Dictionary<string, IContract> { { "contract", contract } });

            exceptionCases.Add(typeof (TExceptionType), activity);
            activity.SetWorkflow(this);

            return activity;
        }

        public ConditionalActivity If(Func<bool> evaluation)
        {
            ConditionalActivity activity = serviceLocator.GetInstance<ConditionalActivity, IContract>(contract, new Dictionary<string, IContract> { { "contract", contract } });

            activity.SetWorkflow(this);
            activity.ForCondition(evaluation);

            return activity;
        }

        public virtual Workflow SetWorkflow(Workflow workflow)
        {
            this.parentWorkflow = workflow;
            this.rootWorkflow = workflow.RootWorkflow ?? workflow;
            this.parentWorkflow.Add(this);

            return workflow;
        }

        public Workflow SetRootWorkflow(Workflow workflow)
        {
            this.rootWorkflow = workflow;

            return workflow;
        }

        public void Add(Workflow workflow)
        {
            this.sequence.Add(workflow);
        }

        protected void Break()
        {
            shouldBreak = true;
            if (this.parentWorkflow != null) this.parentWorkflow.Break();
        }

        public virtual void Process()
        {
            foreach (Workflow workflow in this.sequence)
            {
                if (shouldBreak) break;
                workflow.Process();
            }
        }
    }
}