using System;
using System.Reflection;

namespace Siege.Provisions.Mapping.Conventions
{
    public class ComponentConvention : IConvention
    {
        private readonly PropertyInfo property;
        protected Func<Type, string, string> prefix;
        protected Func<Type, string, string> suffix;

        public ComponentConvention(PropertyInfo property)
        {
            this.property = property;
        }

        public ComponentConvention PrefixWith(Func<Type, string, string> formatter)
        {
            this.prefix = formatter;
            return this;
        }

        public ComponentConvention SuffixWith(Func<Type, string, string> suffix)
        {
            this.suffix = suffix;
            return this;
        }

        public void Map(Type type, DomainMapping mapper)
        {
            mapper.MapComponent(this.property, subMapping =>
            {
                foreach (var typeProperty in this.property.PropertyType.GetProperties())
                {
                    subMapping.MapProperty(typeProperty);
                }
            });
        }
    }
}