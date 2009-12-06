using System;

namespace Siege.ServiceLocation
{
    public class ConditionalGenericUseCase<TBaseService> : GenericUseCase<TBaseService>, IConditionalUseCase<TBaseService>
    {
        public override Type GetUseCaseBindingType()
        {
            return typeof (IConditionalUseCaseBinding<>);
        }
    }
}