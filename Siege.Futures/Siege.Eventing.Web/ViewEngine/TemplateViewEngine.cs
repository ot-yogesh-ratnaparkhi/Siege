using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Siege.Eventing.Web.Conventions;
using Siege.ServiceLocator.Web;

namespace Siege.Eventing.Web.ViewEngine
{
    public class TemplateViewEngine : ServiceLocatorViewEngine
    {
        private readonly Func<List<object>> templateCriteria;
        private readonly object lockObject = new object();
        private readonly Dictionary<string, TemplateFamily> templates = new Dictionary<string, TemplateFamily>();
        private readonly TemplateFamily defaultFamily;

        public TemplateViewEngine(Func<List<object>> templateCriteria)
        {
            this.templateCriteria = templateCriteria;
            this.defaultFamily = new TemplateFamily(templateCriteria);
        }

        public void UseConvention(IConvention convention)
        {
            Map(convention.GetSelectors().ToArray());
        }

        public void Map(params ITemplateSelector[] selectors)
        {
            if (selectors == null || selectors.Length == 0) return;

            foreach (ITemplateSelector selector in selectors)
            {
                if (selector is IConditionalTemplateSelector)
                {
                    ((IConditionalTemplateSelector)selector).WithCriteria(this.templateCriteria);
                }

                this.defaultFamily.Map(selector);
            }
        }

        public TemplateFamily For<TController>(Expression<Action<TController>> expression)
        {
            var method = (MethodCallExpression)expression.Body;
            string name = null;

            if (Regex.IsMatch(method.Method.DeclaringType.Name, "(.*)[Cc]ontroller.*"))
            {
                name = Regex.Replace(method.Method.DeclaringType.Name, "(.*)[Cc]ontroller.*", "$1");
            }

            if (!templates.ContainsKey((name + "." + method.Method.Name).ToLower()))
            {
                lock (lockObject)
                {
                    if (!templates.ContainsKey((name + "." + method.Method.Name).ToLower()))
                    {
                        templates.Add((name + "." + method.Method.Name).ToLower(), new TemplateFamily(this.templateCriteria));
                    }
                }
            }

            return templates[(name + "." + method.Method.Name).ToLower()];
        }

        public TemplateFamily ForPartial(string name)
        {
            if (!templates.ContainsKey(("Partial." + name).ToLower()))
            {
                lock (lockObject)
                {
                    if (!templates.ContainsKey(("Partial." + name).ToLower()))
                    {
                        templates.Add(("Partial." + name).ToLower(), new TemplateFamily(this.templateCriteria));
                    }
                }
            }

            return templates[("Partial." + name).ToLower()];
        }

        public override ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            var conditionalPath = this.defaultFamily.GetValidPath();
            var defaultPath = this.defaultFamily.GetDefaultPath();

            if (!templates.ContainsKey((controllerContext.RouteData.Values["controller"] + "." + viewName).ToLower()))
            {
                var masterPage = FindMaster(BuildPath(this.defaultFamily.GetMaster(), conditionalPath, defaultPath)) ??
                                 FindMaster(BuildPath(this.defaultFamily.GetMaster(), defaultPath, conditionalPath)) ??
                                 FindMaster(BuildPath(masterName, conditionalPath, defaultPath)) ??
                                 FindMaster(BuildPath(masterName, defaultPath, conditionalPath)) ??
                                 masterName;

                var defaultView = FindExistingView(controllerContext, "", BuildPath(viewName, defaultPath, conditionalPath), masterPage, useCache);
                if (defaultView == null) defaultView = FindExistingView(controllerContext, "", BuildPath(viewName, conditionalPath, defaultPath), masterPage, useCache);

                return defaultView ?? base.FindView(controllerContext, viewName, masterPage, useCache);
            }

            var template = templates[(controllerContext.RouteData.Values["controller"] + "." + viewName).ToLower()];
            var templatePath = template.GetValidPath();

            string path = BuildPath(viewName, templatePath, conditionalPath);

            if (FindExistingView(controllerContext, "", path, masterName, useCache) == null)
            {
                path = BuildPath(viewName, template.GetDefaultPath(), defaultPath);
            }

            var master = FindMaster(BuildPath(template.GetMaster(), conditionalPath, defaultPath)) ??
                                 FindMaster(BuildPath(template.GetMaster(), defaultPath, conditionalPath)) ??
                                 FindMaster(BuildPath(this.defaultFamily.GetMaster(), conditionalPath, defaultPath)) ??
                                 FindMaster(BuildPath(this.defaultFamily.GetMaster(), defaultPath, conditionalPath)) ??
                                 masterName;

            var result = FindExistingView(controllerContext, "", path, master, useCache);
            
            return result ?? base.FindView(controllerContext, viewName, master, useCache);
        }

        private static string BuildPath(string viewName, string templatePath, string defaultPath)
        {
            return templatePath.StartsWith("~/") 
                       ? (templatePath.EndsWith("/") ? templatePath + viewName : templatePath + "/" + viewName)
                       : (defaultPath == null ? viewName : defaultPath.EndsWith("/") ? defaultPath + viewName : defaultPath + "/" + viewName);
        }

        protected string FindMaster(string masterName)
        {
            if (masterName.StartsWith("~/") && VirtualPathProvider.FileExists(masterName))
                return masterName;

            if (masterName.StartsWith("~/") && VirtualPathProvider.FileExists(masterName + ".master"))
                return masterName + ".master";

            if (VirtualPathProvider.FileExists("~/Views/" + masterName))
                return "~/Views/" + masterName;

            if (VirtualPathProvider.FileExists("~/Views/" + masterName + ".master"))
                return "~/Views/" + masterName + ".master";

            if (VirtualPathProvider.FileExists("~/Views/Shared/" + masterName))
                return "~/Views/Shared" + masterName;

            if (VirtualPathProvider.FileExists("~/Views/Shared/" + masterName + ".master"))
                return "~/Views/Shared" + masterName + ".master";

            return null;
        }

        public override ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache)
        {
            var conditionalPath = this.defaultFamily.GetValidPath();
            var defaultPath = this.defaultFamily.GetDefaultPath();

            if (!templates.ContainsKey(("Partial." + partialViewName).ToLower()))
            {
                var defaultView = base.FindPartialView(controllerContext, BuildPath(partialViewName, defaultPath, conditionalPath), useCache);
                if(defaultView == null) defaultView = base.FindPartialView(controllerContext, BuildPath(partialViewName, conditionalPath, defaultPath), useCache);

                return defaultView ?? base.FindPartialView(controllerContext, partialViewName, useCache);
            }

            var template = templates[("Partial." + partialViewName).ToLower()];
            string path = BuildPath(partialViewName, template.GetValidPath(), conditionalPath);

            if (FindPartialView(controllerContext, path, useCache) == null)
            {
                path = BuildPath(partialViewName, template.GetDefaultPath(), defaultPath);
            }

            var result = FindPartialView(controllerContext, path, useCache);

            return result ?? base.FindPartialView(controllerContext, partialViewName, useCache);
        }
    }
}
