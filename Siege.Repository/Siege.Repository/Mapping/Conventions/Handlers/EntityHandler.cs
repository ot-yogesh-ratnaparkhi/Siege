using System;
using System.Reflection;
using Siege.Repository.Mapping.Conventions.Formatters;
using Siege.Repository.Mapping.Conventions.Identifiers;

namespace Siege.Repository.Mapping.Conventions.Handlers
{
    public class EntityHandler : IHandler
    {
        private readonly IIdentifier<Type> entityIdentifier;
        private readonly Formatter<PropertyInfo> foreignKeyFormatter;
        private readonly DomainMapper masterMap;

        public EntityHandler(IIdentifier<Type> entityIdentifier, Formatter<PropertyInfo> foreignKeyFormatter, DomainMapper masterMap)
        {
            this.entityIdentifier = entityIdentifier;
            this.foreignKeyFormatter = foreignKeyFormatter;
            this.masterMap = masterMap;
        }

        public bool CanHandle(PropertyInfo property)
        {
            return entityIdentifier.Matches(property.PropertyType);
        }

        public void Handle(PropertyInfo property, Type type, DomainMapping mapper)
        {
            mapper.MapForeignRelationship(masterMap, property, property.PropertyType, foreignKeyFormatter);
        }
    }
}