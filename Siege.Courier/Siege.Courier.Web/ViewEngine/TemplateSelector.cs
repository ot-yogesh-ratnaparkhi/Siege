using System;

namespace Siege.Courier.Web.ViewEngine
{
    public class TemplateSelector : ITemplateSelector
    {
        private readonly string path;
        private Func<bool> condition;

        public TemplateSelector(string path)
        {
            this.path = path;
        }

        public string Path
        {
            get { return path; }
        }

        public void When(Func<bool> condition)
        {
            this.condition = condition;
        }

        public bool IsValid()
        {
            return condition();
        }
    }
}