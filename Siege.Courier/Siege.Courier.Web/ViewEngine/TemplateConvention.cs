using System;
using Siege.Requisitions;
using Siege.Requisitions.Extensions.Conventions;

namespace Siege.Courier.Web.ViewEngine
{
    public class TemplateConvention : IConvention
    {
        public Action<IServiceLocator> Build()
        {
            return locator =>
            {
                locator.Register(Requisitions.Extensions.ExtendedRegistrationSyntax.Given<TemplateViewEngine>.Then<TemplateViewEngine>());
            };
        }
    }
}