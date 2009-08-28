using System;
using System.Collections;
using System.Collections.Generic;
using Ninject;
using Ninject.Parameters;
using Ninject.Planning.Bindings;
using Siege.ServiceLocation;

namespace Siege.Container.NinjectAdapter
{
    public class NinjectAdapter : IServiceLocator
    {
        private readonly IKernel kernel;

        public NinjectAdapter(IKernel kernel)
        {
            this.kernel = kernel;
        }

        public T GetInstance<T>()
        {
            return this.kernel.Get<T>();
        }

        public T GetInstance<T>(IDictionary constructorArguments)
        {
            return GetInstance<T>(typeof (T), constructorArguments);
        }

        public T GetInstance<T>(Type type)
        {
            return (T)this.kernel.Get(type);
        }

        public T GetInstance<T>(Type type, IDictionary constructorArguments)
        {
            if (constructorArguments == null || constructorArguments.Count == 0) return (T)kernel.Get(type);
            
            List<ConstructorArgument> args = new List<ConstructorArgument>();
            
            foreach (string key in constructorArguments.Keys)
            {
                ConstructorArgument argument = new ConstructorArgument(key, constructorArguments[key]);
                args.Add(argument);
            }

            return this.kernel.Get<T>(args.ToArray());
        }

        public void Register<T>(IUseCase<T> useCase)
        {
            BindingBuilder<T> builder = new BindingBuilder<T>(new Binding(typeof(T)));

            if (useCase is GenericUseCase<T>)
            {
                GenericUseCase<T> genericCase = useCase as GenericUseCase<T>;

                genericCase.Bind(this.kernel, builder);
            }

            if (useCase is ImplementationUseCase<T>)
            {
                var implementation = useCase as ImplementationUseCase<T>;

                implementation.Bind(this.kernel, builder);
            }
        }
    }

    public static class GenericUseCaseExtensions
    {
        public static void Bind<TBaseType>(this GenericUseCase<TBaseType> useCase, IKernel kernel, BindingBuilder<TBaseType> builder)
        {
            Type type = useCase.GetBinding();

            builder.To(type).InTransientScope();
            kernel.AddBinding(builder.Binding);
        }
    }

    public static class ImplementationUseCaseExtensions
    {
        public static void Bind<TBaseType>(this ImplementationUseCase<TBaseType> useCase, IKernel kernel, BindingBuilder<TBaseType> builder)
        {
            builder.ToConstant(useCase.GetBinding()).InTransientScope();
            kernel.AddBinding(builder.Binding);
        }
    }
}