using System;
using System.Reflection;

namespace Siege.DynamicTypeGeneration
{
    public class GeneratedDelegate
    {
        private readonly MethodBodyContext context;
        private readonly GeneratedMethod method;
        private readonly DelegateGenerator generator;

        public GeneratedDelegate(MethodBodyContext context, GeneratedMethod method, DelegateGenerator generator)
        {
            this.context = context;
            this.method = method;
            this.generator = generator;
        }

        public GeneratedVariable CreateFunc(MethodInfo target)
        {
            method.TargettingSelf();

            int counter = 1;

            foreach (ParameterInfo info in target.GetParameters())
            {
                method.LoadVariable(counter);
                counter++;
            }

            if (!context.TypeGenerationContext.TypesToComplete.Contains(generator.NestedType.Builder))
            {
                context.TypeGenerationContext.TypesToComplete.Add(generator.NestedType.Builder);
            }
            var variable = context.CreateVariable(generator.NestedType.Builder);
            variable.AssignFrom(context.Instantiate(generator.Constructor));

            Type delegateType = method.CreateDelegate(variable, generator.NestedType.EntryPoint, target.ReturnType);
            var returnValue = context.CreateVariable(delegateType);

            returnValue.AssignFrom(context.Instantiate(delegateType, new[] { typeof(object), typeof(IntPtr) }));

            return returnValue;
        }
    }
}
