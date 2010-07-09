using System;
using System.Collections.Generic;
using Siege.ServiceLocation.Stores.UseCases;

namespace Siege.ServiceLocation.UseCases.Managers
{
    public class DefaultRegistrationUseCaseManager : IUseCaseManager
    {
        private readonly ConditionalUseCaseList useCases = new ConditionalUseCaseList();

        public void Add(IUseCase useCase)
        {
            useCases.Add(useCase.GetBoundType(), useCase);
        }

        public List<IUseCase> GetUseCasesForType(Type type)
        {
            return useCases.GetUseCasesForType(type);
        }

        public bool Contains(Type type)
        {
            return useCases.Contains(type);
        }
    }
}