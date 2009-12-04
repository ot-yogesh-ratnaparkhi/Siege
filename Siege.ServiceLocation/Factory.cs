using System.Collections;
using System.Collections.Generic;

namespace Siege.ServiceLocation
{
    public class Factory<TBaseService> : IGenericFactory<TBaseService>
    {
        private readonly IContextualServiceLocator serviceLocator;
        private readonly List<IConditionalUseCase<TBaseService>> conditionalUseCases = new List<IConditionalUseCase<TBaseService>>();
        private readonly List<IDefaultUseCase<TBaseService>> defaultCases = new List<IDefaultUseCase<TBaseService>>();

        public Factory(IContextualServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        public List<IConditionalUseCase<TBaseService>> ConditionalUseCases
        {
            get { return conditionalUseCases; }
        }

        public List<IDefaultUseCase<TBaseService>> DefaultUseCases
        {
            get { return defaultCases; }
        }

        public void AddCase(IConditionalUseCase<TBaseService> useCase)
        {
            ConditionalUseCases.Add(useCase);
        }

        public void AddCase(IDefaultUseCase<TBaseService> useCase)
        {
            DefaultUseCases.Add(useCase);
        }

        public TBaseService Build(IDictionary constructorArguments)
        {
            foreach (IConditionalUseCase<TBaseService> useCase in ConditionalUseCases)
            {
                TBaseService result = (TBaseService)useCase.Resolve(serviceLocator, serviceLocator.Context);

                if (!Equals(result, default(TBaseService))) return result;
            }

            foreach (IDefaultUseCase<TBaseService> useCase in defaultCases)
            {
                TBaseService result = (TBaseService)useCase.Resolve(serviceLocator);

                if (!Equals(result, default(TBaseService))) return result;
            }

            return serviceLocator.GetInstance<TBaseService>();
        }
    }
}