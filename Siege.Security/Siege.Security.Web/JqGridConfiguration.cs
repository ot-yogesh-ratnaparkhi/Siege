namespace Siege.Security.Web
{
    public class JqGridConfiguration
    {
        public JqGridConfiguration()
        {
            PageIndex = 1;
            TotalPages = 1;
            TotalRows = 10;
            PageSize = 10;
            SortColumn = string.Empty;
            SortOrder = "asc";
        }

        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalRows { get; set; }
        public string SortColumn { get; set; }
        public string SortOrder { get; set; }
    }
}