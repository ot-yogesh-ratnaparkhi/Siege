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
using System.Collections.Generic;
using Siege.ServiceLocator.RegistrationPolicies;
using Siege.ServiceLocator.Registrations;
using Siege.ServiceLocator.Registrations.ConditionalInjection;
using Siege.ServiceLocator.Registrations.Decorator;
using Siege.ServiceLocator.Registrations.Default;
using Siege.ServiceLocator.Registrations.FactorySupport;
using Siege.ServiceLocator.Registrations.Initialization;
using Siege.ServiceLocator.Registrations.InjectionOverrides;
using Siege.ServiceLocator.Registrations.MultiConditionalActivation;
using Siege.ServiceLocator.Registrations.Named;
using Siege.ServiceLocator.Registrations.OpenGenerics;
using Siege.ServiceLocator.Resolution;
using Siege.ServiceLocator.ResolutionRules;

namespace Siege.ServiceLocator.RegistrationSyntax
{
    public class Given
    {
        public static IOpenGenericRegistration OpenType(Type type)
        {
            return new OpenGenericRegistration(type);
        }
    }

    public class Given<TService>
    {
        public static ConditionalActivationRule<TService> When<TContext>(Func<TContext, bool> evaluation)
        {
            var rule = new ConditionalActivationRule<TService>();

            var lambdaEvaluation = new LambdaCondition<TContext>(evaluation);

            rule.SetEvaluation(lambdaEvaluation);

            return rule;
        }

        public static ConditionBasedActivationRule<TService, TCondition> When<TCondition>() where TCondition : ICondition
        {
            var rule = new ConditionBasedActivationRule<TService, TCondition>();

            return rule;
        }

        public static IRegistration Then<TImplementingType>() where TImplementingType : TService
        {
            var registration = new DefaultRegistration<TService>();

            registration.MapsTo<TImplementingType>();

            return registration;
        }

        public static INamedRegistration Then<TImplementingType>(string key) where TImplementingType : TService
        {
            var registration = new NamedRegistration<TService>(key);

            registration.MapsTo<TImplementingType>();

            return registration;
        }

        public static IRegistration Then(TService implementation)
        {
            var registration = new DefaultInstanceRegistration<TService>();

            registration.MapsTo(implementation);

            return registration;
        }

        public static IRegistration Then(string key, TService implementation)
        {
            var registration = new NamedInstanceRegistration<TService>(key);

            registration.MapsTo(implementation);

            return registration;
        }

        public static IRegistration ConstructWith(Func<IInstanceResolver, TService> factoryMethod)
        {
            var registration = new DefaultFactoryRegistration<TService>();

            registration.MapsTo<Func<IInstanceResolver, TService>>();
            registration.ConstructWith(factoryMethod);

            return registration;
        }

        public static Action<IServiceLocator> ConstructWith(List<IResolutionArgument> arguments)
        {
            var registration = new ConstructorRegistration { Arguments = arguments };

            return serviceLocator =>
            {
                serviceLocator.Store.AddStore<IInjectionOverrideStore>(new InjectionOverrideStore());

                if (!serviceLocator.HasTypeRegistered(typeof(InjectionOverrideRegistrationStore))) serviceLocator.Register<Singleton>(Given<InjectionOverrideRegistrationStore>.Then<InjectionOverrideRegistrationStore>());

                serviceLocator.Register(registration);
            };
        }

        public static InjectionRule<TService> WhenInjectingInto<TResolvedType>()
        {
            var registration = new InjectionRule<TService>();

            registration.BasedOn<TResolvedType>();

            return registration;
        }

        public static MultiConditionalActivationRule<TService> When(Action<MultiConditionalActivationRule<TService>> generator)
        {
            var rule = new MultiConditionalActivationRule<TService>();

            generator.Invoke(rule);

            return rule;

        }

        public static IInitializationRegistration<TService> InitializeWith(Action<TService> action)
        {
            var registration = new DefaultInitializationRegistration<TService>();

            registration.MapsTo<TService>();

            Func<TService, TService> func = service =>
            {
                action(service);
                return service;
            };

            registration.Associate(func);

            return registration;
        }

        public static IRegistration DecorateWith(Func<TService, TService> func)
        {
            var registration = new DefaultDecoratorRegistration<TService>();

            registration.MapsTo<TService>();

            registration.Associate(func);

            return registration;
        }

        public static List<IRegistration> Then(Func<Given<TService>, List<IRegistration>> given)
        {
            return given(new Given<TService>());
        }
    }
}