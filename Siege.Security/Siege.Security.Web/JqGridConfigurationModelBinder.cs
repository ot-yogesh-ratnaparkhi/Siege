using System;
using System.Web.Mvc;

namespace Siege.Security.Web
{
    public class JqGridConfigurationModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var request = controllerContext.HttpContext.Request;
            return new JqGridConfiguration
            {

                PageIndex = int.Parse(request["page"] ?? "1"),
                PageSize = int.Parse(request["rows"] ?? "10"),
                SortColumn = request["sidx"] ?? String.Empty,
                SortOrder = request["sord"] ?? "asc",
            };
        }
    }
}
