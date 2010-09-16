using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Siege.Provisions.Mapping.PropertyMappings
{
    public class ElementMapping<TClass, TType> : IElementMapping
    {
        private readonly PropertyInfo property;

        public ElementMapping(Expression<Func<TClass, TType>> expression)
        {
            if (!(expression.Body is MemberExpression)) throw new ArgumentException("Only properties can be mapped in this fashion");

            property = ((MemberExpression)expression.Body).Member as PropertyInfo;
        }

        public PropertyInfo Property
        {
            get { return property; }
        }
    }
}