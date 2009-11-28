using System.Collections;
using System.Collections.Generic;
using Ninject.Activation;
using Ninject.Parameters;

namespace Siege.Container.NinjectAdapter
{
    public class ParameterAdapter
    {
        public ParameterAdapter(IContext context)
        {
            Dictionary = new Dictionary<string, object>();
            if (context.Parameters == null) return;
            foreach (IParameter parameter in context.Parameters)
            {
                var value = parameter.GetValue(context);
                if (value == null) continue;
                Dictionary.Add(parameter.Name, value);
            }
        }

        public IDictionary Dictionary { get; set; }
    }
}