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

        public void AddCase(IConditionalUseCase<TBaseService> useCase)
        {
            conditionalUseCases.Add(useCase);
        }

        public void AddCase(IDefaultUseCase<TBaseService> useCase)
        {
            defaultCases.Add(useCase);
        }

        public TBaseService Build()
        {
            foreach (IConditionalUseCase<TBaseService> useCase in conditionalUseCases)
            {
                TBaseService result = default(TBaseService);

                if(useCase.IsValid(serviceLocator.Context)) result = (TBaseService)useCase.Resolve(serviceLocator, serviceLocator.Context);

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