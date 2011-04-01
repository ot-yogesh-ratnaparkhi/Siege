using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using Siege.Provisions.Mapping.Conventions.Identifiers;

namespace Siege.Provisions.Mapping.Conventions.Handlers
{
    public class ComponentHandler : IHandler
    {
        private IIdentifier<Type> componentIdentifier;
        private Action<ComponentConvention> componentConvention;

        public void MatchesOn(IIdentifier<Type> componentIdentifier)
        {
            this.componentIdentifier = componentIdentifier;
        }

        public void HandlesWith(Action<ComponentConvention> convention)
        {
            this.componentConvention = convention;
        }

        public bool CanHandle(PropertyInfo property)
        {
            return property.PropertyType.GetInterfaces().Where(i => i == typeof(IEnumerable)).Count() == 0 && (this.componentIdentifier != null) && this.componentIdentifier.Matches(property.PropertyType);
        }

        public void Handle(PropertyInfo property, Type type, DomainMapping mapper)
        {
            var convention = new ComponentConvention(property);

            componentConvention(convention);

            convention.Map(type, mapper);
        }
    }
}