using System;
using Siege.ServiceLocation.Bindings.Default;
using Siege.ServiceLocation.UseCases;
using Siege.ServiceLocation.UseCases.Default;

namespace Siege.ServiceLocation.Extensions.AutoScanner
{
    public class AutoScannedUseCase : GenericUseCase, IDefaultUseCase
    {
        public AutoScannedUseCase(Type baseType, Type targetType) : base(baseType)
        {
            BindTo(targetType);
        }

        public override Type GetUseCaseBindingType()
        {
            return typeof (DefaultUseCaseBinding);
        }
    }
}