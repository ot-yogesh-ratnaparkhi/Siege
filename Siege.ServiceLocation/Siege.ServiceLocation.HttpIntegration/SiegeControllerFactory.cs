using System;
using System.Web.Mvc;

namespace Siege.ServiceLocation.HttpIntegration
{
    public class SiegeControllerFactory : DefaultControllerFactory
    {
        private readonly IContextualServiceLocator locator;

        public SiegeControllerFactory(IContextualServiceLocator locator)
        {
            this.locator = locator;
        }
        
        protected override IController GetControllerInstance(Type controllerType)
        {
            if (controllerType == null) return null;

            return locator.GetInstance<ControllerBase>(controllerType);
        }
    }
}
