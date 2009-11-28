using System;

namespace Siege.ServiceLocation
{
    public class DefaultInstanceUseCase<TBaseService> : InstanceUseCase<TBaseService>, IDefaultUseCase<TBaseService>
    {
        public override Type GetUseCaseBindingType()
        {
            return typeof (IDefaultInstanceUseCaseBinding<>);
        }
    }
}