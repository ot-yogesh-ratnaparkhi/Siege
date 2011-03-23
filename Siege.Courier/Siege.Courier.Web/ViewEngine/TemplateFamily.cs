using System;
using System.Collections.Generic;
using System.Linq;
using Siege.Courier.Web.Conventions;

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

        public void Map(params ITemplateSelector[] selectors)
        {
            if (selectors == null || selectors.Length == 0) return;

            foreach (ITemplateSelector selector in selectors)
            {
                if (selector is IConditionalTemplateSelector)
                {
                    ((IConditionalTemplateSelector) selector).WithCriteria(this.templateCriteria);
                }
                this.selectors.Add(selector);
            }
        }

        public void UseConvention(IConvention convention)
        {
            Map(convention.GetSelectors().ToArray());
        }

        public string GetValidPath()
        {
            var orderedSelectors = selectors.Where(selector => !(selector is IMasterTemplateSelector)).OrderByDescending(x => x is IConditionalTemplateSelector);

            return orderedSelectors.Select(selector => selector.Path).FirstOrDefault(path => path != null);
        }

        public string GetDefaultPath()
        {
            var orderedSelectors = selectors.Where(selector => !(selector is IMasterTemplateSelector)).OrderByDescending(x => !(x is IConditionalTemplateSelector));

            return orderedSelectors.Select(selector => selector.Path).FirstOrDefault(path => path != null);
        }

        public string GetMaster()
        {
            var orderedSelectors = selectors.OfType<IMasterTemplateSelector>().OrderByDescending(x => x is IConditionalTemplateSelector);

            return orderedSelectors.Select(selector => selector.Path).FirstOrDefault(path => path != null);
        }
    }
}