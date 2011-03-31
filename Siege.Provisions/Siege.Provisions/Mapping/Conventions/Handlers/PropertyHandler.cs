using System;
using System.Collections;
using System.Linq;
using System.Reflection;

namespace Siege.Provisions.Mapping.Conventions.Handlers
{
    public class PropertyHandler : IHandler
    {
        public bool CanHandle(PropertyInfo property)
        {
            return !property.PropertyType.IsClass && property.PropertyType.GetInterfaces().Where(i => i == typeof(IEnumerable)).Count() == 0;
        }

        public void Handle(PropertyInfo property, Type type, DomainMapping mapper)
        {
            mapper.MapProperty(property);
        }
    }
}