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
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Siege.TypeGenerator
{
    public class DelegateGenerator
    {
        private readonly List<DelegateMethod> methods = new List<DelegateMethod>();
        private readonly BaseTypeGenerationContext context;
        private readonly List<Type> argumentTypes;
        internal NestedTypeGenerationContext NestedType { get; set; }
        internal Func<ConstructorBuilder> Constructor { get; set; }
        private Type returnType;

        public DelegateGenerator(BaseTypeGenerationContext context)
        {
            argumentTypes = new List<Type>();
            this.context = context;
        }

        public void WithArgument(Type argumentType)
        {
            argumentTypes.Add(argumentType);
        }

        public void Returns(Type returnType)
        {
            this.returnType = returnType;
        }

        public void Build()
        {
            GeneratedMethod entryPoint = null;
            NestedType = context.CreateNestedType(type =>
            {
            	var callingType = type.AddField(context.Builder, "CallingType");

                var fields = new List<GeneratedField>
                {
                    callingType
                };

                for(int i = 0; i < argumentTypes.Count; i++)
                {
                    fields.Add(type.AddField(argumentTypes[i], "Field" + i));
                }

                var constructorAction = type.AddConstructor(constructor =>
                {
                    var constructorArguments = new Hashtable();
                    for (int i = 0; i < fields.Count; i++)
                    {
                        GeneratedField field = fields[i];
                        constructorArguments.Add(field, constructor.CreateArgument(() => field));
                    }

                    constructor.WithBody(body =>
                    {
                        foreach (GeneratedField field in constructorArguments.Keys)
                        {
                            var parameter = (GeneratedParameter)constructorArguments[field];
                            parameter.AssignTo(field);
                        }
                    });
                });

                Constructor = () => constructorAction.Constructor;

                for (int i = 0; i < methods.Count; i++)
                {
                    var info = methods[i];
                    entryPoint = type.AddMethod(method =>
                    {
                        method.Named(info.Name);
                        method.Returns(returnType);
                        method.WithBody(body =>
                        {
                            if(info.Body != null) info.Body(body);

                            if (returnType != (typeof(void)))
                            {
                                var variable = body.CreateVariable(returnType);

								if (entryPoint == null)
								{
									if (info.ExitClosure == null)
									{
										variable.AssignFrom(() => body.Call(() => info.Method().MethodBuilder().MethodBuilder, () => fields));
									}
									else
									{
										info.ExitClosure(body, variable, info.Method, callingType);
									}
								}
								else
								{
									if (info.ExitClosure == null)
									{
										body.TargettingSelf();
										variable.AssignFrom(() => body.Call(entryPoint, returnType));
									}
									else
									{
										info.ExitClosure(body, variable, () => entryPoint, callingType);
									}
								}
                                body.Return(variable);
                            }
                            else
                            {
                                if(entryPoint == null)
                                {
                                  	if (info.ExitClosure == null)
									{
										body.Call(() => info.Method().MethodBuilder().MethodBuilder, () => fields);
									}
									else
									{
										info.ExitClosure(body, null, info.Method, callingType);
									}
                                }
                                else
                                {
									if (info.ExitClosure == null)
									{
										body.TargettingSelf();
										body.Call(() => entryPoint, () => fields);
									}
									else
									{
										info.ExitClosure(body, null, () => entryPoint, callingType);
									}
                                }
                                body.Return();
                            }
                        });
                    });
                }
            });

            NestedType.EntryPoint = entryPoint.MethodBuilder;
        }

        public void AddMethod(string name, Func<GeneratedMethod> info)
        {
            methods.Add(new DelegateMethod { Name = name, Method = info});
        }

        public void AddMethod(string name, Func<GeneratedMethod> info, Action<MethodBodyContext> closure)
        {
            methods.Add(new DelegateMethod { Name = name, Method = info, Body = closure });
        }

		public void AddMethod(string name, Func<GeneratedMethod> info, Action<MethodBodyContext> closure, Action<MethodBodyContext, GeneratedVariable, Func<GeneratedMethod>, GeneratedField> exitClosure)
		{
			methods.Add(new DelegateMethod { Name = name, Method = info, Body = closure, ExitClosure = exitClosure });
		}
    }

    public class DelegateMethod
    {
        public string Name { get; set; }
        public Func<GeneratedMethod> Method { get; set; }
        public Action<MethodBodyContext> Body { get; set; }
		public Action<MethodBodyContext, GeneratedVariable, Func<GeneratedMethod>, GeneratedField> ExitClosure { get; set; }

    }
}
