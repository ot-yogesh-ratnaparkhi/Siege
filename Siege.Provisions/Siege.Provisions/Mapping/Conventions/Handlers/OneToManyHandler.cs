using System;
using System.Collections;
using System.Linq;
using System.Reflection;

namespace Siege.Provisions.Mapping.Conventions.Handlers
{
    public class OneToManyHandler : IHandler
    {
        public bool CanHandle(PropertyInfo property)
        {
            return property.PropertyType.IsGenericType && property.PropertyType.GetInterfaces().Where(i => i == typeof (IEnumerable)).Count() > 0;
        }

        public void Handle(PropertyInfo property, Type type, DomainMapping mapper)
        {
            mapper.MapForeignRelationship(property, property.PropertyType.GetGenericArguments().First());
        }
    }
}