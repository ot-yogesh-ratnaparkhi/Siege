namespace Siege.ServiceLocation
{
    public class ConditionalGenericUseCase<TBaseService> : GenericUseCase<TBaseService>, IConditionalUseCase<TBaseService>
    {
    }
}