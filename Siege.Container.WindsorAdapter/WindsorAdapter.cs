﻿using System;
using System.Collections;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Siege.ServiceLocation;

namespace Siege.Container.WindsorAdapter
{
    public class WindsorAdapter : IServiceLocator
    {
        private readonly IKernel kernel;

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

        public T GetInstance<T>(Type type)
        {
            return (T)kernel.Resolve(type);
        }

        public T GetInstance<T>(Type type, IDictionary constructorArguments)
        {
            if (constructorArguments == null) return (T) kernel.Resolve(type);
            return (T)kernel.Resolve(type, constructorArguments);
        }

        public void Register<T>(IUseCase<T> useCase)
        {
            if (useCase is GenericUseCase<T>)
            {
                GenericUseCase<T> genericCase = useCase as GenericUseCase<T>;

                genericCase.Bind(kernel);
            }

            if (useCase is ImplementationUseCase<T>)
            {
                var implementation = useCase as ImplementationUseCase<T>;

                implementation.Bind(kernel);
            }
        }
    }

    public static class GenericUseCaseExtensions
    {
        public static void Bind<TBaseType>(this GenericUseCase<TBaseType> useCase, IKernel kernel)
        {
            kernel.Register(Component.For(useCase.GetBinding()).Unless(Component.ServiceAlreadyRegistered).LifeStyle.Transient);
        }
    }

    public static class ImplementationUseCaseExtensions
    {
        public static void Bind<TBaseType>(this ImplementationUseCase<TBaseType> useCase, IKernel kernel)
        {
            kernel.Register(Component.For<TBaseType>().Instance(useCase.GetBinding()));
        }
    }
}
