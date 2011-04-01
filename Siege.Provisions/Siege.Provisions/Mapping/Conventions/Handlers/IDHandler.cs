using System;
using System.Reflection;
using Siege.Provisions.Mapping.Conventions.Identifiers;

namespace Siege.Provisions.Mapping.Conventions.Handlers
{
    public class IDHandler : IHandler
    {
        private readonly IIdentifier<PropertyInfo> idIdentifier;

        public IDHandler(IIdentifier<PropertyInfo> idIdentifier)
        {
            this.idIdentifier = idIdentifier;
        }

        public bool CanHandle(PropertyInfo property)
        {
            return this.idIdentifier.Matches(property);
        }

        public void Handle(PropertyInfo property, Type type, DomainMapping mapper)
        {
            mapper.MapID(property);
        }
    }
}