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
using System.IO;
using System.Linq;
using System.Reflection;
using Siege.ServiceLocator.ExtensionMethods;
using Siege.ServiceLocator.InternalStorage;
using Siege.ServiceLocator.RegistrationPolicies;
using Siege.ServiceLocator.Registrations;
using Siege.ServiceLocator.Registrations.AutoLoader;
using Siege.ServiceLocator.Registrations.Stores;
using Siege.ServiceLocator.RegistrationTemplates.Meta;
using Siege.ServiceLocator.Resolution;
using Siege.ServiceLocator.Resolution.Pipeline;

namespace Siege.ServiceLocator
{
    public abstract class ServiceLocator : IServiceLocator
    {
        private readonly IServiceLocatorAdapter serviceLocator;
        private readonly IServiceLocatorStore store;
        private readonly ResolutionPipeline pipeline;
        private readonly ResolutionPipeline factoryPipeline;
        private readonly PostResolutionPipeline postResolutionPipeline;
        private readonly IMetaRegistrationTemplate registrationTemplate;
        private readonly Foundation foundation;

        protected ServiceLocator(IServiceLocatorAdapter serviceLocator, IServiceLocatorStore store)
        {
            this.serviceLocator = serviceLocator;
            this.store = store;
            foundation = new Foundation();
            pipeline = new DefaultResolutionPipeline(foundation, serviceLocator, this.store);
            factoryPipeline = new FactoryResolutionPipeline(foundation, serviceLocator, this.store);
            postResolutionPipeline = new PostResolutionPipeline(foundation, serviceLocator, store);

            registrationTemplate = new DefaultMetaRegistrationTemplate(serviceLocator);

            serviceLocator.Register(typeof(Transient), typeof(Transient));
            serviceLocator.Register(typeof(Singleton), typeof(Singleton));
            serviceLocator.RegisterInstance(typeof(IServiceLocatorAdapter), serviceLocator); 
            serviceLocator.RegisterInstance(typeof(IServiceLocator), this);
            serviceLocator.RegisterInstance(typeof(Microsoft.Practices.ServiceLocation.IServiceLocator), this);
            serviceLocator.RegisterInstance(typeof(Foundation), this.foundation);
            serviceLocator.RegisterInstance(typeof(IServiceLocatorStore), this.store);
            serviceLocator.RegisterInstance(typeof(IContextStore), this.store.Get<IContextStore>());
            serviceLocator.RegisterInstance(typeof(IExecutionStore), this.store.Get<IExecutionStore>());
            serviceLocator.RegisterInstance(typeof(IResolutionStore), this.store.Get<IResolutionStore>());

            Register(Load.FromAssembliesIn(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\Plugins\", ".plugin"));
        }

        public object GetInstance(Type type, params IResolutionArgument[] arguments)
        {
            store.Get<IResolutionStore>().Add(new List<IResolutionArgument>(arguments));

            return pipeline.Execute(type);
        }

        public object GetInstance(Type type, string key, params IResolutionArgument[] arguments)
        {
            store.Get<IResolutionStore>().Add(new List<IResolutionArgument>(arguments));

            var registrationStore = foundation.StoreFor<NamedRegistrationStore>();
            
            if (registrationStore.HasRegistrationsForTypeNamed(type, key))
            {
                var registration = registrationStore.GetRegistrationForTypeAndName(type, key);
                return registration.ResolveWith(serviceLocator, store, postResolutionPipeline);
            }
            
            var stores = store.All<IResolutionStore>().ToList();
            var items = new List<IResolutionArgument>();
            stores.ForEach(x => items.AddRange(x.Items));
                
            return serviceLocator.GetInstance(type, key, items.OfType<ConstructorParameter, IResolutionArgument>());
        }

        public IServiceLocator Register<TRegistrationPolicy>(IRegistration registration) where TRegistrationPolicy : IRegistrationPolicy
        {
            var template = registration.GetRegistrationTemplate();
            var registrationStore = foundation.HasStoreFor(registration)
                                              ? foundation.StoreFor(registration)
                                              : GetInstance<IRegistrationStore>(registration.GetRegistrationStore().GetType());

            var processedRegistration = HasTypeRegistered(typeof(IMetaRegistrationTemplate))
                                           ? GetInstance<IMetaRegistrationTemplate>(new ContextArgument(registration)).Process(registration)
                                           : registrationTemplate.Process(registration);

            var policy = GetInstance<TRegistrationPolicy>();
            policy.Handle(processedRegistration);

            registrationStore.Add(policy);

            template.Register(serviceLocator, this.store, processedRegistration, factoryPipeline);

            return this;
        }

        public void AddContext(object contextItem)
        {
            store.Get<IContextStore>().Add(contextItem);
        }

        public TService GetInstance<TService>()
        {
            return GetInstance<TService>(typeof (TService), new IResolutionArgument[] {});
        }

        public TService GetInstance<TService>(params IResolutionArgument[] arguments)
        {
            return GetInstance<TService>(typeof (TService), arguments);
        }

        public TService GetInstance<TService>(Type type, params IResolutionArgument[] arguments)
        {
            return (TService) GetInstance(type, arguments);
        }

        public object GetInstance(Type type)
        {
            return GetInstance(type, new IResolutionArgument[] {});
        }

        public TService GetInstance<TService>(string key)
        {
            return (TService) GetInstance(typeof (TService), key, new IResolutionArgument[] {});
        }

        public TService GetInstance<TService>(string key, params IResolutionArgument[] arguments)
        {
            return (TService) GetInstance(typeof (TService), key, arguments);
        }

        public object GetService(Type serviceType)
        {
            return GetInstance(serviceType, new IResolutionArgument[] {});
        }

        public object GetInstance(Type type, string key)
        {
            return GetInstance(type, key, new IResolutionArgument[] {});
        }

        public IEnumerable<object> GetAllInstances(Type serviceType)
        {
            var list = new List<IRegistration>();

            var registrations = foundation.StoreFor<DefaultRegistrationStore>().GetRegistrationsForType(serviceType);
            if (registrations != null)
            {
                list.AddRange(registrations);
            }
            registrations = foundation.StoreFor<ConditionalRegistrationStore>().GetRegistrationsForType(serviceType);
            if (registrations != null)
            {
                list.AddRange(registrations);
            }

            return list.Select(item => item.ResolveWith(serviceLocator, store, postResolutionPipeline)).ToList();
        }

        public IEnumerable<TService> GetAllInstances<TService>()
        {
            return GetAllInstances(typeof (TService)).Cast<TService>();
        }

        public bool HasTypeRegistered(Type type)
        {
            return serviceLocator.HasTypeRegistered(type);
        }

        public IServiceLocator Register(Action<IServiceLocator> serviceLocator)
        {
            serviceLocator(this);

            return this;
        }

        public IServiceLocator Register(List<IRegistration> registrations)
        {
            return Register<Transient>(registrations);
        }

        public IServiceLocator Register<TRegistrationPolicy>(List<IRegistration> registrations)
            where TRegistrationPolicy : IRegistrationPolicy
        {
            foreach (IRegistration registration in registrations) Register<TRegistrationPolicy>(registration);

            return this;
        }

        public IServiceLocator Register(IRegistration registration)
        {
            return Register<Transient>(registration);
        }

        public IServiceLocatorStore Store
        {
            get { return store; }
        }

        public void Dispose()
        {
            serviceLocator.Dispose();
            store.Dispose();
        }
    }
}