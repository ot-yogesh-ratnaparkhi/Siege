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
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Siege.Requisitions.InternalStorage;
using Siege.Requisitions.RegistrationSyntax;

namespace Siege.Requisitions.HttpIntegration
{
    public abstract class SiegeHttpApplication : HttpApplication
    {
        protected static IContextualServiceLocator locator;

        public IContextualServiceLocator ServiceLocator
        {
            get { return locator; }
        }

        protected abstract string GetApplicationName();
        protected abstract IServiceLocatorAdapter GetServiceLocatorAdapter();

        protected virtual IControllerFactory GetControllerFactory()
        {
            return new SiegeControllerFactory(locator);
        }

        public virtual void RegisterRoutes(RouteCollection routes)
        {
        }

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
                locator = new HttpSiegeContainer(GetServiceLocatorAdapter(), GetContextStore());
                locator.Register(Given<RouteCollection>.Then(RouteTable.Routes));

                RegisterRoutes(RouteTable.Routes);

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
            OnApplicationError(Server.GetLastError());
        }
    }
}