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
        private readonly TypeBuilder typeBuilder;
        private readonly AssemblyBuilder assemblyBuilder;
        private IList<ITypeGenerationAction> actions = new List<ITypeGenerationAction>();

        public TypeGenerator(string typeName, Type baseType)
        {
            var dllName = baseType.Namespace + "." + typeName;
            AssemblyName assemblyName = new AssemblyName { Name = dllName };
            AppDomain thisDomain = Thread.GetDomain();

            this.assemblyBuilder = thisDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.RunAndSave);
            var moduleBuilder = assemblyBuilder.DefineDynamicModule(assemblyBuilder.GetName().Name, assemblyBuilder.GetName().Name + ".dll");

            this.typeBuilder = moduleBuilder.DefineType(typeName,
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
            this.actions.Add(new AddDefaultConstructorAction(this.typeBuilder));
        }

        public GeneratedMethod CreateMethod(string methodName, Type returnType, Type[] parameterTypes)
        {
            var action = new AddMethodAction(this.typeBuilder, methodName, returnType, parameterTypes);
            var method = new GeneratedMethod(this.typeBuilder, action.MethodBuilder, this.actions);

            this.actions.Add(action);

            return method;
        }

        public Type Create()
        {
            foreach(ITypeGenerationAction action in this.actions)
            {
                action.Execute();
            }

            Type type = this.typeBuilder.CreateType();

            assemblyBuilder.Save(assemblyBuilder.GetName().Name + ".dll");

            return type;
        }

        public GeneratedField CreateField(FieldInfo field)
        {
            var action = new AddFieldAction(this.typeBuilder, field);
            var generatedField = new GeneratedField(action);

            this.actions.Add(action);

            return generatedField;
        }
    }
}
