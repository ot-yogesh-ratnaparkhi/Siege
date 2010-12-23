using System.Collections.Generic;

namespace Siege.Courier.Web.ViewEngine
{
    public class TemplateFamily
    {
        IList<ITemplateSelector> selectors = new List<ITemplateSelector>();

        public ITemplateSelector Use(string path)
        {
            var selector = new TemplateSelector(path);
            selectors.Add(selector);

            return selector;
        }
    }
}