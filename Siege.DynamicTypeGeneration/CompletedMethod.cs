using System.Collections.Generic;
using System.Reflection;
using Siege.DynamicTypeGeneration.Actions;

namespace Siege.DynamicTypeGeneration
{
    public class CompletedMethod
    {
        private readonly MethodBuilderBundle bundle;
        private IList<ITypeGenerationAction> actions;

        public MethodInfo Method
        {
            get { return bundle.MethodBuilder; }
        }

        public CompletedMethod(MethodBuilderBundle bundle, IList<ITypeGenerationAction> actions)
        {
            this.bundle = bundle;
            this.actions = actions;
        }

        public void Override(MethodInfo method)
        {
            this.actions.Add(new OverrideMethodAction(bundle, method));
        }
    }
}