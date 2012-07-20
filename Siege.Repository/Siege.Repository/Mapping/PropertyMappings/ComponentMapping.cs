using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Siege.Repository.Mapping.PropertyMappings
{
    public class ComponentMapping<TClass, TType> : ElementMapping<TClass, TType>
    {
        private readonly List<IElementMapping> subMappings = new List<IElementMapping>();
        private readonly string name;

        public string Name { get { return name; } }
        public List<IElementMapping> SubMappings { get { return subMappings; } }

        public ComponentMapping(Expression<Func<TClass, TType>> expression, string name) : base(expression)
        {
            this.name = name;
        }

        public ComponentMapping(Expression<Func<TClass, TType>> expression) : base(expression)
        {
            this.name = Property.Name;
        }

        public ComponentMapping<TClass, TType> MapProperty<TComponentType>(Expression<Func<TType, TComponentType>> expression)
        {
            this.subMappings.Add(new PropertyMapping<TType, TComponentType>(expression));
            return this;
        }

        public ComponentMapping<TClass, TType> MapProperty<TComponentType>(Expression<Func<TType, TComponentType>> expression, string columnName)
        {
            this.subMappings.Add(new PropertyMapping<TType, TComponentType>(expression, columnName));
            return this;
        }
    }

    public class ComponentMapping : ElementMapping
    {
        public ComponentMapping(PropertyInfo property) : base(property)
        {
        }

        private readonly List<IElementMapping> subMappings = new List<IElementMapping>();
        public List<IElementMapping> SubMappings { get { return subMappings; } }

        public ComponentMapping MapProperty(PropertyInfo property, string name)
        {
            this.subMappings.Add(new PropertyMapping(property, name));
            return this;
        }
    }
}