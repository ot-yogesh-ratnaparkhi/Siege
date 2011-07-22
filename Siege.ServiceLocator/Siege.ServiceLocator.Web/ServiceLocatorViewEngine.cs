/*   Copyright 2009 - 2010 Marcus Bratton

     Licensed under the Apache License, Version 2.0 (the "License");
     you may not use this file except in compliance with the License.
     You may obtain a copy of the License at

     http://www.apache.org/licenses/LICENSE-2.0

     Unless required by applicable law or agreed to in writing, software
     distributed under the License is distributed on an "AS IS" BASIS,
     WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
     See the License for the specific language governing permissions and
     limitations under the License.
*/

using System;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace Siege.ServiceLocator.Web
{
    public class ServiceLocatorViewEngine : WebFormViewEngine
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

            if (result == null || result.View == null) result = base.FindView(controllerContext, viewName, masterName, useCache);

            return result;
        }

        protected ViewEngineResult FindExistingView(ControllerContext controllerContext, string controllerName, string viewName, string masterName, bool useCache)
        {
            if (viewName.StartsWith("~/") && VirtualPathProvider.FileExists(viewName))
                return base.FindView(controllerContext, viewName, masterName, useCache);

            if (viewName.StartsWith("~/") && VirtualPathProvider.FileExists(viewName + ".aspx"))
                return base.FindView(controllerContext, viewName + ".aspx", masterName, useCache);

            if (viewName.StartsWith("~/") && VirtualPathProvider.FileExists(viewName + ".ascx"))
                return base.FindView(controllerContext, viewName + ".ascx", masterName, useCache);

            if (VirtualPathProvider.FileExists("~/Views/" + controllerName + "/" + viewName))
                return base.FindView(controllerContext, "~/Views/" + controllerName + "/" + viewName, masterName, useCache);

            if (VirtualPathProvider.FileExists("~/Views/" + controllerName + "/" + viewName + ".aspx")) 
                return base.FindView(controllerContext, "~/Views/" + controllerName + "/" + viewName + ".aspx", masterName, useCache);

            if (VirtualPathProvider.FileExists("~/Views/" + controllerName + "/" + viewName + ".ascx"))
                return base.FindView(controllerContext, "~/Views/" + controllerName + "/" + viewName + ".ascx", masterName, useCache);

            if (VirtualPathProvider.FileExists("~/Views/Shared/" + controllerName + "/" + viewName))
                return base.FindView(controllerContext, "~/Views/Shared/" + controllerName + "/" + viewName, masterName, useCache);

            if (VirtualPathProvider.FileExists("~/Views/Shared/" + controllerName + "/" + viewName + ".aspx"))
                return base.FindView(controllerContext, "~/Views/Shared/" + controllerName + "/" + viewName + ".aspx", masterName, useCache);

            if (VirtualPathProvider.FileExists("~/Views/Shared/" + controllerName + "/" + viewName + ".ascx"))
                return base.FindView(controllerContext, "~/Views/Shared/" + controllerName + "/" + viewName + ".ascx", masterName, useCache);
            
            return null;
        }

        public override ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache)
        {
            if (partialViewName.StartsWith("~/") && VirtualPathProvider.FileExists(partialViewName))
                return base.FindPartialView(controllerContext, partialViewName, useCache);

            if (partialViewName.StartsWith("~/") && VirtualPathProvider.FileExists(partialViewName + ".aspx"))
                return base.FindPartialView(controllerContext, partialViewName + ".aspx", useCache);

            if (partialViewName.StartsWith("~/") && VirtualPathProvider.FileExists(partialViewName + ".ascx"))
                return base.FindPartialView(controllerContext, partialViewName + ".ascx", useCache);

            if (VirtualPathProvider.FileExists("~/Views/" + partialViewName))
                return base.FindPartialView(controllerContext, "~/Views/" + partialViewName, useCache);

            if (VirtualPathProvider.FileExists("~/Views/" + partialViewName + ".aspx"))
                return base.FindPartialView(controllerContext, "~/Views/" + partialViewName + ".aspx", useCache);

            if (VirtualPathProvider.FileExists("~/Views/" + partialViewName + ".ascx"))
                return base.FindPartialView(controllerContext, "~/Views/" + partialViewName + ".ascx", useCache);

            if (VirtualPathProvider.FileExists("~/Views/Shared/" + partialViewName))
                return base.FindPartialView(controllerContext, "~/Views/Shared/" + partialViewName, useCache);

            if (VirtualPathProvider.FileExists("~/Views/Shared/" + partialViewName + ".aspx"))
                return base.FindPartialView(controllerContext, "~/Views/Shared/" + partialViewName + ".aspx", useCache);

            if (VirtualPathProvider.FileExists("~/Views/Shared/" + partialViewName + ".ascx"))
                return base.FindPartialView(controllerContext, "~/Views/Shared/" + partialViewName + ".ascx", useCache);

            return null;
        }
    }
}
