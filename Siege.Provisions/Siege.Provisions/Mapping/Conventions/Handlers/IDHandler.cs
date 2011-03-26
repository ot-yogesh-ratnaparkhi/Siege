using System;
using System.Reflection;

namespace Siege.Provisions.Mapping.Conventions.Handlers
{
    public class IDHandler : IHandler
    {
        public bool CanHandle(PropertyInfo property)
        {
            return property.Name.ToLower().EndsWith("id");
        }

        public void Handle(PropertyInfo property, Type type, DomainMapper mapper)
        {
            
        }
    }
}