using Siege.ServiceLocation.UseCases.Conditional;
using Siege.ServiceLocation.UseCases.Default;

namespace Siege.ServiceLocation.UseCases.Actions
{
    public interface IDefaultActionUseCase : IDefaultUseCase
    {
        void Invoke(object item);
    }
    
    public interface IConditionalActionUseCase : IConditionalUseCase
    {
        void Invoke(object item);
    } 
}