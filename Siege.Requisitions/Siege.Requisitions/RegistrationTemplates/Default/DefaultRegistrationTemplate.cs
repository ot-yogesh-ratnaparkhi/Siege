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
using Siege.Requisitions.InternalStorage;
using Siege.Requisitions.Registrations;
using Siege.Requisitions.Registrations.Named;
using Siege.Requisitions.Resolution;
using Siege.Requisitions.Resolution.Pipeline;

namespace Siege.Requisitions.RegistrationTemplates.Default
{
    public class DefaultRegistrationTemplate : AbstractRegistrationTemplate
    {
        public override void Register(IServiceLocatorAdapter adapter, IServiceLocatorStore store, IRegistration registration, ResolutionPipeline pipeline)
        {
            var mappedFromType = registration.GetMappedFromType();
            var mappedToType = registration.GetMappedToType();  

            if (mappedFromType != mappedToType)
            {
                adapter.RegisterFactoryMethod(mappedFromType, () => pipeline.Execute(mappedFromType));
                RegisterLazy(adapter, mappedFromType, pipeline);
                adapter.Register(mappedToType, mappedToType);
				RegisterContextual(adapter, mappedToType);
            }
            else
            {
                var serviceLocator = adapter.GetInstance<IServiceLocator>();
                adapter.RegisterFactoryMethod(mappedToType, () =>
                {
                    var instance = serviceLocator.GetInstance(mappedToType, "default." + mappedToType, new IResolutionArgument[0]);
                    return instance;
                }); 
                serviceLocator.Register(new NamedRegistration(mappedToType, mappedToType, "default." + mappedToType));
            }
  
            RegisterLazy(adapter, mappedToType, pipeline);
			RegisterTypeResolver(adapter, mappedFromType);
			RegisterContextual(adapter, mappedFromType);
        }
    }
}