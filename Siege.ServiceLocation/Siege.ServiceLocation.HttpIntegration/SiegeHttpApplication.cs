using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Siege.ServiceLocation.HttpIntegration
{
    public abstract class SiegeHttpApplication : HttpApplication
    {
        private static IContextualServiceLocator locator;

        public IContextualServiceLocator ServiceLocator
        {
            get
            {
                return locator;
            }
        }

        protected abstract IServiceLocatorAdapter GetServiceLocatorAdapter();

        protected virtual IControllerFactory GetControllerFactory()
        {
            return new SiegeControllerFactory(locator);
        }

        public virtual void RegisterRoutes(RouteCollection routes) { }

        protected virtual IContextStore GetContextStore()
        {
            return new HttpSessionStore();
        }

        protected virtual void OnApplicationStarted()
        {
        }

        protected virtual void OnApplicationStopped()
        {
        }

        protected virtual void OnApplicationError(Exception exception)
        {
        }

        public void Application_Start(object sender, EventArgs e)
        {
            lock (this)
            {
                locator = new SiegeContainer(GetServiceLocatorAdapter(), GetContextStore());
                locator
                    .Register(Given<RouteCollection>.Then(RouteTable.Routes))
                    .Register(Given<IContextStore>.Then(locator.ContextStore));


                RegisterRoutes(RouteTable.Routes);
                ViewEngines.Engines.Clear();
                ViewEngines.Engines.Add(new SiegeViewEngine());
                ViewEngines.Engines.Add(new WebFormViewEngine());

                ControllerBuilder.Current.SetControllerFactory(GetControllerFactory());

                OnApplicationStarted();
            }
        }

        public void Application_Stop(object sender, EventArgs e)
        {
            lock (this)
            {
                OnApplicationStopped();

                if (locator != null)
                {
                    locator.Dispose();
                    locator = null;
                }
            }
        }

        protected void Application_Error(object sender, EventArgs e)
        {
        }
    }
}
