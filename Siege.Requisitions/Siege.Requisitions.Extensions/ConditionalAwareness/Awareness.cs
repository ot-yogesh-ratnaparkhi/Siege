﻿/*   Copyright 2009 - 2010 Marcus Bratton

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
using Siege.Requisitions.Registrations.Containers;

namespace Siege.Requisitions.Extensions.ConditionalAwareness
{
    public class Awareness
    {
        public static Action<IServiceLocator> Of<T>(Func<T> func)
        {
            var registration = new ContextualRegistration<T>(func);

            return serviceLocator =>
            {
                serviceLocator.Store.AddStore<IAwarenessStore>(new AwarenessStore());
                serviceLocator.Register(Given<IRegistrationContainer>
                                  .When<IRegistration>(r => r.GetRegistrationTemplate().GetType() == typeof (ContextualRegistrationTemplate))
                                  .Then(new CompositeRegistrationList()));

                serviceLocator.Register(registration);
            };
        }
    }
}