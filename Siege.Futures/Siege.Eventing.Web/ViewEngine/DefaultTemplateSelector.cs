using System;

namespace Siege.Eventing.Web.ViewEngine
{
    public class DefaultTemplateSelector : ITemplateSelector
    {
        protected readonly string path;

        public DefaultTemplateSelector(string path)
        {
            this.path = path;
        }

        public string Path
        {
            get { return path; }
        }

        public IConditionalTemplateSelector When<T>(Func<T, bool> condition)
        {
            return new ConditionalTemplateSelector<T>(condition, this.path);
        }
    }
}