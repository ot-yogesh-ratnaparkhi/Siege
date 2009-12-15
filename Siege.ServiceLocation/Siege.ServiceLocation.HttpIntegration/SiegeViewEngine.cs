using System;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace Siege.ServiceLocation.HttpIntegration
{
    public class SiegeViewEngine : WebFormViewEngine
    {
        public override ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            ViewEngineResult result = null;

            if (controllerContext.Controller != null)
            {
                string name = null;
                if (Regex.IsMatch(controllerContext.Controller.GetType().Name, "(.*)[Cc]ontroller.*"))
                {
                    name = Regex.Replace(controllerContext.Controller.GetType().Name, "(.*)[Cc]ontroller.*", "$1");
                }
                
                if (!String.IsNullOrEmpty(name))
                {
                    result = FindExistingView(controllerContext, name, viewName, masterName, useCache);
                }
            }

            if(result == null || result.View == null) result = base.FindView(controllerContext, viewName, masterName, useCache);

            return result;
        }

        private ViewEngineResult FindExistingView(ControllerContext controllerContext, string controllerName, string viewName, string masterName, bool useCache)
        {
            if (FileExists(controllerContext, "~/Views/" + controllerName + "/" + viewName + ".aspx")) 
                return base.FindView(controllerContext, "~/Views/" + controllerName + "/" + viewName + ".aspx", masterName, useCache);

            if (FileExists(controllerContext, "~/Views/" + controllerName + "/" + viewName + ".ascx"))
                return base.FindView(controllerContext, "~/Views/" + controllerName + "/" + viewName + ".ascx", masterName, useCache);

            if (FileExists(controllerContext, "~/Views/Shared/" + controllerName + "/" + viewName + ".aspx"))
                return base.FindView(controllerContext, "~/Views/Shared/" + controllerName + "/" + viewName + ".aspx", masterName, useCache);

            if (FileExists(controllerContext, "~/Views/Shared/" + controllerName + "/" + viewName + ".ascx"))
                return base.FindView(controllerContext, "~/Views/Shared/" + controllerName + "/" + viewName + ".ascx", masterName, useCache);
            
            return null;
        }

        public override ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache)
        {
            ViewEngineResult result = null;

            if (controllerContext.Controller != null)
            {
                string name = null;
                
                if (Regex.IsMatch(controllerContext.Controller.GetType().Name, "(.*)[Cc]ontroller.*"))
                {
                    name = Regex.Replace(controllerContext.Controller.GetType().Name, "(.*)[Cc]ontroller.*", "$1");
                }

                if (!String.IsNullOrEmpty(name))
                {
                    result = base.FindPartialView(controllerContext, name + "/" + partialViewName, useCache);
                }
            }

            if (result == null) result = base.FindPartialView(controllerContext, partialViewName, useCache);

            return result;
        }
    }
}
