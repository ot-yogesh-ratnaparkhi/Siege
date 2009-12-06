using System;

namespace Siege.ServiceLocation
{
    public class DefaultUseCase<TBaseService> : GenericUseCase<TBaseService>, IDefaultUseCase<TBaseService> 
    {
        public override Type GetUseCaseBindingType()
        {
            return typeof (IDefaultUseCaseBinding<>);
        }
    }
}