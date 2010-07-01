using Siege.ServiceLocation.Stores.UseCases;

namespace Siege.ServiceLocation.UseCases.Managers
{
    public class ConditionalUseCaseManager : IUseCaseManager
    {
        private UseCaseStore useCaseStore;

        public ConditionalUseCaseManager(UseCaseStore useCaseStore)
        {
            this.useCaseStore = useCaseStore;
        }

        public void Add(IUseCase useCase)
        {
            useCaseStore.Conditional.ResolutionCases.Add(useCase.GetBaseBindingType(), useCase);
        }
    }
}