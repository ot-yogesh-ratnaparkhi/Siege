namespace Siege.ServiceLocation.UseCases.Actions
{
    public interface IActionUseCase : IGenericUseCase
    {
        object Invoke(object item);
    }
}