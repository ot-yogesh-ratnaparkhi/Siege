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
using Siege.ServiceLocation.Extensions.ExtendedSyntax;
using Siege.ServiceLocation.Rules;
using Siege.ServiceLocation.UseCases;

namespace Siege.ServiceLocation.Extensions.ResolutionContextSupport
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
            return context == basedOnType;
        }

        public new IInjectionUseCase<TService> Then<TImplementingType>() where TImplementingType : TService
        {
            var useCase = new InjectionUseCase<TService>();

            useCase.SetActivationRule(this);
            useCase.BindTo<TImplementingType>();

            return useCase;
        }

        public new IUseCase Then(TService implementation)
        {
            var useCase = new InjectionInstanceUseCase<TService>();

            useCase.SetActivationRule(this);
            useCase.BindTo(implementation);

            return useCase;
        }
    }
}