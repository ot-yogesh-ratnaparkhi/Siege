using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace Siege.DynamicTypeGeneration.Actions
{
    public class CallBaseAction : CallAction
    {
        private readonly Type baseType;

        public CallBaseAction(MethodBuilder builder, MethodInfo method, IList<ITypeGenerationAction> actions, Type baseType) : base(builder, method, actions)
        {
            this.baseType = baseType;
        }

        public override void Execute()
        {
            var methodGenerator = builder.GetILGenerator();

            if (target != null)
            {
                methodGenerator.Emit(OpCodes.Ldarg_0);
                methodGenerator.Emit(OpCodes.Ldfld, target);
            }
            else
            {
                methodGenerator.Emit(OpCodes.Ldarg_0);
            }

            List<Type> parameters = new List<Type>();
            var methodParameters = method.GetParameters();
            
            for (int i = 0; i < methodParameters.Length; i++)
            {
                parameters.Add(methodParameters[i].ParameterType);
                methodGenerator.Emit(OpCodes.Ldarg, i+1);
            }

            methodGenerator.Emit(OpCodes.Call, baseType.GetMethod(method.Name, parameters.ToArray()));
            
            if (method.ReturnType != typeof(void))
            {
                methodGenerator.DeclareLocal(method.ReturnType);
            }
            else
            {
                methodGenerator.Emit(OpCodes.Nop);
            }
        }
    }
}
