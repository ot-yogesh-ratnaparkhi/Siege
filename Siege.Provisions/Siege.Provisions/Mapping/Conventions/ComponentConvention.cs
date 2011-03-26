using System;

namespace Siege.Provisions.Mapping.Conventions
{
    public class ComponentConvention : IConvention
    {
        protected Func<Type, string, string> prefix;
        protected Func<Type, string, string> suffix;

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

        public void Map(Type type, DomainMapper mapper)
        {
            mapper.For(type).Map(mapping => mapping.);
        }
    }
}