using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
using Siege.DynamicTypeGeneration.Actions;

namespace Siege.DynamicTypeGeneration
{
    public class TypeGenerator
    {
        private readonly AssemblyBuilder assemblyBuilder;
        private IList<ITypeGenerationAction> actions = new List<ITypeGenerationAction>();
        private BuilderBundle bundle = new BuilderBundle();
        public TypeGenerator(string typeName, Type baseType)
        {
            var dllName = baseType.Namespace + "." + typeName;
            AssemblyName assemblyName = new AssemblyName { Name = dllName };
            AppDomain thisDomain = Thread.GetDomain();

            this.assemblyBuilder = thisDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.RunAndSave);
            this.bundle.ModuleBuilder = assemblyBuilder.DefineDynamicModule(assemblyBuilder.GetName().Name, assemblyBuilder.GetName().Name + ".dll");

            this.bundle.TypeBuilder = this.bundle.ModuleBuilder.DefineType(typeName,
                                                            TypeAttributes.Public |
                                                            TypeAttributes.Class |
                                                            TypeAttributes.AutoClass |
                                                            TypeAttributes.AnsiClass |
                                                            TypeAttributes.BeforeFieldInit |
                                                            TypeAttributes.AutoLayout,
                                                            baseType);


            GenerateDefaultConstructor();
        }

        private void GenerateDefaultConstructor()
        {
            this.actions.Add(new AddDefaultConstructorAction(this.bundle));
        }

        public GeneratedMethod CreateMethod(string methodName, Type returnType, Type[] parameterTypes, bool isOverride)
        {
            var action = new AddMethodAction(this.bundle, methodName, returnType, parameterTypes, isOverride);

            var methodBundle = new MethodBuilderBundle(bundle) {MethodBuilder = action.MethodBuilder};
            var method = new GeneratedMethod(methodBundle, this.actions);

            this.actions.Add(action);
            if (returnType != typeof(void)) method.AddLocal(action.MethodBuilder);

            return method;
        }

        public Type Create()
        {
            foreach(ITypeGenerationAction action in this.actions)
            {
                action.Execute();
            }

            Type type = this.bundle.TypeBuilder.CreateType();

            assemblyBuilder.Save(assemblyBuilder.GetName().Name + ".dll");

            return type;
        }

        public GeneratedField CreateField(FieldInfo field)
        {
            var action = new AddFieldAction(this.bundle, field);
            var generatedField = new GeneratedField(action);

            this.actions.Add(action);

            return generatedField;
        }
    }
}
