namespace Siege.ServiceLocation
{
    public class ConditionalInstanceUseCase<TBaseService> : InstanceUseCase<TBaseService>, IConditionalUseCase<TBaseService>
    {
    }
}