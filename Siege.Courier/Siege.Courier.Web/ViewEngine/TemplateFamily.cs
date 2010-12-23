using System;
using System.Collections.Generic;
using System.Linq;

namespace Siege.Courier.Web.ViewEngine
{
    public class TemplateFamily
    {
        private readonly Func<List<object>> templateCriteria;
        private readonly IList<ITemplateSelector> selectors = new List<ITemplateSelector>();

        public TemplateFamily(Func<List<object>> templateCriteria)
        {
            this.templateCriteria = templateCriteria;
        }

        public void Map(ITemplateSelector selector)
        {
            if(selector is IConditionalTemplateSelector)
            {
                ((IConditionalTemplateSelector)selector).WithCriteria(this.templateCriteria);
            }
            this.selectors.Add(selector);
        }

        public string GetValidPath()
        {
            var orderedSelectors = selectors.OrderBy(x => x is IConditionalTemplateSelector);

            foreach(ITemplateSelector selector in orderedSelectors)
            {
                var path = selector.Path;

                if (path != null) return path;
            }

            return null;
        }
    }
}