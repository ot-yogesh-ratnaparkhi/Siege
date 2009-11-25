using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Siege.DynamicTypeGeneration;
using Siege.ServiceLocation.AOP;

namespace Siege.ServiceLocation.Aop
{
    public class AopBinder
    {
        private static readonly Hashtable definedTypes = new Hashtable();
        public static IContextualServiceLocator ServiceLocator;

        public static Type Generate<TBaseType>()
        {
            if (definedTypes.ContainsKey(typeof(TBaseType))) return (Type)definedTypes[typeof (TBaseType)];
            if (typeof(TBaseType).GetMethods().Where(methodInfo => methodInfo.GetCustomAttributes(typeof(IAopAttribute), true).Count() > 0).Count() == 0) return typeof (TBaseType);

            TypeGenerator generator = new TypeGenerator("Dynamic" + typeof(TBaseType).Name, typeof(TBaseType));
            
            GenerateMethods<TBaseType>(generator);

            Type type = generator.Create();
            definedTypes.Add(typeof(TBaseType), type);

            return (Type)definedTypes[typeof(TBaseType)];
        }

        private static void GenerateMethods<TBaseType>(TypeGenerator generator)
        {
            MethodInfo[] methods = typeof (TBaseType).GetMethods();
            GeneratedField field = generator.CreateField(typeof (AopBinder).GetField("ServiceLocator"));

            foreach (MethodInfo method in methods.Where(methodInfo => methodInfo.GetCustomAttributes(typeof(IAopAttribute), true).Count() > 0))
            {
                List<Type> types = new List<Type>();

                foreach (ParameterInfo info in method.GetParameters())
                {
                    types.Add(info.ParameterType);
                }

                GeneratedMethod generatedMethod = generator.CreateMethod(method.Name, method.ReturnType, types.ToArray(), true);
                generatedMethod.Assign(field.Source).To(field.Field);

                GenerateCalls(generatedMethod, method, typeof(IPreProcessingAttribute));
                GenerateEncapsulatedCalls<TBaseType>(generatedMethod, method, generator, types.ToArray(), field.Field);
                GenerateCalls(generatedMethod, method, typeof(IPostProcessingAttribute));

                generatedMethod.ReturnFrom(method).Override(typeof(TBaseType).GetMethod(method.Name));
            }
        }

        private static void GenerateCalls(GeneratedMethod generatedMethod, MethodInfo method, Type attributeType)
        {
            foreach (Attribute attribute in method.GetCustomAttributes(attributeType, true))
            {
                GenerateMethod(generatedMethod, attribute);
            }
        }

        private static void GenerateMethod(GeneratedMethod generatedMethod, Attribute attribute)
        {
            if (ServiceLocator == null)
            {
                generatedMethod.Instantiate(attribute.GetType(), new Type[0]);
            }
            else
            {

                generatedMethod.Call(typeof(IServiceLocator).GetMethod("GetInstance", new Type[0]).MakeGenericMethod(attribute.GetType())).On(typeof(AopBinder).GetField("ServiceLocator"));
            }

            generatedMethod.Call(attribute.GetType().GetMethod("Process"));
        }

        private static MethodInfo GenerateNestedMethod<TBaseType>(object[] attributes, Type returnType, int counter, TypeGenerator builder, MethodInfo method, Type[] types, FieldInfo serviceLocatorField)
        {
            if (attributes.Length == counter + 1) return typeof(TBaseType).GetMethod(method.Name, types);
            
            method = GenerateNestedMethod<TBaseType>(attributes, returnType, counter + 1, builder, method, types, serviceLocatorField);
            
            Attribute attribute = (Attribute)attributes[counter];

            GeneratedMethod generatedMethod = builder.CreateMethod(method.Name + "_" + counter, method.ReturnType, types.ToArray(), false);

            if (ServiceLocator == null)
            {
                generatedMethod.Instantiate(attribute.GetType(), new Type[0]);
            }
            else
            {
                generatedMethod.Call(typeof(IServiceLocator).GetMethod("GetInstance", new Type[0]).MakeGenericMethod(attribute.GetType())).On(serviceLocatorField);
            }

            MethodInfo info;

            if (returnType == typeof(void))
            {
                info = attribute.GetType().GetMethod("Process", new[] { typeof(Action) });
            }
            else
            {
                info = attribute.GetType().GetMethods().Where(
                    memberInfo => memberInfo.Name == "Process" &&
                                  memberInfo.GetParameters().Where(parameter => parameter.ParameterType != typeof(Action)).Count() >
                                  0).First().MakeGenericMethod(returnType);
            }

            generatedMethod.UsingFunc(method.ReturnType).Targetting(method).Call(info);

            var completedMethod = generatedMethod.ReturnFrom(method);

            return completedMethod.Method;
        }

        private static void GenerateEncapsulatedCalls<TBaseType>(GeneratedMethod generatedMethod, MethodInfo method, TypeGenerator generator, Type[] types, FieldInfo serviceLocatorField)
        {
            var attributes = method.GetCustomAttributes(typeof(IProcessEncapsulatingAttribute), true);
            if(attributes.Length == 0)
            {
                GeneratedMethod subMethod = generator.CreateMethod(method.Name + "_Base", method.ReturnType, types, true);
                subMethod.CallBase(method, typeof(TBaseType)).CaptureResult();

                var completedSubMethod = subMethod.ReturnFrom(method);
                generatedMethod.Call(completedSubMethod.Method).WithParametersFrom(method).CaptureResult();

                return;
            }

            if (ServiceLocator == null)
            {
                generatedMethod.Instantiate(attributes[0].GetType(), new Type[0]);
            }
            else
            {

                generatedMethod.Call(typeof(IServiceLocator).GetMethod("GetInstance", new Type[0]).MakeGenericMethod(attributes[0].GetType())).On(serviceLocatorField);
            }

            MethodInfo info;

            if (method.ReturnType == typeof(void))
            {
                info = attributes[0].GetType().GetMethod("Process", new[] { typeof(Action) });
            }
            else
            {
                info = attributes[0].GetType().GetMethods().Where(
                    memberInfo => memberInfo.Name == "Process" &&
                                  memberInfo.GetParameters().Where(parameter => parameter.ParameterType != typeof(Action)).Count() >
                                  0).First().MakeGenericMethod(method.ReturnType);
            }


            List<Type> parameters = new List<Type>();
            foreach (ParameterInfo parameterInfo in method.GetParameters())
            {
                parameters.Add(parameterInfo.ParameterType);
            }

            generatedMethod.UsingFunc(method.ReturnType).Targetting(GenerateNestedMethod<TBaseType>(attributes, method.ReturnType, 0, generator, method, types, serviceLocatorField)).Call(info).CaptureResult();
        }
    }
}