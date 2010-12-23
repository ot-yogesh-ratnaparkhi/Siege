using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;
using Siege.Requisitions.Web;

namespace Siege.Courier.Web.ViewEngine
{
    public class TemplateViewEngine : ServiceLocatorViewEngine
    {
        private readonly object lockObject = new object();
        private readonly Dictionary<string, TemplateFamily> templates = new Dictionary<string, TemplateFamily>();

        public TemplateFamily For<TController>(Expression<Action<TController>> expression)
        {
            var method = expression.Body as MemberExpression;

            if (!templates.ContainsKey(method.Member.Name))
            {
                lock (lockObject)
                {
                    if (!templates.ContainsKey(method.Member.Name))
                    {
                        templates.Add(method.Member.Name, new TemplateFamily());
                    }
                }
            }

            return templates[method.Member.Name];
        }

        public override ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            //var master = masterSelector().MasterName;
            //var template = selector().Path;

            //masterName = master ?? masterName;
            //viewName = template ?? viewName;

            return base.FindView(controllerContext, viewName, masterName, useCache);
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
                    engine.For<HomeController>(controller => controller.Index(null)).Use("~/Views/Home/Index");
                    engine.For<HomeController>(controller => controller.Index(null)).Use("~/Views/Dashboard/Index").When(() => session.GetActiveMember().Customer.PortalVersion == "ScoreSense");
                }));
            };
        }
    }*/
}
