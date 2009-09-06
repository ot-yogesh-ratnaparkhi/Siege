using System;
using System.Collections;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Siege.ServiceLocation;

namespace Siege.Container.WindsorAdapter
{
    public class WindsorAdapter : IServiceLocatorAdapter
    {
        private IKernel kernel;
        private IContextualServiceLocator locator;

        public WindsorAdapter(IKernel kernel)
        {
            this.kernel = kernel;
        }

        public T GetInstance<T>()
        {
            return kernel.Resolve<T>();
        }

        public T GetInstance<T>(IDictionary constructorArguments)
        {
            return GetInstance<T>(typeof (T), constructorArguments);
        }

        public T GetInstance<T>(object anonymousConstructorArguments)
        {
            return GetInstance<T>(anonymousConstructorArguments.AnonymousTypeToDictionary());
        }

        public T GetInstance<T>(Type type)
        {
            return (T)kernel.Resolve(type);
        }

        public T GetInstance<T>(Type type, IDictionary constructorArguments)
        {
            if (constructorArguments == null) return (T) kernel.Resolve(type);
            return (T)kernel.Resolve(type, constructorArguments);
        }

        public T GetInstance<T>(string key)
        {
            return GetInstance<T>(key, null);
        }

        public T GetInstance<T>(string key, IDictionary constructorArguments)
        {
            if (constructorArguments == null) return kernel.Resolve<T>(key);

            return (T)kernel.Resolve(key, constructorArguments);
        }

        public IServiceLocator Register<T>(IUseCase<T> useCase)
        {
            if(useCase is IConditionalUseCase<T>)
            {
                var conditionalCase = useCase as IConditionalUseCase<T>;

                var factory = locator.GetConditionalFactory<T>();
                factory.AddCase(conditionalCase);

                conditionalCase.Bind(this.kernel, this.locator);
            }
            else if (useCase is KeyBasedImplementationUseCase<T>)
            {
                var keyCase = useCase as KeyBasedImplementationUseCase<T>;

                keyCase.Bind(this.kernel);
            }
            else if (useCase is KeyBasedUseCase<T>)
            {
                var keyCase = useCase as KeyBasedUseCase<T>;

                keyCase.Bind(this.kernel);
            }
            else if (useCase is GenericUseCase<T>)
            {
                GenericUseCase<T> genericCase = useCase as GenericUseCase<T>;

                genericCase.Bind(kernel);
            }
            else if (useCase is ImplementationUseCase<T>)
            {
                var implementation = useCase as ImplementationUseCase<T>;

                implementation.Bind(kernel);
            }

            return this;
        }

        public void RegisterParentLocator(IContextualServiceLocator locator)
        {
            this.locator = locator;
            kernel.Register(Component.For<IServiceLocator, IContextualServiceLocator>().Instance(locator));
        }
    }
}
