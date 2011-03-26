using System;
using System.Reflection;

namespace Siege.Provisions.Mapping.Conventions.Handlers
{
    public class PropertyHandler : IHandler
    {
        public bool CanHandle(PropertyInfo property)
        {
            return !property.PropertyType.IsClass && !new IDHandler().CanHandle(property);
        }

        public void Handle(PropertyInfo property, Type type, DomainMapper mapper)
        {
            if (property.PropertyType.IsClass) return;

            mapper.For(type).Map(mapping => mapping.MapProperty(property));
        }
    }
}