using System;
using Siege.ServiceLocator.Registrations;
using Siege.ServiceLocator.Registrations.Stores;
using Siege.ServiceLocator.RegistrationTemplates;
using Siege.ServiceLocator.RegistrationTemplates.Default;

namespace Siege.ServiceLocator.AutoMocker
{
    public class AutoMockRegistration : InstanceRegistration<object>
    {
        private readonly Type from;
        private readonly DefaultInstanceRegistrationTemplate defaultInstanceRegistrationTemplate = new DefaultInstanceRegistrationTemplate();
        private readonly DefaultRegistrationStore defaultRegistrationStore = new DefaultRegistrationStore();

        public AutoMockRegistration(Type from, object to)
        {
            this.from = from;
            this.implementation = to;
        }

        public override IRegistrationStore GetRegistrationStore()
        {
            return defaultRegistrationStore;
        }

        public override IRegistrationTemplate GetRegistrationTemplate()
        {
            return defaultInstanceRegistrationTemplate;
        }

        public override Type GetMappedFromType()
        {
            return from;
        }
    }
}
