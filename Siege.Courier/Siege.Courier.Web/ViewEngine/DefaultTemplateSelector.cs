using System;

namespace Siege.Courier.Web.ViewEngine
{
    public class DefaultTemplateSelector : ITemplateSelector
    {
        private readonly string path;

        public DefaultTemplateSelector(string path)
        {
            this.path = path;
        }

        public string Path
        {
            get { return path; }
        }

        public ConditionalTemplateSelector<T> When<T>(Func<T, bool> condition)
        {
            return new ConditionalTemplateSelector<T>(condition, this.path);
        }
    }
}