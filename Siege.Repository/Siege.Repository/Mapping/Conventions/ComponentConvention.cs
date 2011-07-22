using System;
using System.Reflection;

namespace Siege.Repository.Mapping.Conventions
{
    public class ComponentConvention : IConvention
    {
        private readonly PropertyInfo property;
        protected Func<Type, Type, string, string> prefix;
        protected Func<Type, Type, string, string> suffix;

        public ComponentConvention(PropertyInfo property)
        {
            this.property = property;
        }

        public ComponentConvention PrefixWith(Func<Type, Type, string, string> formatter)
        {
            this.prefix = formatter;
            return this;
        }

        public ComponentConvention SuffixWith(Func<Type, Type, string, string> suffix)
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
                    string propertyName = this.prefix(type, property.PropertyType, typeProperty.Name) + typeProperty.Name + this.suffix(type, property.PropertyType, typeProperty.Name);
                    subMapping.MapProperty(typeProperty, propertyName);
                }
            });
        }
    }
}