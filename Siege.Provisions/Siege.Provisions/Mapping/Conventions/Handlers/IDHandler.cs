using System;
using System.Reflection;
using Siege.Provisions.Mapping.Conventions.Formatters;
using Siege.Provisions.Mapping.Conventions.Identifiers;

namespace Siege.Provisions.Mapping.Conventions.Handlers
{
    public class IDHandler : IHandler
    {
        private readonly IIdentifier<PropertyInfo> idIdentifier;
        private readonly Formatter<Type> primaryKeyFormatter;

        public IDHandler(IIdentifier<PropertyInfo> idIdentifier, Formatter<Type> primaryKeyFormatter)
        {
            this.idIdentifier = idIdentifier;
            this.primaryKeyFormatter = primaryKeyFormatter;
        }

        public bool CanHandle(PropertyInfo property)
        {
            return this.idIdentifier.Matches(property);
        }

        public void Handle(PropertyInfo property, Type type, DomainMapping mapper)
        {
            mapper.MapID(property, type, primaryKeyFormatter);
        }
    }
}