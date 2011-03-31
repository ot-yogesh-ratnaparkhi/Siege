using System;
using System.Collections;
using System.Linq;
using System.Reflection;

namespace Siege.Provisions.Mapping.Conventions.Handlers
{
    public class ComponentHandler : IHandler
    {
        private Predicate<Type> componentMatcher;
        private Action<ComponentConvention> componentConvention;

        public void MatchesOn(Predicate<Type> componentMatcher)
        {
            this.componentMatcher = componentMatcher;
        }

        public void HandlesWith(Action<ComponentConvention> convention)
        {
            this.componentConvention = convention;
        }

        public bool CanHandle(PropertyInfo property)
        {
            return property.PropertyType.GetInterfaces().Where(i => i == typeof(IEnumerable)).Count() == 0 && (this.componentMatcher != null) && this.componentMatcher(property.PropertyType);
        }

        public void Handle(PropertyInfo property, Type type, DomainMapping mapper)
        {
            var convention = new ComponentConvention(property);

            componentConvention(convention);

            convention.Map(type, mapper);
        }
    }
}