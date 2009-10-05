using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace Siege.DynamicTypeGeneration.Actions
{
    public class CallAction : ITypeGenerationAction
    {
        protected readonly MethodBuilderBundle bundle;
        protected readonly MethodInfo method;
        protected IList<ITypeGenerationAction> actions;
        protected readonly GeneratedMethod generatedMethod;
        protected FieldInfo target;
        protected MethodInfo parametersFrom;

        public CallAction(MethodBuilderBundle bundle, MethodInfo method, IList<ITypeGenerationAction> actions,
                          GeneratedMethod generatedMethod)
        {
            this.bundle = bundle;
            this.method = method;
            this.actions = actions;
            this.generatedMethod = generatedMethod;
        }

        public void On(FieldInfo field)
        {
            target = field;
        }

        public virtual void Execute()
        {
            var methodGenerator = this.bundle.MethodBuilder.GetILGenerator();

            if (target != null)
            {
                methodGenerator.Emit(OpCodes.Ldarg_0);
                methodGenerator.Emit(OpCodes.Ldfld, target);
            }

            if (parametersFrom != null)
            {
                var parameters = parametersFrom.GetParameters();

                for (int i = 0; i <= parameters.Length; i++)
                {
                    methodGenerator.Emit(OpCodes.Ldarg, i);
                }
            }
            methodGenerator.Emit(OpCodes.Call, method);
        }

        public CallAction WithParametersFrom(MethodInfo info)
        {
            parametersFrom = info;

            return this;
        }

        public CallAction CaptureResult()
        {
            this.actions.Add(new CaptureCallResultAction(this.bundle, this.method, this.generatedMethod));

            return this;
        }
    }
}