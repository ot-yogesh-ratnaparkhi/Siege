using Siege.Requisitions.Extensions.ExtendedRegistrationSyntax;

namespace Siege.Requisitions.Extensions.RubyInstaller
{
    public class ServiceLocatorProxy
    {
        private readonly IServiceLocator locator;

        public ServiceLocatorProxy(IServiceLocator locator)
        {
            this.locator = locator;
        }

        public void Bind<TFrom, TTo>() where TTo : TFrom
        {
            locator.Register(Given<TFrom>.Then<TTo>("test"));
        }
    }
}