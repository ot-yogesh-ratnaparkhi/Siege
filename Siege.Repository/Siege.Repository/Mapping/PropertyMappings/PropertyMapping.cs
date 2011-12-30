using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Siege.Repository.Mapping.PropertyMappings
{
    public class PropertyMapping<TClass, TType> : ElementMapping<TClass, TType>, IPropertyMapping
    {
        public PropertyMapping(Expression<Func<TClass, TType>> expression, string name) : base(expression)
        {
            this.ColumnName = name;
        }

        public PropertyMapping(Expression<Func<TClass, TType>> expression) : base(expression)
        {
            this.ColumnName = Property.Name;
        }

        public string ColumnName { get; protected set; }
    }

    public class PropertyMapping : ElementMapping, IPropertyMapping
    {
        public PropertyMapping(PropertyInfo property) : this(property, property.Name)
        {
        }

        public PropertyMapping(PropertyInfo property, string name) : base(property)
        {
            this.ColumnName = name;
        }

        public virtual string ColumnName { get; protected set; }
    }
}