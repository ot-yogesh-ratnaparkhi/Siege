using System;
using System.Collections;
using System.Collections.Generic;
using Siege.ServiceLocation;

namespace Siege.Container
{
    public class SiegeContainer : IContextualServiceLocator
    {
        private readonly IServiceLocator serviceLocator;
        private readonly Hashtable useCases = new Hashtable();
        private readonly Hashtable registeredImplementors = new Hashtable();
        private readonly Hashtable registeredTypes = new Hashtable();
        private readonly Hashtable defaultCases = new Hashtable();

        public SiegeContainer(IServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        public TOutput GetInstance<TOutput, TContext>(TContext context)
        {
            return GetInstance<TOutput, TContext>(typeof(TOutput), context, null);
        }

        public TOutput GetInstance<TOutput, TContext>(TContext context, IDictionary constructorArguments)
        {
            return GetInstance<TOutput, TContext>(typeof(TOutput), context, constructorArguments);
        }

        public TOutput GetInstance<TOutput>()
        {
            return GetInstance<TOutput>(typeof(TOutput));
        }

        public TOutput GetInstance<TOutput>(Type type)
        {
            return GetInstance<TOutput>(type, null);
        }

        public TOutput GetInstance<TOutput, TContext>(Type type, TContext context)
        {
            return GetInstance<TOutput, TContext>(type, context, null);
        }

        public TOutput GetInstance<TOutput>(IDictionary constructorArguments)
        {
            return GetInstance<TOutput>(typeof(TOutput), constructorArguments);
        }

        public T GetInstance<T>(object anonymousConstructorArguments)
        {
            return GetInstance<T>(anonymousConstructorArguments.AnonymousTypeToDictionary());
        }

        public T GetInstance<T, TContext>(TContext context, object anonymousConstructorArguments)
        {
            return this.GetInstance<T, TContext>(context, anonymousConstructorArguments.AnonymousTypeToDictionary());
        }

        public T GetInstance<T, TContext>(Type type, TContext context, object anonymousConstructorArguments)
        {
            return this.GetInstance<T, TContext>(type, context, anonymousConstructorArguments.AnonymousTypeToDictionary());
        }

        public TOutput GetInstance<TOutput, TContext>(Type type, TContext context, IDictionary constructorArguments)
        {
            IList<IUseCase> selectedCase = (IList<IUseCase>)useCases[type];

            if (selectedCase != null)
            {
                foreach (IUseCase<TOutput> useCase in selectedCase)
                {
                    TOutput value = useCase.Resolve(serviceLocator, context, constructorArguments);

                    if (!Equals(value, default(TOutput))) return value;
                }
            }

            if (defaultCases.ContainsKey(type))
            {
                DefaultUseCase<TOutput> useCase = (DefaultUseCase<TOutput>)defaultCases[type];
                return serviceLocator.GetInstance<TOutput>(useCase.GetBinding(), constructorArguments);
            }

            return serviceLocator.GetInstance<TOutput>(constructorArguments);
        }

        public TOutput GetInstance<TOutput>(Type type, IDictionary constructorArguments)
        {
            DefaultUseCase<TOutput> defaultCase = (DefaultUseCase<TOutput>)defaultCases[typeof(TOutput)];
            if (defaultCase != null) return serviceLocator.GetInstance<TOutput>(defaultCase.GetBinding(), constructorArguments);

            return serviceLocator.GetInstance<TOutput>(constructorArguments);
        }

        public IServiceLocator Register<T>(IUseCase<T> useCase)
        {
            if (useCase is DefaultUseCase<T>)
            {
                defaultCases.Add(typeof(T), useCase);
            }
            else
            {

                if (!useCases.ContainsKey(typeof(T)))
                {
                    List<IUseCase> list = new List<IUseCase>();

                    useCases.Add(typeof(T), list);
                }

                IList<IUseCase> selectedCase = (IList<IUseCase>)useCases[typeof(T)];

                selectedCase.Add(useCase);
            }

            if (!registeredTypes.ContainsKey(typeof(T))) registeredTypes.Add(typeof(T), typeof(T));
            if (!registeredImplementors.ContainsKey(useCase.GetType())) registeredImplementors.Add(useCase.GetType(), useCase.GetType());

            serviceLocator.Register(useCase);

            return this;
        }
    }
}