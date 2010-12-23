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

        public TemplateViewEngine(Func<List<object>> templateCriteria)
        {
            this.templateCriteria = templateCriteria;
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

        public override ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            if (!templates.ContainsKey(controllerContext.RouteData.Values["controller"] + "." + viewName))
            {
                return base.FindView(controllerContext, viewName, masterName, useCache);
            }

            var path = templates[controllerContext.RouteData.Values["controller"] + "." + viewName].GetValidPath();
            
            return base.FindView(controllerContext, path, masterName, useCache);
        }
    }

    /*public class ViewEngineConvention : IConvention
    {
        private readonly HttpContextBase session;

        public ViewEngineConvention(HttpContextBase session)
        {
            this.session = session;
        }

        public Action<IServiceLocator> Build()
        {
            return serviceLocator =>
            {
                serviceLocator.Register(Given<TemplateViewEngine>.InitializeWith(engine =>
                {
                    engine.For<HomeController>(controller => controller.Index(null)).Map(To.Path("~/Views/Home/Index"));
                    engine.For<HomeController>(controller => controller.Index(null)).Map(To.Path("~/Views/Dashboard/Index").When(() => session.GetActiveMember().Customer.PortalVersion == "ScoreSense"));
                }));
            };
        }
    }*/
}
