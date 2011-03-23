using System.Collections.Generic;
using Siege.Courier.Web.ViewEngine;

namespace Siege.Courier.Web.Conventions
{
    public interface IConvention
    {
        List<ITemplateSelector> GetSelectors();
    }
}