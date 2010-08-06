using System;
using System.Reflection;

namespace Siege.Foundry
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

            for (int counter = 1; counter <= target.GetParameters().Length; counter++)
            {
                method.LoadVariable(counter);
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
