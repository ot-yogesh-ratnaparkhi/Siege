using System;
using System.Linq.Expressions;

namespace Siege.Provisions.Mapping.PropertyMappings
{
    public class PropertyMapping<TClass, TType> : ElementMapping<TClass, TType>, IPropertyMapping
    {
        private readonly string columnName;

        public PropertyMapping(Expression<Func<TClass, TType>> expression, string name) : base(expression)
        {
            columnName = name;
        }

        public PropertyMapping(Expression<Func<TClass, TType>> expression) : base(expression)
        {
            columnName = Property.Name;
        }

        public string ColumnName
        {
            get { return columnName; }
        }
    }
}