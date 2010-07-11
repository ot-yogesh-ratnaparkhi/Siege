using System;
using Siege.ServiceLocation.Registrations;
using Siege.ServiceLocation.Resolution;

namespace Siege.ServiceLocation.RegistrationTemplates.Registration
{
    public class DefaultRegistrationTemplate : IRegistrationTemplate
    {
        public void Register(IServiceLocatorAdapter adapter, IRegistration registration, IResolutionTemplate template)
        {
            throw new NotImplementedException();
        }
    }
}