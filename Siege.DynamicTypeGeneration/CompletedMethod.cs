using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using Siege.DynamicTypeGeneration.Actions;

namespace Siege.DynamicTypeGeneration
{
    public class CompletedMethod
    {
        private readonly TypeBuilder builder;
        private readonly MethodBuilder methodBuilder;
        private readonly IList<ITypeGenerationAction> actions;
        public MethodInfo Method { get { return methodBuilder; } }

        public CompletedMethod(TypeBuilder builder, MethodBuilder methodBuilder, IList<ITypeGenerationAction> actions)
        {
            this.builder = builder;
            this.methodBuilder = methodBuilder;
            this.actions = actions;
        }

        public void Override(MethodInfo method)
        {
            this.actions.Add(new OverrideMethodAction(builder, methodBuilder, method));
        }
    }
}
