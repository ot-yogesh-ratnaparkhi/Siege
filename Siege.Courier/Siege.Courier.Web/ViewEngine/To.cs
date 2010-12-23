namespace Siege.Courier.Web.ViewEngine
{
    public class To
    {
        public static DefaultTemplateSelector Path(string path)
        {
            var selector = new DefaultTemplateSelector(path);
         
            return selector;
        }
    }
}