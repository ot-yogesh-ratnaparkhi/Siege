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

        public DelegateGenerator(BaseTypeGenerationContext context)
        {
            this.context = context;
        }

        public void WithArgument(Type argumentType)
        {
            argumentTypes.Add(argumentType);
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

                foreach (DelegateMethod info in methods)
                {
                    entryPoint = type.AddMethod(method =>
                    {
                        method.Named(() => info.Name);
                        method.Returns(() => info.Method().MethodBuilder().MethodBuilder.ReturnType);
                        method.WithBody(body =>
                        {
                            var variable = body.CreateVariable(() => info.Method().MethodBuilder().MethodBuilder.ReturnType);
                            variable.AssignFrom(() => body.Call(() => info.Method().MethodBuilder().MethodBuilder, () => fields));
                            body.Return(variable);
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
    }

    internal class DelegateMethod
    {
        public string Name { get; set; }
        public Func<GeneratedMethod> Method { get; set; }
    }
}
