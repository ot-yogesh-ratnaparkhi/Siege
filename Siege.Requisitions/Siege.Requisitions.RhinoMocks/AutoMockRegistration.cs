using System;
using Siege.Requisitions.Registrations;
using Siege.Requisitions.Registrations.Stores;
using Siege.Requisitions.RegistrationTemplates;
using Siege.Requisitions.RegistrationTemplates.Default;

namespace Siege.Requisitions.AutoMocker
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
