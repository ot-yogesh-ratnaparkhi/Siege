using System;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Siege.ServiceLocation;

namespace Siege.Container.WindsorAdapter
{
    public class WindsorAdapter : IContextualServiceLocator
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

        public T GetInstance<T>(Type type)
        {
            return (T)kernel.Resolve(type);
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

        public T GetInstance<T, TContext>(TContext context) where TContext : IContext
        {
            return kernel.Resolve<T>(context.GetValue());
        }
    }

    public static class GenericUseCaseExtensions
    {
        public static void Bind<TBaseType>(this GenericUseCase<TBaseType> useCase, IKernel kernel)
        {
            kernel.Register(Component.For(useCase.GetBinding()).Unless(Component.ServiceAlreadyRegistered));
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
