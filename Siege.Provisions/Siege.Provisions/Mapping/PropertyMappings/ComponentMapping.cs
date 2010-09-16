using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Siege.Provisions.Mapping.PropertyMappings
{
    public class ComponentMapping<TClass, TType> : ElementMapping<TClass, TType>, IComponentPropertyMapping
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
}