using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;

namespace Siege.ServiceLocation.TypeGeneration
{
    public class TypeGenerator
    {
        private static readonly Hashtable definedTypes = new Hashtable();
        public static IContextualServiceLocator ServiceLocator;

        public static Type Generate<TBaseType>()
        {
            if (definedTypes.ContainsKey(typeof(TBaseType))) return (Type)definedTypes[typeof (TBaseType)];
            if (typeof(TBaseType).GetMethods().Where(methodInfo => methodInfo.GetCustomAttributes(typeof(IAopAttribute), true).Count() > 0).Count() == 0) return typeof (TBaseType);


            var dllName = typeof(TBaseType).Namespace + ".Dynamic" + typeof(TBaseType).Name;
            AssemblyName assemblyName = new AssemblyName { Name = dllName };
            AppDomain thisDomain = Thread.GetDomain();

            var assemblyBuilder = thisDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.RunAndSave);
            var moduleBuilder = assemblyBuilder.DefineDynamicModule(assemblyBuilder.GetName().Name, assemblyBuilder.GetName().Name + ".dll");

            TypeBuilder typeBuilder = moduleBuilder.DefineType("Dynamic" + typeof(TBaseType).Name,
                                                               TypeAttributes.Public |
                                                               TypeAttributes.Class |
                                                               TypeAttributes.AutoClass |
                                                               TypeAttributes.AnsiClass |
                                                               TypeAttributes.BeforeFieldInit |
                                                               TypeAttributes.AutoLayout,
                                                               typeof(TBaseType));

            GenerateConstructors(typeBuilder);
            GenerateMethods<TBaseType>(typeBuilder);

            Type type = typeBuilder.CreateType();
            definedTypes.Add(typeof(TBaseType), type);

            assemblyBuilder.Save(assemblyBuilder.GetName().Name + ".dll");
            return (Type)definedTypes[typeof(TBaseType)];
        }

        private static void GenerateConstructors(TypeBuilder builder)
        {
            ConstructorBuilder constructor = builder.DefineConstructor(
                MethodAttributes.Public |
                MethodAttributes.SpecialName |
                MethodAttributes.RTSpecialName,
                CallingConventions.Standard,
                new Type[0]);

            //Define the reflection ConstructorInfor for System.Object
            ConstructorInfo conObj = typeof (object).GetConstructor(new Type[0]);
            //call constructor of base object
            ILGenerator il = constructor.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Call, conObj);
            il.Emit(OpCodes.Ret);
        }

        private static void GenerateMethods<TBaseType>(TypeBuilder builder)
        {
            MethodInfo[] methods = typeof (TBaseType).GetMethods();
            foreach (MethodInfo method in methods.Where(methodInfo => methodInfo.GetCustomAttributes(typeof(IAopAttribute), true).Count() > 0))
            {
                var parameters = method.GetParameters();
                List<Type> types = new List<Type>();

                foreach (ParameterInfo info in parameters)
                {
                    types.Add(info.ParameterType);
                }
              
                MethodBuilder methodBuilder = builder.DefineMethod(
                    method.Name,
                    MethodAttributes.Public | MethodAttributes.Virtual,
                    method.ReturnType, types.ToArray());

                GenerateEncapsulatedCalls(methodBuilder.GetILGenerator(), method);

                builder.DefineMethodOverride(methodBuilder, typeof(TBaseType).GetMethod(method.Name));
            }
        }

        private static void GenerateEncapsulatedCalls(ILGenerator methodGenerator, MethodInfo method)
        {
            Type funcType = null;

            if(method.ReturnType == typeof(void))
            {
                funcType = typeof (Action);
            }
            else
            {
                funcType = typeof(Func<>).MakeGenericType(method.ReturnType);
            }

            var funcConstructor = funcType.GetConstructor(new[] { typeof(object), typeof(IntPtr) });

            if (method.ReturnType != typeof(void)) methodGenerator.DeclareLocal(method.ReturnType);
            methodGenerator.Emit(OpCodes.Nop);

            foreach (Attribute attribute in method.GetCustomAttributes(typeof(IProcessEncapsulatingAttribute), true))
            {
                if (ServiceLocator == null)
                {
                    methodGenerator.Emit(OpCodes.Newobj, attribute.GetType().GetConstructor(new Type[0]));
                }
                else
                {
                    methodGenerator.Emit(OpCodes.Ldarg_0);
                    methodGenerator.Emit(OpCodes.Ldfld, typeof(TypeGenerator).GetField("ServiceLocator"));
                    methodGenerator.Emit(OpCodes.Callvirt, typeof(IServiceLocator).GetMethod("GetInstance", new Type[0]).MakeGenericMethod(attribute.GetType()));
                }
                methodGenerator.Emit(OpCodes.Ldarg_0);
                methodGenerator.Emit(OpCodes.Ldftn, method);
                methodGenerator.Emit(OpCodes.Newobj, funcConstructor);

                MethodInfo info = null;

                if (method.ReturnType == typeof(void))
                {
                    info = attribute.GetType().GetMethod("Process", new [] {typeof(Action)});
                }
                else
                {
                    info = attribute.GetType().GetMethods().Where(
                        memberInfo => memberInfo.Name == "Process" && 
                                      memberInfo.GetParameters().Where(parameter => parameter.ParameterType != typeof (Action)).Count() >
                                      0).First().MakeGenericMethod(method.ReturnType);
                }

                methodGenerator.Emit(OpCodes.Call, info);
            }

            if(method.ReturnType != typeof(void))
            {
                methodGenerator.Emit(OpCodes.Stloc_0);
                methodGenerator.Emit(OpCodes.Ldloc_0);
            }
            
            methodGenerator.Emit(OpCodes.Ret);
        }
    }
}