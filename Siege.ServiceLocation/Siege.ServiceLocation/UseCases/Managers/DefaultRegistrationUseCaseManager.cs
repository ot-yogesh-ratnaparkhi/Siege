using Siege.ServiceLocation.Stores.UseCases;

namespace Siege.ServiceLocation.UseCases.Managers
{
    public class DefaultRegistrationUseCaseManager : IUseCaseManager
    {
        private UseCaseStore useCaseStore;

        public DefaultRegistrationUseCaseManager(UseCaseStore useCaseStore)
        {
            this.useCaseStore = useCaseStore;
        }

        public void Add(IUseCase useCase)
        {
            useCaseStore.Default.RegistrationCases.Add(useCase.GetBoundType(), useCase);
        }
    }
}