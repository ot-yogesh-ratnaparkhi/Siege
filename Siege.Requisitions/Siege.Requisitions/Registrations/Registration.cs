/*   Copyright 2009 - 2010 Marcus Bratton

     Licensed under the Apache License, Version 2.0 (the "License");
     you may not use this file except in compliance with the License.
     You may obtain a copy of the License at

     http://www.apache.org/licenses/LICENSE-2.0

     Unless required by applicable law or agreed to in writing, software
     distributed under the License is distributed on an "AS IS" BASIS,
     WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
     See the License for the specific language governing permissions and
     limitations under the License.
*/

using System;
using Siege.Requisitions.EventHandlers;
using Siege.Requisitions.InternalStorage;
using Siege.Requisitions.RegistrationTemplates;
using Siege.Requisitions.ResolutionRules;

namespace Siege.Requisitions.Registrations
{
    public abstract class Registration : IRegistration, ITypeRequester
    {
        protected abstract IActivationStrategy GetActivationStrategy();
        protected IActivationRule rule;
        public abstract IRegistrationTemplate GetRegistrationTemplate();
        public abstract Type GetMappedFromType();
        public abstract object GetMappedTo();
        public abstract Type GetMappedToType();
        public abstract void MapsTo(object target);
        public event TypeRequestedEventHandler TypeRequested;

        public void SetActivationRule(IActivationRule rule)
        {
            this.rule = rule;
        }

        public bool IsValid(IServiceLocatorStore context)
        {
            if(rule == null) return false;

            return rule.GetRuleEvaluationStrategy().IsValid(rule, context);
        }

        public virtual object ResolveWith(IInstanceResolver locator, IServiceLocatorStore context)
        {
            context.ExecutionStore.WireEvent(this);
            object instance = null;

            if (rule == null)
            {
                RaiseTypeRequestedEvent(GetMappedToType());
                instance = GetActivationStrategy().Resolve(locator, context);
            }
            else
            {
                if (rule.GetRuleEvaluationStrategy().IsValid(rule, context))
                {
                    RaiseTypeRequestedEvent(GetMappedToType());
                    instance = GetActivationStrategy().Resolve(locator, context);
                }
            }

            context.ExecutionStore.UnWireEvent(this);

            return instance;
        }

        private void RaiseTypeRequestedEvent(Type type)
        {
            if (this.TypeRequested != null) this.TypeRequested(type);
        }
    }
}