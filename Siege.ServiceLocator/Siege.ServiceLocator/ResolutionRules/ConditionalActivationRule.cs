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
using Siege.ServiceLocator.Registrations;
using Siege.ServiceLocator.Registrations.Conditional;
using Siege.ServiceLocator.Registrations.Decorator;
using Siege.ServiceLocator.Registrations.FactorySupport;
using Siege.ServiceLocator.Registrations.Initialization;

namespace Siege.ServiceLocator.ResolutionRules
{
    public class ConditionalActivationRule<TBaseService> : IConditionalActivationRule
    {
        private ICondition evaluation;

        public void SetEvaluation(ICondition evaluation)
        {
            this.evaluation = evaluation;
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

        public virtual bool Evaluate(IInstanceResolver resolver, object context)
        {
            return evaluation.Evaluate(context);
        }

        public virtual IRuleEvaluationStrategy GetRuleEvaluationStrategy()
        {
            return new ContextEvaluationStrategy();
        }

        public IRegistration ConstructWith<TService>(Func<IInstanceResolver, TService> factoryMethod)
        {
            var registration = new ConditionalFactoryRegistration<TService>();

            registration.MapsTo<TService>();
            registration.SetActivationRule(this);
            registration.ConstructWith(factoryMethod);

            return registration;
        }

        public IRegistration InitializeWith(Action<TBaseService> action)
        {
            var registration = new ConditionalInitializationRegistration<TBaseService>();

            registration.MapsTo<TBaseService>();
            registration.SetActivationRule(this);

            Func<TBaseService, TBaseService> func = service =>
            {
                action(service);
                return service;
            };

            registration.Associate(func);

            return registration;
        }

        public IRegistration DecorateWith(Func<TBaseService, TBaseService> func)
        {
            var registration = new ConditionalDecoratorRegistration<TBaseService>();

            registration.MapsTo<TBaseService>();
            registration.SetActivationRule(this);

            registration.Associate(func);

            return registration;
        }
    }
}