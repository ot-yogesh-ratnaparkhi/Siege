using Siege.ServiceLocation.UseCases.Conditional;
using Siege.ServiceLocation.UseCases.Default;

namespace Siege.ServiceLocation.UseCases.Actions
{
    public interface IDefaultActionUseCase : IDefaultUseCase
    {
        object Invoke(object item);
    }
    
    public interface IConditionalActionUseCase : IConditionalUseCase
    {
        object Invoke(object item);
    } 
}