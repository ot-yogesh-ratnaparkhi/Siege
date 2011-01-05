namespace Siege.Courier.Web.ViewEngine
{
    public class DefaultMasterSelector : IMasterTemplateSelector
    {
        protected readonly string path;

        public DefaultMasterSelector(string path)
        {
            this.path = path;
        }

        public string Path
        {
            get { return path; }
        }

        public ConditionalMasterSelector<T> When<T>(System.Func<T, bool> condition)
        {
            return new ConditionalMasterSelector<T>(condition, path);
        }
    }
}