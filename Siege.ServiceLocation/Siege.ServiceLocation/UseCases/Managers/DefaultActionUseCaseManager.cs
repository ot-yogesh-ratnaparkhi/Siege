using Siege.ServiceLocation.Stores.UseCases;

namespace Siege.ServiceLocation.UseCases.Managers
{
    public class DefaultActionUseCaseManager : IUseCaseManager
    {
        private UseCaseStore useCaseStore;

        public DefaultActionUseCaseManager(UseCaseStore useCaseStore)
        {
            this.useCaseStore = useCaseStore;
        }

        public void Add(IUseCase useCase)
        {
            useCaseStore.Default.PostResolutionCases.Add(useCase.GetBoundType(), useCase);
        }
    }
}