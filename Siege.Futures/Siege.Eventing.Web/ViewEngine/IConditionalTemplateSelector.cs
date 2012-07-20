using System;
using System.Collections.Generic;

namespace Siege.Eventing.Web.ViewEngine
{
    public interface IConditionalTemplateSelector : ITemplateSelector
    {
        void WithCriteria(Func<List<object>> criteria);
    }
}