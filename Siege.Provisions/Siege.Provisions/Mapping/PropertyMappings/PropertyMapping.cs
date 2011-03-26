using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Siege.Provisions.Mapping.PropertyMappings
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
        public PropertyMapping(PropertyInfo property) : base(property)
        {
            this.ColumnName = Property.Name;
        }

        public string ColumnName { get; protected set; }
    }
}