using System;
using Siege.ServiceLocation;

namespace Siege.Workflow.Framework.Activities
{
    public class CaptureResultActivity<T> : AbstractWorkflowActivity
        where T : IWorkflowActivity
    {
        protected T parentActivity;
        protected Action<T> outcome;

        public CaptureResultActivity(IContextualServiceLocator serviceLocator)
            : base(serviceLocator)
        {
        }

        public void For(T activity, Action<T> action)
        {
            this.parentActivity = activity;
            this.outcome = action;
        }

        protected override void Invoke(IContract contract)
        {
            outcome(parentActivity);
        }
    }
}