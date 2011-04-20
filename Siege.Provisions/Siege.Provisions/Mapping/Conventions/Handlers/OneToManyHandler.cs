using System;
using System.Linq;
using System.Reflection;
using Siege.Provisions.Mapping.Conventions.Formatters;
using Siege.Provisions.Mapping.Conventions.Identifiers;

namespace Siege.Provisions.Mapping.Conventions.Handlers
{
    public class OneToManyHandler : IHandler
    {
        private readonly Formatter<PropertyInfo> foreignKeyFormatter;
        private readonly DomainMapper masterMap;

        public OneToManyHandler(Formatter<PropertyInfo> foreignKeyFormatter, DomainMapper masterMap)
        {
            this.foreignKeyFormatter = foreignKeyFormatter;
            this.masterMap = masterMap;
        }

        public bool CanHandle(PropertyInfo property)
        {
            return new GenericListIdentifier().Matches(property.PropertyType);
        }

        public void Handle(PropertyInfo property, Type type, DomainMapping mapper)
        {
            mapper.MapForeignRelationship(masterMap, property, property.PropertyType.GetGenericArguments().First(), foreignKeyFormatter);
        }
    }
}