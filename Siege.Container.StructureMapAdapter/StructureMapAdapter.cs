using System;
using Siege.ServiceLocation;
using StructureMap;
using IContext=Siege.ServiceLocation.IContext;

namespace Siege.Container.StructureMapAdapter
{
    public class StructureMapAdapter : IContextualServiceLocator
    {
        public T GetInstance<T>()
        {
            return ObjectFactory.GetInstance<T>();
        }

        public T GetInstance<T>(Type type)
        {
            return (T)ObjectFactory.GetInstance(type);
        }

        public void Register<T>(IUseCase<T> useCase)
        {
            if (useCase is GenericUseCase<T>)
            {
                GenericUseCase<T> genericCase = useCase as GenericUseCase<T>;

                genericCase.Bind();
            }

            if (useCase is ImplementationUseCase<T>)
            {
                var implementation = useCase as ImplementationUseCase<T>;

                implementation.Bind();
            }
        }

        public T GetInstance<T, TContext>(TContext context) where TContext : IContext
        {
            return ObjectFactory.With(context).GetInstance<T>();
        }
    }

    public static class GenericUseCaseExtensions
    {
        public static void Bind<TBaseType>(this GenericUseCase<TBaseType> useCase)
        {
            ObjectFactory.Initialize(registry => registry.ForRequestedType<TBaseType>().TheDefault.Is.OfConcreteType(useCase.GetBinding()));
        }
    }

    public static class ImplementationUseCaseExtensions
    {
        public static void Bind<TBaseType>(this ImplementationUseCase<TBaseType> useCase)
        {
            ObjectFactory.Initialize(registry => registry.ForRequestedType<TBaseType>().TheDefault.IsThis(useCase.GetBinding()));
        }
    }
}