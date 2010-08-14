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
using Siege.Requisitions.Extensions.ExtendedRegistrationSyntax;
using Siege.Requisitions.Registrations;
using Siege.Requisitions.ResolutionRules;

namespace Siege.Requisitions.Extensions.ConditionalInjection
{
    public class InjectionRule<TService> : ActivationRule<TService, object>
    {
        private Type basedOnType;

        public void BasedOn<TResolvedType>()
        {
            basedOnType = typeof(TResolvedType);
        }

        public override IRuleEvaluationStrategy GetRuleEvaluationStrategy()
        {
            return new InjectionRuleEvaluationStrategy();
        }

        public override bool Evaluate(object context)
        {
            return (Type)context == basedOnType;
        }

        public new IRegistration Then<TImplementingType>() where TImplementingType : TService
        {
            var registration = new InjectionRegistration<TService>();

            registration.SetActivationRule(this);
            registration.MapsTo<TImplementingType>();

            return registration;
        }

        public new IRegistration Then(TService implementation)
        {
            var registration = new InjectionInstanceRegistration<TService>();

            registration.SetActivationRule(this);
            registration.MapsTo(implementation);

            return registration;
        }
    }
}