using System;
using System.Collections.Generic;

namespace Siege.Eventing.Web.ViewEngine
{
    public class ConditionalTemplateSelector<T> : IConditionalTemplateSelector
    {
        private readonly Func<T, bool> condition;
        private readonly string path;
        private Func<List<object>> criteria;

        public ConditionalTemplateSelector(Func<T, bool> condition, string path)
        {
            this.condition = condition;
            this.path = path;
        }

        public void WithCriteria(Func<List<object>> criteria)
        {
            this.criteria = criteria;
        }

        public string Path
        {
            get
            {
                foreach(object item in criteria())
                {
                    if (!(item is T)) continue;
                    if (condition((T)item)) return this.path;
                }

                return null;
            }
        }
    }
}