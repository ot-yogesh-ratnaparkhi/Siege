using System;
using System.Linq.Expressions;
using System.Reflection;
using Siege.Provisions.Mapping.Conventions.Formatters;

namespace Siege.Provisions.Mapping.PropertyMappings
{
    public class IdMapping<TClass, TType> : PropertyMapping<TClass, TType>
    {
        public IdMapping(Expression<Func<TClass, TType>> expression, string name) : base(expression, name)
        {
        }

        public IdMapping(Expression<Func<TClass, TType>> expression) : base(expression)
        {
        }
    }

    public class IdMapping : PropertyMapping
    {
        public IdMapping(PropertyInfo property, Type type, Formatter<Type> keyFormatter) : base(property)
        {
            this.ColumnName = keyFormatter.Format(type);
        }
    }
}