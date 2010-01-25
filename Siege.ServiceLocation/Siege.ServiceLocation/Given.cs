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
using Siege.ServiceLocation.Rules;
using Siege.ServiceLocation.UseCases;
using Siege.ServiceLocation.UseCases.Default;
using Siege.ServiceLocation.UseCases.Named;

namespace Siege.ServiceLocation
{
    public class Given<TBaseService>
    {
        public static ConditionalActivationRule<TBaseService, TContext> When<TContext>(Func<TContext, bool> evaluation)
        {
            return new ConditionalActivationRule<TBaseService, TContext>(evaluation);
        }

        public static IDefaultUseCase<TBaseService> Then<TImplementingType>() where TImplementingType : TBaseService
        {
            DefaultUseCase<TBaseService> useCase = new DefaultUseCase<TBaseService>();

            useCase.BindTo<TImplementingType>();

            return useCase;
        }

        public static IKeyBasedUseCase<TBaseService> Then<TImplementingType>(string key) where TImplementingType : TBaseService
        {
            KeyBasedUseCase<TBaseService> useCase = new KeyBasedUseCase<TBaseService>(key);

            useCase.BindTo<TImplementingType>();

            return useCase;
        }

        public static IInstanceUseCase Then(TBaseService implementation)
        {
            DefaultInstanceUseCase<TBaseService> useCase = new DefaultInstanceUseCase<TBaseService>();

            useCase.BindTo(implementation);

            return useCase;
        }

        public static IInstanceUseCase Then(string key, TBaseService implementation)
        {
            KeyBasedInstanceUseCase<TBaseService> useCase = new KeyBasedInstanceUseCase<TBaseService>(key);

            useCase.BindTo(implementation);

            return useCase;
        }
    }
}
