using System;
using System.Collections;
using Siege.ServiceLocation;
using StructureMap;

namespace Siege.Container.StructureMapAdapter
{
    public class StructureMapAdapter : IServiceLocatorAdapter
    {
        private readonly StructureMap.Container container;

        private readonly Hashtable factories = new Hashtable();
        public StructureMapAdapter() : this(new StructureMap.Container(x => x.IncludeConfigurationFromConfigFile = true)) {}
        public StructureMapAdapter(StructureMap.Container container)
        {
            this.container = container;
        }

        private IContextualServiceLocator locator;

        public T GetInstance<T>()
        {
            return container.GetInstance<T>();
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
            return (T)container.GetInstance(type);
        }

        public T GetInstance<T>(Type type, IDictionary constructorArguments)
        {
            if (constructorArguments == null || constructorArguments.Count == 0) return (T)container.GetInstance(type);

            ExplicitArgsExpression expression = null;

            foreach (string key in constructorArguments.Keys)
            {
                if (expression == null)
                {
                    expression = container.With(key).EqualTo(constructorArguments[key]);
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
            if (constructorArguments == null || constructorArguments.Count == 0) return container.GetInstance<T>(name);

            ExplicitArgsExpression expression = null;

            foreach (string key in constructorArguments.Keys)
            {
                if (expression == null)
                {
                    expression = container.With(key).EqualTo(constructorArguments[key]);
                    continue;
                }

                expression.With(key).EqualTo(constructorArguments[key]);
            }

            return expression.GetInstance<T>(name);
        }

        public IServiceLocator Register<T>(IUseCase<T> useCase)
        {
            if(useCase is IConditionalUseCase<T>)
            {
                var conditionalCase = useCase as IConditionalUseCase<T>;

                conditionalCase.Bind(container, this);
            }
            else if (useCase is IKeyBasedUseCase<T>)
            {
                var keyCase = useCase as IKeyBasedUseCase<T>;

                keyCase.Bind(container);
            }
            else if (useCase is ImplementationUseCase<T>)
            {
                var implementation = useCase as ImplementationUseCase<T>;

                implementation.Bind(container);
            }
            else if (useCase is IDefaultUseCase<T>)
            {
                var genericCase = useCase as IDefaultUseCase<T>;

                genericCase.Bind(container, this);
            }

            return this;
        }

        public void RegisterParentLocator(IContextualServiceLocator locator)
        {
            this.locator = locator;

            Register(Given<IServiceLocator>.Then(locator));
            Register(Given<IContextualServiceLocator>.Then(locator));
        }

        public IGenericFactory<TBaseType> GetFactory<TBaseType>()
        {
            if (!factories.ContainsKey(typeof(TBaseType)))
            {
                lock (factories.SyncRoot)
                {
                    if (!factories.ContainsKey(typeof(TBaseType)))
                    {
                        Factory<TBaseType> factory = new Factory<TBaseType>(this.locator);
                        Register(Given<Factory<TBaseType>>.Then("Factory" + typeof(TBaseType), factory));

                        factories.Add(typeof(TBaseType), factory);
                    }
                }
            }

            return (Factory<TBaseType>)factories[typeof(TBaseType)];
        }

        public void Dispose()
        {
        }
    }
}