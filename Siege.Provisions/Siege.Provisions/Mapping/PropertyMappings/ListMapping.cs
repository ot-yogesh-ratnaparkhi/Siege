using System;
using System.Linq.Expressions;

namespace Siege.Provisions.Mapping.PropertyMappings
{
    public class ListMapping<TClass, TType> : ElementMapping<TClass, TType>
    {
        public ListMapping(Expression<Func<TClass, TType>> expression) : base(expression)
        {
        }
    }
}