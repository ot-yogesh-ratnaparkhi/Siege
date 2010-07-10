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
using Siege.ServiceLocation.Extensions.Decorator;
using Siege.ServiceLocation.Extensions.FactorySupport;
using Siege.ServiceLocation.Extensions.Initialization;
using Siege.ServiceLocation.Extensions.ConditionalInjection;
using Siege.ServiceLocation.Registrations;
using Siege.ServiceLocation.Registrations.OpenGenerics;

namespace Siege.ServiceLocation.Extensions.ExtendedRegistrationSyntax
{
    public class Given
    {
        public static IOpenGenericRegistration OpenType(Type type)
        {
            return new OpenGenericRegistration(type);
        }
    }

    public class Given<TService> : RegistrationSyntax.Given<TService>
    {
        public static IRegistration ConstructWith(Func<IInstanceResolver, TService> factoryMethod)
        {
            var registration = new DefaultFactoryRegistration<TService>();

            registration.MapsTo<Func<IInstanceResolver, TService>>();
            registration.ConstructWith(factoryMethod);

            return registration;
        }

        public static InjectionRule<TService> WhenInjectingInto<TResolvedType>()
        {
            var registration = new InjectionRule<TService>();

            registration.BasedOn<TResolvedType>();

            return registration;
        }

        public new static ConditionalActivationRule<TService, TContext> When<TContext>(Func<TContext, bool> evaluation)
        {
            var rule = new ConditionalActivationRule<TService, TContext>();

            rule.SetEvaluation(evaluation);

            return rule;
        }

        public static IInitializationregistration<TService> InitializeWith(Action<TService> action)
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
    }
}