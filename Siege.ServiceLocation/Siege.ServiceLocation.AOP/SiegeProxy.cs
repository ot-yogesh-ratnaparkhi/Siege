/*   Copyright 2009 - 2010 Marcus Bratton

     Licensed under the Apache License, Version 2.0 (the "License");
     you may not use this file except in compliance with the License.
     You may obtain a copy of the License at

     http://www.apache.org/licenses/LICENSE-2.0

     Unless required by applicable law or agreed to in writing, software
     distributed under the License is distributed on an "AS IS" BASIS,
     WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
     See the License for the specific language governing permissions and
     limitations under the License.
*/

using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using Siege.DynamicTypeGeneration;

namespace Siege.ServiceLocation.AOP
{
    public class SiegeProxy
    {
        private static readonly Hashtable definedTypes = new Hashtable();
        public Type Create<TProxy>() where TProxy : class
        {
            return Create(typeof (TProxy));
        }

        public Type Create(Type typeToProxy)
        {
            if (definedTypes.ContainsKey(typeToProxy)) return (Type)definedTypes[typeToProxy];
            if (typeToProxy.GetMethods().Where(methodInfo => methodInfo.GetCustomAttributes(typeof(IAopAttribute), true).Count() > 0).Count() == 0) return typeToProxy;

            var generator = new TypeGenerator();
            
            Type generatedType = generator.CreateType(type =>
            {
                type.Named(typeToProxy.Name);
                type.InheritFrom(typeToProxy);
                var field = type.AddField<IServiceLocator>("serviceLocator");

                type.AddConstructor(constructor => constructor.CreateArgument<IServiceLocator>().AssignTo(field));

                foreach (MethodInfo methodInfo in typeToProxy.GetMethods(BindingFlags.Public | BindingFlags.Instance))
                {
                    if (methodInfo.IsVirtual && methodInfo.GetBaseDefinition().DeclaringType != typeof(object))
                    {
                        type.OverrideMethod(methodInfo, method => method.WithBody(body =>
                        {
                            if (methodInfo.GetCustomAttributes(typeof(IAopAttribute), true).Count() == 0)
                            {
                                body.CallBase(methodInfo);
                                return;
                            }

                            var serviceLocator = body.CreateVariable<IServiceLocator>();
                            serviceLocator.AssignFrom(field);

                            GeneratePreProcessors(body, methodInfo, serviceLocator);

                            var returnValue = GenerateEncapsulatedCalls(methodInfo, body, serviceLocator);

                            GeneratePostProcessors(body, methodInfo, serviceLocator);

                            if(returnValue != null) body.Return(returnValue);
                        }));
                    }
                }
            });

            generator.Save();
            return generatedType;
        }

        private void GeneratePreProcessors(MethodBodyContext body, MethodInfo methodInfo, GeneratedVariable serviceLocator)
        {
            foreach (Attribute attribute in methodInfo.GetCustomAttributes(typeof(IPreProcessingAttribute), true))
            {
                var preProcessor = body.CreateVariable<IPreProcessingAttribute>();
                preProcessor.AssignFrom(() => serviceLocator.Invoke(typeof(Microsoft.Practices.ServiceLocation.IServiceLocator).GetMethod("GetInstance", new Type[0]).MakeGenericMethod(attribute.GetType())));

                preProcessor.Invoke<IPreProcessingAttribute>(processor => processor.Process());
            }
        }

        private void GeneratePostProcessors(MethodBodyContext body, MethodInfo methodInfo, GeneratedVariable serviceLocator)
        {
            foreach (Attribute attribute in methodInfo.GetCustomAttributes(typeof(IPostProcessingAttribute), true))
            {
                var postProcessor = body.CreateVariable<IPostProcessingAttribute>();
                postProcessor.AssignFrom(() => serviceLocator.Invoke(typeof(Microsoft.Practices.ServiceLocation.IServiceLocator).GetMethod("GetInstance", new Type[0]).MakeGenericMethod(attribute.GetType())));

                postProcessor.Invoke<IPostProcessingAttribute>(processor => processor.Process());
            }
        }

        private GeneratedVariable GenerateEncapsulatedCalls(MethodInfo methodInfo, MethodBodyContext body, GeneratedVariable serviceLocator)
        {
            var attributes = methodInfo.GetCustomAttributes(typeof(IProcessEncapsulatingAttribute), true);

            GeneratedVariable variable = null;

            if (attributes.Length == 0)
            {
                if(methodInfo.ReturnType == typeof(void))
                {
                    body.CallBase(methodInfo);
                }
                else
                {
                    variable = body.CreateVariable(methodInfo.ReturnType);
                    variable.AssignFrom(() => body.CallBase(methodInfo));
                }

                return variable;
            }

            var encapsulating = body.CreateVariable<IProcessEncapsulatingAttribute>();
            encapsulating.AssignFrom(() => serviceLocator.Invoke(typeof(Microsoft.Practices.ServiceLocation.IServiceLocator).GetMethod("GetInstance", new Type[0]).MakeGenericMethod(attributes[0].GetType())));
            
            var lambdaVariable = body.CreateLambda(lambda =>
            {
                lambda.Target(methodInfo);
            });

            var func = lambdaVariable.CreateFunc(methodInfo);
            
            if (methodInfo.ReturnType != typeof(void))
            {
                var funcProcessor = attributes[0].GetType().GetMethods().Where(
                    memberInfo => memberInfo.Name == "Process" &&
                                  memberInfo.GetParameters()
                                  .Where(parameter => parameter.ParameterType != typeof(Action))
                                  .Count() > 0).First().MakeGenericMethod(methodInfo.ReturnType);
                variable = body.CreateVariable(methodInfo.ReturnType);
                variable.AssignFrom(() => encapsulating.Invoke(funcProcessor, func));
            }
            else
            {
                var actionProcessor = attributes[0].GetType().GetMethods().Where(
                    memberInfo => memberInfo.Name == "Process" &&
                                  memberInfo.GetParameters()
                                      .Where(parameter => parameter.ParameterType == typeof(Action))
                                      .Count() > 0).First();
                encapsulating.Invoke(actionProcessor, func);
            }

            return variable;
        }
    }
}
