using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Siege.Requisitions.Web;

namespace Siege.Courier.Web.ViewEngine
{
    public class TemplateViewEngine : ServiceLocatorViewEngine
    {
        private readonly Func<List<object>> templateCriteria;
        private readonly object lockObject = new object();
        private readonly Dictionary<string, TemplateFamily> templates = new Dictionary<string, TemplateFamily>();
        private readonly TemplateFamily defaultMasterFamily;

        public TemplateViewEngine(Func<List<object>> templateCriteria)
        {
            this.templateCriteria = templateCriteria;
            this.defaultMasterFamily = new TemplateFamily(templateCriteria);
        }

        public void Map(params IMasterTemplateSelector[] selectors)
        {
            if (selectors == null || selectors.Length == 0) return;

            foreach (IMasterTemplateSelector selector in selectors)
            {
                if (selector is IConditionalTemplateSelector)
                {
                    ((IConditionalTemplateSelector)selector).WithCriteria(this.templateCriteria);
                }
                this.defaultMasterFamily.Map(selector);
            }
        }

        public TemplateFamily For<TController>(Expression<Action<TController>> expression)
        {
            var method = expression.Body as MethodCallExpression;
            string name = null;
            if (Regex.IsMatch(method.Method.DeclaringType.Name, "(.*)[Cc]ontroller.*"))
            {
                name = Regex.Replace(method.Method.DeclaringType.Name, "(.*)[Cc]ontroller.*", "$1");
            }

            if (!templates.ContainsKey(name + "." + method.Method.Name))
            {
                lock (lockObject)
                {
                    if (!templates.ContainsKey(name + "." + method.Method.Name))
                    {
                        templates.Add(name + "." + method.Method.Name, new TemplateFamily(this.templateCriteria));
                    }
                }
            }

            return templates[name + "." + method.Method.Name];
        }

        public TemplateFamily ForPartial(string name)
        {
            if (!templates.ContainsKey("Partial." + name))
            {
                lock (lockObject)
                {
                    if (!templates.ContainsKey("Partial." + name))
                    {
                        templates.Add("Partial." + name, new TemplateFamily(this.templateCriteria));
                    }
                }
            }

            return templates["Partial." + name];
        }

        public override ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            if (!templates.ContainsKey(controllerContext.RouteData.Values["controller"] + "." + viewName))
            {
                return base.FindView(controllerContext, viewName, masterName, useCache);
            }
            var template = templates[controllerContext.RouteData.Values["controller"] + "." + viewName];
            var path = template.GetValidPath();
            var master = template.GetMaster() ?? this.defaultMasterFamily.GetMaster() ?? masterName;
            
            return base.FindView(controllerContext, path, master, useCache);
        }

        public override ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache)
        {
            if (!templates.ContainsKey("Partial." + partialViewName))
            {
                return base.FindPartialView(controllerContext, partialViewName, useCache);
            }

            var template = templates["Partial." + partialViewName];
            var path = template.GetValidPath();

            return base.FindPartialView(controllerContext, path, useCache);
        }
    }
}
