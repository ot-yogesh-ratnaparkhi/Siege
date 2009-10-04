using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using Siege.DynamicTypeGeneration.Actions;

namespace Siege.DynamicTypeGeneration
{
    public class GeneratedMethod
    {
        private readonly TypeBuilder typeBuilder;
        private readonly MethodBuilder builder;
        private readonly IList<ITypeGenerationAction> actions;

        public GeneratedMethod(TypeBuilder typeBuilder, MethodBuilder builder, IList<ITypeGenerationAction> actions)
        {
            this.typeBuilder = typeBuilder;
            this.builder = builder;
            this.actions = actions;
        }

        public void Instantiate(Type type, Type[] constructorArguments)
        {
            this.actions.Add(new InstantiationAction(builder, type, constructorArguments));
        }

        public CallAction Call(MethodInfo method)
        {
            var action = new CallAction(builder, method, actions);
            this.actions.Add(action);

            return action;
        }

        public CompletedMethod ReturnFrom(MethodInfo method)
        {
            this.actions.Add(new ReturnAction(this.builder, method));

            return new CompletedMethod(this.typeBuilder, this.builder, this.actions);
        }

        public CallBaseAction CallBase(MethodInfo method, Type baseType)
        {
            var action = new CallBaseAction(builder, method, actions, baseType);
            this.actions.Add(action);

            return action;
        }

        public CreateFuncAction UsingFunc(Type type)
        {
            var action = new CreateFuncAction(builder, type, actions);

            return action;
        }

        public FieldAssignmentAction Assign(FieldInfo value)
        {
            var action = new FieldAssignmentAction(builder, value);
            this.actions.Add(action);

            return action;
        }
    }
}
