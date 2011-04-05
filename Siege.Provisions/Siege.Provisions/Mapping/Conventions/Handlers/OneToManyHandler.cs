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

        public OneToManyHandler(Formatter<PropertyInfo> foreignKeyFormatter)
        {
            this.foreignKeyFormatter = foreignKeyFormatter;
        }

        public bool CanHandle(PropertyInfo property)
        {
            return new GenericListIdentifier().Matches(property.PropertyType);
        }

        public void Handle(PropertyInfo property, Type type, DomainMapping mapper)
        {
            mapper.MapForeignRelationship(property, property.PropertyType.GetGenericArguments().First(), foreignKeyFormatter);
        }
    }
}