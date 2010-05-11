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
using Siege.ServiceLocation.Extensions.ResolutionContextSupport;
using Siege.ServiceLocation.UseCases.Actions;
using Siege.ServiceLocation.UseCases.Default;
using Siege.ServiceLocation.UseCases.OpenGenerics;

namespace Siege.ServiceLocation.Extensions.ExtendedSyntax
{
    public class Given
    {
        public static IOpenGenericUseCase OpenType(Type type)
        {
            return new OpenGenericUseCase(type);
        }
    }

    public class Given<TService> : Syntax.Given<TService>
    {
        public static IDefaultUseCase<TService> ConstructWith(Func<IInstanceResolver, TService> factoryMethod)
        {
            var useCase = new DefaultFactoryUseCase<TService>();

            useCase.BindTo<Func<IInstanceResolver, TService>>();
            useCase.ConstructWith(factoryMethod);

            return useCase;
        }

        public static InjectionRule<TService> WhenInjectingInto<TResolvedType>()
        {
            var useCase = new InjectionRule<TService>();

            useCase.BasedOn<TResolvedType>();

            return useCase;
        }

        public new static ConditionalActivationRule<TService, TContext> When<TContext>(Func<TContext, bool> evaluation)
        {
            var rule = new ConditionalActivationRule<TService, TContext>();

            rule.SetEvaluation(evaluation);

            return rule;
        }

        public static IInitializationUseCase<TService> InitializeWith(Action<TService> action)
        {
            InitializationUseCase<TService> useCase = new InitializationUseCase<TService>();

            useCase.BindTo<TService>();

            Func<TService, TService> func = service => 
            {
                action(service);
                return service; 
            };
            
            useCase.Associate(func);

            return useCase;
        }

        public IDefaultActionUseCase DecorateWith(Func<TService, TService> func)
        {
            var useCase = new DecoratorUseCase<TService>();

            useCase.BindTo<TService>();

            useCase.Associate(func);

            return useCase;
        }
    }
}