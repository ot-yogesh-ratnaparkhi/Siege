using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;

namespace Siege.ServiceLocation.TypeGeneration
{
    public interface IAopAttribute
    {
        TResponseType Process<TResponseType>(Func<TResponseType> func);
    }

    public class SampleAttribute : Attribute, IAopAttribute
    {
        public TResponseType Process<TResponseType>(Func<TResponseType> func)
        {
            throw new NotImplementedException();
            return func.Invoke();
        }
    }

    public class LolAttribute : SampleAttribute
    {
        public new TResponseType Process<TResponseType>(Func<TResponseType> func)
        {
            return base.Process(() => default(TResponseType));
        }
    }

    public class TypeGenerator
    {
        private static readonly AssemblyBuilder assemblyBuilder;
        private static readonly ModuleBuilder moduleBuilder;
        private static readonly object lockObject = new object();
        private static readonly Hashtable definedTypes = new Hashtable();

        static TypeGenerator()
        {
            if(assemblyBuilder == null)
            {
                lock (lockObject)
                {
                    if (assemblyBuilder == null)
                    {
                        AssemblyName assemblyName = new AssemblyName {Name = "GeneratedTypes"};
                        AppDomain thisDomain = Thread.GetDomain();

                        assemblyBuilder = thisDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.RunAndSave);
                        moduleBuilder = assemblyBuilder.DefineDynamicModule(assemblyBuilder.GetName().Name, assemblyBuilder.GetName().Name + ".dll");
                    }
                }
            }
        }

        public Type Generate<TBaseType>()
        {
            if (definedTypes.ContainsKey(typeof(TBaseType))) return (Type)definedTypes[typeof (TBaseType)];

            TypeBuilder typeBuilder = moduleBuilder.DefineType("Dynamic" + typeof(TBaseType),
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

                foreach(ParameterInfo info in parameters)
                {
                    types.Add(info.ParameterType);
                }

                MethodBuilder methodBuilder = builder.DefineMethod(
                    method.Name,
                    MethodAttributes.Public | MethodAttributes.HideBySig,
                    method.ReturnType, types.ToArray());

                Type funcType = typeof(Func<>).MakeGenericType(method.ReturnType);
                var funcConstructor = funcType.GetConstructor(new[] { typeof(object), typeof(IntPtr) });
                ILGenerator methodGenerator = methodBuilder.GetILGenerator();

                methodGenerator.DeclareLocal(method.ReturnType);
                methodGenerator.Emit(OpCodes.Nop);

                foreach (Attribute attribute in method.GetCustomAttributes(typeof(IAopAttribute), true))
                {
                    methodGenerator.Emit(OpCodes.Newobj, attribute.GetType().GetConstructor(new Type[0]));
                    methodGenerator.Emit(OpCodes.Ldarg_0);
                    methodGenerator.Emit(OpCodes.Ldftn, method);
                    methodGenerator.Emit(OpCodes.Newobj, funcConstructor);
                    methodGenerator.Emit(OpCodes.Call, attribute.GetType().GetMethod("Process").MakeGenericMethod(method.ReturnType));
                }

                methodGenerator.Emit(OpCodes.Stloc_0);
                methodGenerator.Emit(OpCodes.Ldloc_0);
                methodGenerator.Emit(OpCodes.Ret);
            }
        }
    }
}
