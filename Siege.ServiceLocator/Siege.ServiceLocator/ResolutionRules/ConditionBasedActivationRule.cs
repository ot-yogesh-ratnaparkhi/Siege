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

using System.Collections.Generic;
using Siege.ServiceLocator.Registrations;
using Siege.ServiceLocator.Registrations.Conditional;
using Siege.ServiceLocator.RegistrationSyntax;

namespace Siege.ServiceLocator.ResolutionRules
{
    public class ConditionBasedActivationRule<TBaseService, TCondition> : IConditionalActivationRule where TCondition : ICondition
    {
        public IRuleEvaluationStrategy GetRuleEvaluationStrategy()
        {
            return new ContextEvaluationStrategy();
        }

        public bool Evaluate(IInstanceResolver resolver, object context)
        {
            return resolver.GetInstance<TCondition>().Evaluate(context);
        }

        public List<IRegistration> Then<TImplementingType>() where TImplementingType : TBaseService
        {
            var list = new List<IRegistration>();

            var registration = new ConditionalRegistration<TBaseService>();

            registration.SetActivationRule(this);
            registration.MapsTo<TImplementingType>();

            list.Add(registration);
            list.Add(Given<TCondition>.Then<TCondition>());

            return list;
        }

        public List<IRegistration> Then(TBaseService implementation)
        {
            var list = new List<IRegistration>();

            var registration = new ConditionalInstanceRegistration<TBaseService>();

            registration.SetActivationRule(this);
            registration.MapsTo(implementation);

            list.Add(registration);
            list.Add(Given<TCondition>.Then<TCondition>());

            return list;
        }
    }
}