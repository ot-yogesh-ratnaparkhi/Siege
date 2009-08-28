using System;
using Siege.ServiceLocation;

namespace Siege.Workflow.Framework.Activities
{
    public class ActionActivity : AbstractWorkflowActivity
    {
        private Action action;

        public ActionActivity(IContextualServiceLocator serviceLocator, IContract contract)
            : base(serviceLocator, contract)
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