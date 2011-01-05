namespace Siege.Courier.Web.ViewEngine
{
    public class To
    {
        public static DefaultTemplateSelector Path(string path)
        {
            var selector = new DefaultTemplateSelector(path);
         
            return selector;
        }

        public static DefaultMasterSelector Master(string masterPath)
        {
            var selector = new DefaultMasterSelector(masterPath);

            return selector;
        }
    }
}