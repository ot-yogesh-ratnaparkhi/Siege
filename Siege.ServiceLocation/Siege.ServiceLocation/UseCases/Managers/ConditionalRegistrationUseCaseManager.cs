using Siege.ServiceLocation.Stores.UseCases;

namespace Siege.ServiceLocation.UseCases.Managers
{
    public class ConditionalRegistrationUseCaseManager : IUseCaseManager
    {
        private UseCaseStore useCaseStore;

        public ConditionalRegistrationUseCaseManager(UseCaseStore useCaseStore)
        {
            this.useCaseStore = useCaseStore;
        }

        public void Add(IUseCase useCase)
        {
            useCaseStore.Conditional.RegistrationCases.Add(useCase.GetBoundType(), useCase);
        }
    }
}