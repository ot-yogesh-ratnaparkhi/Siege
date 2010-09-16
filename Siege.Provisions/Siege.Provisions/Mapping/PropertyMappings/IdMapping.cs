using System;
using System.Linq.Expressions;

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
}