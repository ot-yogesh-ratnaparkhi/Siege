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
using Siege.Requisitions.InternalStorage;
using Siege.Requisitions.RegistrationPolicies;
using Siege.Requisitions.Registrations;
using Siege.Requisitions.Registrations.Containers;
using Siege.Requisitions.RegistrationSyntax;
using Siege.Requisitions.RegistrationTemplates.Meta;
using Siege.Requisitions.Resolution;
using Siege.Requisitions.ExtensionMethods;

namespace Siege.Requisitions
{
    public abstract class ServiceLocator : IServiceLocator
    {
        private readonly IServiceLocatorAdapter serviceLocator;
        private readonly IServiceLocatorStore store;
        private readonly IResolutionTemplate resolutionTemplate;
        private readonly IMetaRegistrationTemplate registrationTemplate;
        private readonly Foundation foundation;

        protected ServiceLocator(IServiceLocatorAdapter serviceLocator, IServiceLocatorStore store)
        {
            this.serviceLocator = serviceLocator;
            this.store = store;
            foundation = new Foundation();
            resolutionTemplate = new DefaultResolutionTemplate(serviceLocator, this.store, foundation);
            registrationTemplate = new DefaultMetaRegistrationTemplate(this);

            serviceLocator.RegisterInstance(typeof (IServiceLocatorAdapter), serviceLocator);

            RegisterPolicy(Given<Transient>.Then<Transient>());
            RegisterPolicy(Given<Singleton>.Then<Singleton>());

            Register<Singleton>(Given<IServiceLocator>.Then(this));
            Register<Singleton>(Given<Microsoft.Practices.ServiceLocation.IServiceLocator>.Then(this));
            Register<Singleton>(Given<Foundation>.Then(this.foundation)); 
            Register<Singleton>(Given<IServiceLocatorStore>.Then(this.store));
            Register<Singleton>(Given<IContextStore>.Then(this.store.Get<IContextStore>()));
            Register<Singleton>(Given<IExecutionStore>.Then(this.store.Get<IExecutionStore>()));
            Register<Singleton>(Given<IResolutionStore>.Then(this.store.Get<IResolutionStore>()));
        }

        public object GetInstance(Type type, params IResolutionArgument[] arguments)
        {
            store.Get<IResolutionStore>().Add(new List<IResolutionArgument>(arguments));

            object instance = !HasTypeRegistered(typeof(IResolutionTemplate)) && type != typeof(IResolutionTemplate)
                ? resolutionTemplate.Resolve(type)
                : GetInstance<IResolutionTemplate>().Resolve(type);

            return instance;
        }

        public object GetInstance(Type type, string key, params IResolutionArgument[] arguments)
        {
            store.Get<IResolutionStore>().Add(new List<IResolutionArgument>(arguments));

            return serviceLocator.GetInstance(type, key, store.Get<IResolutionStore>().Items.OfType<ConstructorParameter, IResolutionArgument>());
        }

        public IServiceLocator Register<TRegistrationPolicy>(IRegistration registration) where TRegistrationPolicy : IRegistrationPolicy
        {
            if (foundation.IsRegistered(registration)) return this;

            IRegistrationContainer registrationContainer = foundation.ContainsRegistrationContainerForTemplate(registration.GetRegistrationTemplate())
                                              ? foundation.GetRegistrationContainer(registration.GetRegistrationTemplate())
                                              : GetInstance<IRegistrationContainer>(new ContextArgument(registration));

            var processedRegistration = HasTypeRegistered(typeof(IMetaRegistrationTemplate))
                                           ? GetInstance<IMetaRegistrationTemplate>(new ContextArgument(registration)).Process(registration)
                                           : registrationTemplate.Process(registration);

            var policy = GetInstance<TRegistrationPolicy>();
            policy.Handle(processedRegistration);

            registrationContainer.Add(policy);

            var factoryResolutionTemplate = HasTypeRegistered(typeof(IResolutionTemplate))
                                           ? GetInstance<IResolutionTemplate>(new ContextArgument(registration))
                                           : new FactoryResolutionTemplate(this, this.store, this.foundation);

            policy.GetRegistrationTemplate().Register(serviceLocator, this.store, processedRegistration, factoryResolutionTemplate);

            return this;
        }

        private void RegisterPolicy(IRegistration registration)
        {
            if (foundation.IsRegistered(registration)) return;

            IRegistrationContainer registrationContainer = foundation.ContainsRegistrationContainerForTemplate(registration.GetRegistrationTemplate())
                                              ? foundation.GetRegistrationContainer(registration.GetRegistrationTemplate())
                                              : GetInstance<IRegistrationContainer>(new ContextArgument(registration));

            registrationContainer.Add(registration);

            var factoryResolutionTemplate = HasTypeRegistered(typeof(IResolutionTemplate))
                                         ? GetInstance<IResolutionTemplate>()
                                         : new FactoryResolutionTemplate(this, this.store, this.foundation);

            registration.GetRegistrationTemplate().Register(serviceLocator, this.store, registration, factoryResolutionTemplate);
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
            return serviceLocator.GetAllInstances(serviceType);
        }

        public IEnumerable<TService> GetAllInstances<TService>()
        {
            return serviceLocator.GetAllInstances<TService>();
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