using System;
using Siege.ServiceLocation;

namespace Siege.Workflow.Framework.Activities
{
    public class ActionActivity : AbstractWorkflowActivity
    {
        private Action action;

        public ActionActivity(IContextualServiceLocator serviceLocator)
            : base(serviceLocator)
        {
        }

        protected override void Invoke(IContract contract)
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