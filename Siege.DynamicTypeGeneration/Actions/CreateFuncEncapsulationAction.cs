using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace Siege.DynamicTypeGeneration.Actions
{
    public class CreateFuncEncapsulationAction : ITypeGenerationAction
    {
        private ConstructorInfo constructor;
        private MethodInfo internalMethod;
        private TypeBuilder localBuilder;
        private IList<ITypeGenerationAction> actions;
        private MethodBuilderBundle methodBundle;
        private IList<FieldInfo> fields = new List<FieldInfo>();
		private IList<FieldInfo> publicFields = new List<FieldInfo>();
    	private FieldInfo dynamicType;
        public GeneratedMethod LocalGeneratedMethod { get { return this.localMethod; } }
        private GeneratedMethod localMethod;

        public MethodInfo Method { get { return this.internalMethod; } }
        public ConstructorInfo Constructor { get { return constructor; } }

        public CreateFuncEncapsulationAction(BuilderBundle bundle, MethodInfo method,
                                             IList<ITypeGenerationAction> actions, GeneratedMethod generatedMethod)
        {
            this.actions = actions;

            localBuilder = bundle.TypeBuilder.DefineNestedType("NestedTypeIn" + bundle.TypeBuilder.Name + "For" + method.Name,
                                                               TypeAttributes.Sealed |
                                                               TypeAttributes.NestedPrivate |
                                                               TypeAttributes.BeforeFieldInit);

            var localBundle = new BuilderBundle {TypeBuilder = localBuilder, ModuleBuilder = bundle.ModuleBuilder};
            
            var constructorAction = new AddDefaultConstructorAction(localBundle);
            this.constructor = constructorAction.Constructor;
            this.actions.Add(constructorAction);

            var thisAction = new AddFieldAction(localBundle, bundle.TypeBuilder.Name, bundle.TypeBuilder);
            this.fields.Add(thisAction.Field);
			this.dynamicType = thisAction.Field;

            this.actions.Add(thisAction);

            foreach (ParameterInfo info in method.GetParameters())
            {
                var fieldAction = new AddFieldAction(localBundle, info.Name, info.ParameterType);
                this.fields.Add(fieldAction.Field);
				this.PublicFields.Add(fieldAction.Field);

                this.actions.Add(fieldAction);
            }

            var action = new AddMethodAction(localBundle, "InternalMethod", method.ReturnType, new Type[0], false);
            
            this.internalMethod = action.MethodBuilder;

            this.methodBundle = new MethodBuilderBundle(localBundle) {MethodBuilder = action.MethodBuilder};
            this.localMethod = new GeneratedMethod(methodBundle, this.actions);

            generatedMethod.AddLocal(action.MethodBuilder); 
            generatedMethod.AddLocal(localBuilder);
            this.actions.Add(new FuncInternalsAction(methodBundle, fields, method));

            this.actions.Add(new CaptureCallResultAction(methodBundle, action.MethodBuilder, generatedMethod));
            localMethod.AddLocal(method); 
            localMethod.ReturnFrom(action.MethodBuilder);
            this.actions.Add(action);
        }

    	public FieldInfo DynamicType
    	{
    		get { return dynamicType; }
    	}

    	public IList<FieldInfo> PublicFields
    	{
    		get { return publicFields; }
    	}

    	public void Execute()
        {
            localBuilder.CreateType();
        }

        private class FuncInternalsAction : ITypeGenerationAction
        {
            private readonly MethodBuilderBundle bundle;
            private IList<FieldInfo> fields;
            private readonly MethodInfo method;

            public FuncInternalsAction(MethodBuilderBundle bundle, IList<FieldInfo> fields, MethodInfo method)
            {
                this.bundle = bundle;
                this.fields = fields;
                this.method = method;
            }

            public void Execute()
            {
                var methodGenerator = this.bundle.MethodBuilder.GetILGenerator();
                foreach (FieldInfo info in this.fields)
                {
                    methodGenerator.Emit(OpCodes.Ldarg_0);
                    methodGenerator.Emit(OpCodes.Ldfld, info);
                }

                methodGenerator.Emit(OpCodes.Call, method);
            }
        }
    }
}