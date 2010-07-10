using System;
using Siege.ServiceLocation.Registrations;

namespace Siege.ServiceLocation.RegistrationTemplates.Registration
{
    public class ConditionalRegistrationTemplate : IRegistrationTemplate
    {
        public void Register(IServiceLocatorAdapter adapter, IRegistration registration, IFactoryFetcher locator)
        {
            throw new NotImplementedException();
        }
    }
}