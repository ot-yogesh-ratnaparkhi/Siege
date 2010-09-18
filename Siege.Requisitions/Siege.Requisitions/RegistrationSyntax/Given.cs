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
using Siege.Requisitions.Registrations;
using Siege.Requisitions.Registrations.Default;
using Siege.Requisitions.Registrations.Named;
using Siege.Requisitions.ResolutionRules;

namespace Siege.Requisitions.RegistrationSyntax
{
    public class Given<TBaseService>
    {
        public static ConditionalActivationRule<TBaseService, TContext> When<TContext>(Func<TContext, bool> evaluation)
        {
            var rule = new ConditionalActivationRule<TBaseService, TContext>();

            rule.SetEvaluation(evaluation);

            return rule;
        }

        public static IRegistration Then<TImplementingType>() where TImplementingType : TBaseService
        {
            var registration = new DefaultRegistration<TBaseService>();

            registration.MapsTo<TImplementingType>();

            return registration;
        }

        public static INamedRegistration Then<TImplementingType>(string key) where TImplementingType : TBaseService
        {
            var registration = new NamedRegistration<TBaseService>(key);

            registration.MapsTo<TImplementingType>();

            return registration;
        }

        public static IRegistration Then(TBaseService implementation)
        {
            var registration = new DefaultInstanceRegistration<TBaseService>();

            registration.MapsTo(implementation);

            return registration;
        }

        public static IRegistration Then(string key, TBaseService implementation)
        {
            var registration = new NamedInstanceRegistration<TBaseService>(key);

            registration.MapsTo(implementation);

            return registration;
        }
    }
}