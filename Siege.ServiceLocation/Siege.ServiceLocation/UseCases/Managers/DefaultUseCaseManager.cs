using Siege.ServiceLocation.Stores.UseCases;

namespace Siege.ServiceLocation.UseCases.Managers
{
    public class DefaultUseCaseManager : IUseCaseManager
    {
        private UseCaseStore useCaseStore;

        public DefaultUseCaseManager(UseCaseStore useCaseStore)
        {
            this.useCaseStore = useCaseStore;
        }

        public void Add(IUseCase useCase)
        {
            useCaseStore.Default.ResolutionCases.Add(useCase.GetBaseBindingType(), useCase);
        }
    }
}