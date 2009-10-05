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
        private IList<ITypeGenerationAction> actions;
        internal int LocalCount { get; set; }


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
            var action = new CallAction(builder, method, actions, this);
            this.actions.Add(action);

            return action;
        }

        public CompletedMethod ReturnFrom(MethodInfo method)
        {
            this.actions.Add(new ReturnAction(this.builder, method, this));

            return new CompletedMethod(this.typeBuilder, this.builder, this.actions);
        }

        public CallBaseAction CallBase(MethodInfo method, Type baseType)
        {
            var action = new CallBaseAction(builder, method, actions, baseType, this);
            this.actions.Add(action);

            return action;
        }

        public CreateFuncAction UsingFunc(Type type)
        {
            var action = new CreateFuncAction(builder, type, actions, this);

            return action;
        }

        public FieldAssignmentAction Assign(FieldInfo value)
        {
            var action = new FieldAssignmentAction(builder, value);
            this.actions.Add(action);

            return action;
        }
        
        internal void AddLocal(Type localType)
        {
            this.actions.Add(new AddLocalAction(builder, localType));
            this.LocalCount++;
        }
    }
}
