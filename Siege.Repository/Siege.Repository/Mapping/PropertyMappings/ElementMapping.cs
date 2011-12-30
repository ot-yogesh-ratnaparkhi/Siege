using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Siege.Repository.Mapping.PropertyMappings
{
    public class ElementMapping<TClass, TType> : ElementMapping
    {
        public ElementMapping(Expression<Func<TClass, TType>> expression) : base(((MemberExpression)expression.Body).Member as PropertyInfo)
        {
            if (!(expression.Body is MemberExpression)) throw new ArgumentException("Only properties can be mapped in this fashion");
        }
    }

    public class ElementMapping : IElementMapping
    {
        public ElementMapping(PropertyInfo property)
        {
            this.Property = property;
        }

        public PropertyInfo Property { get; protected set; }
        
        public virtual object GetValue(object item)
        {
            return this.Property.GetValue(item, new object[0]);
        }
    }
}