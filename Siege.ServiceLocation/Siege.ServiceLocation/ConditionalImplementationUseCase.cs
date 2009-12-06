using System;

namespace Siege.ServiceLocation
{
    public class ConditionalInstanceUseCase<TBaseService> : InstanceUseCase<TBaseService>, IConditionalUseCase<TBaseService>
    {
        public override Type GetUseCaseBindingType()
        {
            return typeof (IConditionalUseCaseBinding<>);
        }
    }
}