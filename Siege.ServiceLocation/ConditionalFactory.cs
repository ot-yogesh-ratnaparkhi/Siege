using System;
using System.Collections.Generic;

namespace Siege.ServiceLocation
{
    public class ConditionalFactory<TBaseType>
    {
        private readonly IContextualServiceLocator serviceLocator;
        private readonly List<IConditionalUseCase<TBaseType>> useCases = new List<IConditionalUseCase<TBaseType>>();

        public ConditionalFactory(IContextualServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        public List<IConditionalUseCase<TBaseType>> UseCases
        {
            get { return useCases; }
        }

        public void AddCase(IConditionalUseCase<TBaseType> useCase)
        {
            UseCases.Add(useCase);
        }

        public TBaseType Build()
        {
            foreach(IConditionalUseCase<TBaseType> useCase in UseCases)
            {
                TBaseType result = useCase.Resolve(serviceLocator, serviceLocator.Context, null);

                if(!Equals(result, default(TBaseType))) return result;
            }

            throw new NotImplementedException();
        }
    }
}
