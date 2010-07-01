using Siege.ServiceLocation.Stores.UseCases;

namespace Siege.ServiceLocation.UseCases.Managers
{
    public class ConditionalActionUseCaseManager : IUseCaseManager
    {
        private UseCaseStore useCaseStore;

        public ConditionalActionUseCaseManager(UseCaseStore useCaseStore)
        {
            this.useCaseStore = useCaseStore;
        }

        public void Add(IUseCase useCase)
        {
            useCaseStore.Conditional.PostResolutionCases.Add(useCase.GetBoundType(), useCase);
        }
    }
}