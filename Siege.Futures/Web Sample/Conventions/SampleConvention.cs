using System.Collections.Generic;
using Siege.Eventing.Web.Conventions;
using Siege.Eventing.Web.ViewEngine;

namespace Web.Sample.Conventions
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