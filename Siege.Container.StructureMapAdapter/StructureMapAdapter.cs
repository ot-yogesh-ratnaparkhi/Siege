using System;
using System.Collections;
using Siege.ServiceLocation;
using StructureMap;

namespace Siege.Container.StructureMapAdapter
{
    public class StructureMapAdapter : IServiceLocator
    {
        public T GetInstance<T>()
        {
            return ObjectFactory.GetInstance<T>();
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
            return (T)ObjectFactory.GetInstance(type);
        }

        public T GetInstance<T>(Type type, IDictionary constructorArguments)
        {
            if (constructorArguments == null || constructorArguments.Count == 0) return (T)ObjectFactory.GetInstance(type);

            ExplicitArgsExpression expression = null;

            foreach (string key in constructorArguments.Keys)
            {
                if (expression == null)
                {
                    expression = ObjectFactory.With(key).EqualTo(constructorArguments[key]);
                    continue;
                }

                expression.With(key).EqualTo(constructorArguments[key]);
            }

            return (T)expression.GetInstance(type);
        }

        public T GetInstance<T>(string key)
        {
            return GetInstance<T>(key, null);
        }

        public T GetInstance<T>(string name, IDictionary constructorArguments)
        {
            if (constructorArguments == null || constructorArguments.Count == 0) return ObjectFactory.GetNamedInstance<T>(name);

            ExplicitArgsExpression expression = null;

            foreach (string key in constructorArguments.Keys)
            {
                if (expression == null)
                {
                    expression = ObjectFactory.With(key).EqualTo(constructorArguments[key]);
                    continue;
                }

                expression.With(key).EqualTo(constructorArguments[key]);
            }

            return expression.GetInstance<T>(name);
        }

        public IServiceLocator Register<T>(IUseCase<T> useCase)
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

            if (useCase is KeyBasedUseCase<T>)
            {
                var keyCase = useCase as KeyBasedUseCase<T>;

                keyCase.Bind();
            }

            return this;
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

    public static class KeyBasedUseCaseExtensions
    {
        public static void Bind<TBaseType>(this KeyBasedUseCase<TBaseType> useCase)
        {
            ObjectFactory.Initialize(registry => registry
                                                    .ForRequestedType<TBaseType>()
                                                    .AddInstances(x => x.OfConcreteType(useCase.GetBinding())
                                                    .WithName(useCase.Key)));
        }
    }
}