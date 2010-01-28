using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Siege.DynamicTypeGeneration
{
    public class DelegateGenerator
    {
        private List<DelegateMethod> methods = new List<DelegateMethod>();
        private readonly BaseTypeGenerationContext context;
        private List<Type> argumentTypes = new List<Type>();
        internal NestedTypeGenerationContext NestedType { get; set; }
        internal Func<ConstructorBuilder> Constructor { get; set; }
        private Type returnType;

        public DelegateGenerator(BaseTypeGenerationContext context)
        {
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
                List<GeneratedField> fields = new List<GeneratedField>
                {
                    type.AddField(context.Builder, "CallingType")
                };

                for(int i = 0; i < argumentTypes.Count; i++)
                {
                    fields.Add(type.AddField(argumentTypes[i], "Field" + i));
                }

                var constructorAction = type.AddConstructor(constructor =>
                {
                    Hashtable constructorArguments = new Hashtable();
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
                                
                                if(entryPoint == null)
                                {
                                    variable.AssignFrom(() => body.Call(() => info.Method().MethodBuilder().MethodBuilder, () => fields));
                                }
                                else
                                {
                                    body.TargettingSelf();
                                    variable.AssignFrom(() => body.Call(entryPoint, returnType));
                                }
                                body.Return(variable);
                            }
                            else
                            {
                                if(entryPoint == null)
                                {
                                    body.Call(() => info.Method().MethodBuilder().MethodBuilder, () => fields);
                                }
                                else
                                {
                                    body.TargettingSelf();
                                    body.Call(() => entryPoint, () => fields);
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
    }

    public class DelegateMethod
    {
        public string Name { get; set; }
        public Func<GeneratedMethod> Method { get; set; }
        public Action<MethodBodyContext> Body { get; set; }
    }
}
