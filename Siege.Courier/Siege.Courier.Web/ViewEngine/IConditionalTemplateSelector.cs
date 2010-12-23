using System;
using System.Collections.Generic;

namespace Siege.Courier.Web.ViewEngine
{
    public interface IConditionalTemplateSelector
    {
        void WithCriteria(Func<List<object>> criteria);
    }
}