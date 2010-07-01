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
using Siege.ServiceLocation.Extensions.Hydration;
using Siege.ServiceLocation.UseCases;

namespace Siege.ServiceLocation.Extensions.ExtendedSyntax
{
    public abstract class ActivationRule<TBaseService, TContext> : Rules.ConditionalActivationRule<TBaseService, TContext>
    {
        public IUseCase ConstructWith<TService>(Func<IInstanceResolver, TService> factoryMethod)
        {
            var useCase = new ConditionalFactoryUseCase<TService>();

            useCase.BindTo<TService>();
            useCase.SetActivationRule(this);
            useCase.ConstructWith(factoryMethod);

            return useCase;
        }

        public IUseCase InitializeWith(Action<TBaseService> action)
        {
            var useCase = new ConditionalInitializationUseCase<TBaseService>();

            useCase.BindTo<TBaseService>();
            useCase.SetActivationRule(this);

            Func<TBaseService, TBaseService> func = service =>
            {
                action(service);
                return service;
            };

            useCase.Associate(func);

            return useCase;
        }

        public IUseCase DecorateWith(Func<TBaseService, TBaseService> func)
        {
            var useCase = new ConditionalDecoratorUseCase<TBaseService>();

            useCase.BindTo<TBaseService>();
            useCase.SetActivationRule(this);

            useCase.Associate(func);

            return useCase;
        }
    }
}
