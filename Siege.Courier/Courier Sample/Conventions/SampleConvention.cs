using System.Collections.Generic;
using Siege.Courier.Web.Conventions;
using Siege.Courier.Web.ViewEngine;

namespace Courier_Sample.Conventions
{
    public class SampleConvention : IConvention
    {
        public List<ITemplateSelector> GetSelectors()
        {
            return new List<ITemplateSelector>
            {
                To.Path("Home"),
                To.Path("LOL").When<bool>(x => x),
                To.Master("Site"),
                To.Master("LOL").When<bool>(x => x)
            };
        }
    }
}