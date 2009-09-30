using System.Collections;
using System.Collections.Generic;

namespace Siege.ServiceLocation
{
    public class Factory<TBaseType> : IGenericFactory<TBaseType>
    {
        private readonly IContextualServiceLocator serviceLocator;
        private readonly List<IConditionalUseCase<TBaseType>> conditionalUseCases = new List<IConditionalUseCase<TBaseType>>();
        private readonly List<IDefaultUseCase<TBaseType>> defaultCases = new List<IDefaultUseCase<TBaseType>>();

        public Factory(IContextualServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        public List<IConditionalUseCase<TBaseType>> ConditionalUseCases
        {
            get { return conditionalUseCases; }
        }

        public List<IDefaultUseCase<TBaseType>> DefaultUseCases
        {
            get { return defaultCases; }
        }

        public void AddCase(IConditionalUseCase<TBaseType> useCase)
        {
            ConditionalUseCases.Add(useCase);
        }

        public void AddCase(IDefaultUseCase<TBaseType> useCase)
        {
            DefaultUseCases.Add(useCase);
        }

        public TBaseType Build(IDictionary constructorArguments)
        {
            foreach (IConditionalUseCase<TBaseType> useCase in ConditionalUseCases)
            {
                TBaseType result = (TBaseType)useCase.Resolve(serviceLocator, serviceLocator.Context, constructorArguments);

                if (!Equals(result, default(TBaseType))) return result;
            }

            foreach (IDefaultUseCase<TBaseType> useCase in defaultCases)
            {
                TBaseType result = (TBaseType)useCase.Resolve(serviceLocator, constructorArguments);

                if (!Equals(result, default(TBaseType))) return result;
            }

            return serviceLocator.GetInstance<TBaseType>();
        }
    }
}