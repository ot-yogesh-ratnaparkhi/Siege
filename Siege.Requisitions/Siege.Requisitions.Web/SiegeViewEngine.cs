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

namespace Siege.Requisitions.Web
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
