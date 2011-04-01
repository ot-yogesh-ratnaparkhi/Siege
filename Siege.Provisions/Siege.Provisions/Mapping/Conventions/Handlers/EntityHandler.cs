using System;
using System.Reflection;
using Siege.Provisions.Mapping.Conventions.Identifiers;

namespace Siege.Provisions.Mapping.Conventions.Handlers
{
    public class EntityHandler : IHandler
    {
        private readonly IIdentifier<Type> entityIdentifier;

        public EntityHandler(IIdentifier<Type> entityIdentifier)
        {
            this.entityIdentifier = entityIdentifier;
        }

        public bool CanHandle(PropertyInfo property)
        {
            return entityIdentifier.Matches(property.PropertyType);
        }

        public void Handle(PropertyInfo property, Type type, DomainMapping mapper)
        {
            mapper.MapForeignRelationship(property, property.PropertyType);
        }
    }
}