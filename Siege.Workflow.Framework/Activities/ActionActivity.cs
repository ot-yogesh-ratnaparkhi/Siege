using System;
using Siege.ServiceLocation;

namespace Siege.Workflow.Framework.Activities
{
    public class ActionActivity : AbstractWorkflowActivity
    {
        private Action action;

        public ActionActivity(IContextualServiceLocator serviceLocator, IContract request)
            : base(serviceLocator, request)
        {
        }

        protected override void Invoke(IContract request)
        {
            action.Invoke();
        }

        public ActionActivity With(Action action)
        {
            this.action = action;

            return this;
        }
    }
}