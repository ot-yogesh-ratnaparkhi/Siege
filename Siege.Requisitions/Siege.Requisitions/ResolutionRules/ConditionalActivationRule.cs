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
using Siege.Requisitions.Registrations;
using Siege.Requisitions.Registrations.Conditional;

namespace Siege.Requisitions.ResolutionRules
{
    public class ConditionalActivationRule<TBaseService, TContext> : IConditionalActivationRule
    {
        private Predicate<object> evaluation;

        public void SetEvaluation(Func<TContext, bool> evaluation)
        {
            this.evaluation = x => (x is TContext) ? evaluation.Invoke((TContext)x) : false;
        }

        public IRegistration Then<TImplementingType>() where TImplementingType : TBaseService
        {
            var registration = new ConditionalRegistration<TBaseService>();

            registration.SetActivationRule(this);
            registration.MapsTo<TImplementingType>();

            return registration;
        }

        public IRegistration Then(TBaseService implementation)
        {
            var registration = new ConditionalInstanceRegistration<TBaseService>();

            registration.SetActivationRule(this);
            registration.MapsTo(implementation);

            return registration;
        }

        public virtual bool Evaluate(object context)
        {
            return evaluation.Invoke(context);
        }

        public virtual IRuleEvaluationStrategy GetRuleEvaluationStrategy()
        {
            return new ContextEvaluationStrategy();
        }
    }
}