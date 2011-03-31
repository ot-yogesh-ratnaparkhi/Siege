using System;
using System.Reflection;

namespace Siege.Provisions.Mapping.Conventions.Handlers
{
    public class EntityHandler : IHandler
    {
        private readonly Predicate<Type> entityMatcher;

        public EntityHandler(Predicate<Type> entityMatcher)
        {
            this.entityMatcher = entityMatcher;
        }

        public bool CanHandle(PropertyInfo property)
        {
            return entityMatcher(property.PropertyType);
        }

        public void Handle(PropertyInfo property, Type type, DomainMapping mapper)
        {
            throw new NotImplementedException();
        }
    }
}