using System;
using System.Collections;
using System.Collections.Generic;
using Siege.ServiceLocation;
using Siege.ServiceLocation.Exceptions;

namespace Siege.Container
{
    public class SimpleContainer : IContextualServiceLocator
    {
        private readonly IContextualServiceLocator serviceLocator;
        private readonly Hashtable useCases = new Hashtable();
        private readonly Hashtable registeredImplementors = new Hashtable();
        private readonly Hashtable registeredTypes = new Hashtable();

        public SimpleContainer(IContextualServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        public TOutput GetInstance<TOutput, TContext>(TContext context)
            where TContext : IContext
        {
            if (!registeredTypes.ContainsKey(typeof(TOutput))) throw new TypeNotRegisteredException(typeof(TOutput));

            IList<IUseCase> selectedCase = (IList<IUseCase>)useCases[typeof(TOutput)];

            foreach(IUseCase<TOutput, Type> useCase in selectedCase)
            {
                Type value = useCase.Resolve(context);

                if (value != null) return this.serviceLocator.GetInstance<TOutput>(value);
            }

            return this.serviceLocator.GetInstance<TOutput, TContext>(context);
        }

        public TOutput GetInstance<TOutput>()
        {
            if (!registeredTypes.ContainsKey(typeof(TOutput))) throw new TypeNotRegisteredException(typeof(TOutput));

            return serviceLocator.GetInstance<TOutput>();
        }

        public T GetInstance<T>(Type type)
        {
            return serviceLocator.GetInstance<T>(type);
        }

        public TOutput GetInstance<TOutput>(string name)
        {
            return serviceLocator.GetInstance<TOutput>(name);
        }

        public void Register<T>(IUseCase<T> useCase)
        {
            if(!useCases.ContainsKey(typeof(T)))
            {
                List<IUseCase> list = new List<IUseCase>();

                useCases.Add(typeof(T), list);
            }

            IList<IUseCase> selectedCase = (IList<IUseCase>) useCases[typeof(T)];

            selectedCase.Add(useCase);

            if(!registeredTypes.ContainsKey(typeof(T))) registeredTypes.Add(typeof(T), typeof(T));
            if(!registeredImplementors.ContainsKey(useCase.GetType())) registeredImplementors.Add(useCase.GetType(), useCase.GetType());

            serviceLocator.Register(useCase);
        }
    }
}
