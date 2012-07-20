using System;
using Siege.ServiceLocator;
using Siege.ServiceLocator.Extensions.Conventions;

namespace Siege.Eventing.Web.ViewEngine
{
    public class TemplateConvention : IConvention
    {
        public Action<IServiceLocator> Build()
        {
            return locator =>
            {
                locator.Register(ServiceLocator.Extensions.ExtendedRegistrationSyntax.Given<TemplateViewEngine>.Then<TemplateViewEngine>());
            };
        }
    }
}