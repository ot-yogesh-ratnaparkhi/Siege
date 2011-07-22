using System.Collections.Generic;
using Siege.Eventing.Web.ViewEngine;

namespace Siege.Eventing.Web.Conventions
{
    public interface IConvention
    {
        List<ITemplateSelector> GetSelectors();
    }
}