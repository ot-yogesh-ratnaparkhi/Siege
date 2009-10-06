using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Siege.DynamicTypeGeneration.Actions;

namespace Siege.DynamicTypeGeneration
{
    public class GeneratedMethod
    {
        private readonly MethodBuilderBundle bundle;
        private IList<ITypeGenerationAction> actions;
        private Hashtable locals = new Hashtable();
        internal int LocalCount { get; set; }

        public Hashtable Locals
        {
            get { return locals; }
        }


        public GeneratedMethod(MethodBuilderBundle bundle, IList<ITypeGenerationAction> actions)
        {
            this.bundle = bundle;
            this.actions = actions;
        }

        public void Instantiate(Type type, Type[] constructorArguments)
        {
            this.actions.Add(new InstantiationAction(bundle, type, constructorArguments));
        }

        public CallAction Call(MethodInfo method)
        {
            var action = new CallAction(bundle, method, actions, this);
            this.actions.Add(action);

            return action;
        }

        public CompletedMethod ReturnFrom(MethodInfo method)
        {
            this.actions.Add(new ReturnAction(this.bundle, method, this));

            return new CompletedMethod(this.bundle, this.actions);
        }

        public CallBaseAction CallBase(MethodInfo method, Type baseType)
        {
            var action = new CallBaseAction(bundle, method, actions, baseType, this);
            this.actions.Add(action);

            return action;
        }

        public CreateFuncAction UsingFunc(Type type)
        {
            var action = new CreateFuncAction(this.bundle, type, actions, this);
            return action;
        }

        public FieldAssignmentAction Assign(FieldInfo value)
        {
            var action = new FieldAssignmentAction(bundle, value);
            this.actions.Add(action);

            return action;
        }

        internal void AddLocal(Type item)
        {
            this.LocalCount++;
            var local = new AddLocalAction(bundle, item);

            this.Locals.Add(item, this.LocalCount);
            this.actions.Add(local);
        }

        internal void AddLocal(MethodInfo item)
        {
            this.LocalCount++;
            var local = new AddLocalAction(bundle, item.ReturnType);

            this.Locals.Add(item, this.LocalCount);
            this.actions.Add(local);
        }
    }
}