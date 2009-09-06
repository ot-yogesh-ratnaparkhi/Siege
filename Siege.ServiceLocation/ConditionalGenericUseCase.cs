namespace Siege.ServiceLocation
{
    public class ConditionalGenericUseCase<TBaseType> : GenericUseCase<TBaseType>, IConditionalUseCase<TBaseType>
    {
    }
}