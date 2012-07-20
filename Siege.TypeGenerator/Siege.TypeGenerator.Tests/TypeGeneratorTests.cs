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
using System.Reflection;
using NUnit.Framework;

namespace Siege.TypeGenerator.Tests
{
    [TestFixture]
    public class TypeGeneratorTests
    {
        private TypeGenerator generator = new TypeGenerator();

        [Test]
        public void Should_Create_Empty_Class_Type()
        {
            Type generatedType = generator.CreateType(context => context.Named("TestType1"));

            Assert.AreEqual("TestType1", generatedType.Name);
            Assert.AreEqual(4, generatedType.GetMethods().Length, "Should only have types from object");
            Assert.AreEqual(0, generatedType.GetProperties().Length, "Should have no properties");
            Assert.AreEqual(0, generatedType.GetFields().Length, "Should have no fields");
            Assert.AreEqual(1, generatedType.GetConstructors().Length, "Should have one constructor");
            Assert.AreEqual(0, generatedType.GetConstructor(new Type[0]).GetParameters().Length, "Should be a default constructor");
        }

        [Test]
        public void Should_Create_From_Base_Type()
        {
            Type generatedType = generator.CreateType(type =>
            {
                type.Named("TestType2");
                type.InheritFrom<BaseType>();
            });

            Assert.AreEqual(typeof(BaseType), generatedType.BaseType);
        }

        [Test]
        public void Should_Be_Able_To_Add_A_Method()
        {
            Type generatedType = generator.CreateType(type =>
            {
                type.Named("TestType3");
                type.AddMethod(method =>
                {
                    method.Named("TestMethod");
                    method.Returns(typeof(void));
                });
            });

            Assert.IsNotNull(generatedType.GetMethod("TestMethod"));
            Assert.AreEqual(0, generatedType.GetMethod("TestMethod").GetParameters().Length);
            Assert.AreEqual(typeof(void), generatedType.GetMethod("TestMethod").ReturnType);
            generatedType.GetMethod("TestMethod").Invoke(Activator.CreateInstance(generatedType), new object[0]);
        }

        [Test]
        public void Should_Be_Able_To_Add_A_Method_With_Parameters()
        {
            Type generatedType = generator.CreateType(type =>
            {
                type.Named("TestType4");
                type.AddMethod(method =>
                {
                    method.Named("TestMethod");
                    method.CreateArgument<string>();
                    method.CreateArgument<BaseType>();
                    method.Returns(typeof(void));
                });
            });

            Assert.IsNotNull(generatedType.GetMethod("TestMethod"));
            Assert.AreEqual(2, generatedType.GetMethod("TestMethod").GetParameters().Length);

            generatedType.GetMethod("TestMethod").Invoke(Activator.CreateInstance(generatedType), new object[] { "yay", new BaseType() });
        }

        [Test]
        public void Should_Be_Able_To_Add_An_Override_Method()
        {
            Type generatedType = generator.CreateType(type =>
            {
                type.Named("TestType5");
                type.InheritFrom<BaseType>();
                type.OverrideMethod<BaseType>(baseType => baseType.DoSomething(null), method => method.WithBody(body =>
                {
                    GeneratedVariable baseValue = body.CreateVariable<string>();
                    baseValue.AssignFrom(() => body.CallBase(method.Method));
                    body.Return(baseValue);
                }));
            });

            Assert.IsNotNull(generatedType.GetMethod("DoSomething"));
            Assert.AreEqual(1, generatedType.GetMethod("DoSomething").GetParameters().Length);
            Assert.AreEqual("yay", generatedType.GetMethod("DoSomething").Invoke(Activator.CreateInstance(generatedType), new[] { "yay" }));
        }

        [Test]
        public void Should_Be_Able_To_Add_An_Override_Method_And_Instantiate_A_Type()
        {
            Type generatedType = generator.CreateType(type =>
            {
                type.Named("TestType6");
                type.InheritFrom<BaseType>();
                type.OverrideMethod<BaseType>(baseType => baseType.DoSomething(null), method =>
                {
                    method.WithBody(body =>
                    {
                        GeneratedVariable variable = body.CreateVariable<Processor>();
                        variable.AssignFrom(body.Instantiate<Processor>());

                        GeneratedVariable baseValue = body.CreateVariable<string>();
                        baseValue.AssignFrom(() => body.CallBase(method.Method));
                        body.Return(baseValue);
                    });
                });
            });

            Assert.IsNotNull(generatedType.GetMethod("DoSomething"));
            Assert.AreEqual(1, generatedType.GetMethod("DoSomething").GetParameters().Length);
            Assert.AreEqual("yay", generatedType.GetMethod("DoSomething").Invoke(Activator.CreateInstance(generatedType), new[] { "yay" }));
        }

        [Test]
        public void Should_Be_Able_To_Add_An_Override_Method_And_Instantiate_A_Type_And_Invoke_A_Method()
        {
            Type generatedType = generator.CreateType(type =>
            {
                type.Named("TestType7");
                type.InheritFrom<BaseType>();
                type.OverrideMethod<BaseType>(baseType => baseType.DoSomething(null), method =>
                {
                    method.WithBody(body =>
                    {
                        GeneratedVariable variable = body.CreateVariable<Processor>();
                        GeneratedVariable returnValue = body.CreateVariable<string>();

                        variable.AssignFrom(body.Instantiate<Processor>());
                        returnValue.AssignFrom(() => variable.Invoke<Processor>(processor => processor.Process(null, null)));

                        GeneratedVariable baseValue = body.CreateVariable<string>();
                        baseValue.AssignFrom(() => body.CallBase(method.Method));
                        body.Return(baseValue);
                    });
                });
            });

            Assert.IsNotNull(generatedType.GetMethod("DoSomething"));
            Assert.AreEqual(1, generatedType.GetMethod("DoSomething").GetParameters().Length);
            Assert.AreEqual("yay", generatedType.GetMethod("DoSomething").Invoke(Activator.CreateInstance(generatedType), new[] { "yay" }));
        }

        [Test]
        public void Should_Be_Able_To_Add_A_Method_And_Instantiate_A_Type_And_Invoke_A_Method()
        {
            Type generatedType = generator.CreateType(type =>
            {
                type.Named("TestType8");
                type.AddMethod(method =>
                {
                    method.Named("TestMethod");
                    method.Returns(typeof(void));
                    method.CreateArgument<string>();
                    method.WithBody(body =>
                    {
                        GeneratedVariable variable = body.CreateVariable<Processor>();
                        GeneratedVariable returnValue = body.CreateVariable<string>();

                        variable.AssignFrom(body.Instantiate<Processor>());
                        returnValue.AssignFrom(() => variable.Invoke<Processor>(processor => processor.Process(null, null)));
                        body.Return(returnValue);
                    });
                });
            });

            Assert.IsNotNull(generatedType.GetMethod("TestMethod"));
            Assert.AreEqual(1, generatedType.GetMethod("TestMethod").GetParameters().Length);
            generatedType.GetMethod("TestMethod").Invoke(Activator.CreateInstance(generatedType), new[] { "yay" });
        }

        [Test]
        public void Should_Be_Able_To_Create_Constructor()
        {
            Type generatedType = generator.CreateType(type =>
            {
                type.Named("TestType9");
                type.AddConstructor(constructor =>
                {
                    constructor.CreateArgument<string>();
                    constructor.CreateArgument<BaseType>();
                });
            });

            Assert.AreEqual(1, generatedType.GetConstructors().Length);
            Assert.IsNotNull(generatedType.GetConstructor(new[] { typeof(string), typeof(BaseType) }));
        }

        [Test]
        public void Should_Be_Able_To_Add_Fields()
        {
            Type generatedType = new TypeGenerator().CreateType(type =>
            {
                type.Named("TestType10");
                type.AddField<string>("field1");
                type.AddField<BaseType>("field2");
            });

            Assert.AreEqual(2, generatedType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance).Length);
            Assert.AreEqual(typeof(string), generatedType.GetField("field1", BindingFlags.NonPublic | BindingFlags.Instance).FieldType);
            Assert.AreEqual(typeof(BaseType), generatedType.GetField("field2", BindingFlags.NonPublic | BindingFlags.Instance).FieldType);
        }

        [Test]
        public void Should_Be_Able_To_Create_Constructor_And_Initialize_Fields()
        {
            Type generatedType = generator.CreateType(type =>
            {
                type.Named("TestType11");
                var field1 = type.AddField<string>("field1");
                var field2 = type.AddField<BaseType>("field2");
                type.AddConstructor(constructor =>
                {
                    var parameter1 = constructor.CreateArgument<string>();
                    var parameter2 = constructor.CreateArgument<BaseType>();

                    constructor.WithBody(body =>
                    {
                        parameter1.AssignTo(field1);
                        parameter2.AssignTo(field2);
                    });
                });
            });

            Assert.AreEqual(1, generatedType.GetConstructors().Length);
            Assert.IsNotNull(generatedType.GetConstructor(new[] { typeof(string), typeof(BaseType) }));
        }

        [Test]
        public void Should_Be_Able_To_Create_A_Func_Wrapping_A_Method()
        {
            Type generatedType = generator.CreateType(type =>
            {
                type.Named("TestType12");
                type.InheritFrom<BaseType>();
                type.OverrideMethod<BaseType>(baseType => baseType.DoSomething(null), method =>
                {
                    method.WithBody(body =>
                    {
                        MethodInfo target = null;

                        var del = body.CreateVariable(typeof(Delegate1));
                        del.AssignFrom(body.Instantiate<Delegate1>());
                        var variable = body.CreateLambda(lambda =>
                        {
                            target = lambda.Target<BaseType>(p => p.DoSomething(null));
                        });

                        var func = variable.CreateFunc(target);
                        var returnValue = body.CreateVariable(method.Method.ReturnType);
                        returnValue.AssignFrom(() => del.Invoke<Delegate1>(d => d.Process(null), func));

                        body.Return(returnValue);
                    });
                });
            });

            var obj = Activator.CreateInstance(generatedType);
            var result = generatedType.GetMethod("DoSomething").Invoke(obj, new[] { "" });
            Assert.AreEqual("yay", result);
        }

        [Test]
        public void Should_Be_Able_To_Create_A_Nested_Func_Wrapping_A_Method()
        {
            Type generatedType = generator.CreateType(type =>
            {
                type.Named("TestType13");
                type.InheritFrom<BaseType>();
                type.OverrideMethod<BaseType>(baseType => baseType.DoSomething(null), method =>
                {
                    method.WithBody(body =>
                    {
                        MethodInfo target = null;

                        var del = body.CreateVariable(typeof(Delegate1));
                        del.AssignFrom(body.Instantiate<Delegate1>());
                        var variable = body.CreateLambda(lambda =>
                        {
                            target = lambda.Target<BaseType>(p => p.DoSomething(null));
                            lambda.CreateNestedLambda(nestedLambda =>
                            {
                                var stringVariable = nestedLambda.CreateVariable<Delegate1>();
                                stringVariable.AssignFrom(nestedLambda.Instantiate<Delegate1>());

                                lambda.CreateNestedLambda(nestedLambda1 =>
                                {
                                    var stringVariable1 = nestedLambda1.CreateVariable<Delegate1>();
                                    stringVariable1.AssignFrom(nestedLambda1.Instantiate<Delegate1>());
                                });
                            });
                        });

                        var func = variable.CreateFunc(target);
                        var returnValue = body.CreateVariable(method.Method.ReturnType);
                        returnValue.AssignFrom(() => del.Invoke<Delegate1>(d => d.Process(null), func));

                        body.Return(returnValue);
                    });
                });
            });

            var obj = Activator.CreateInstance(generatedType);
            var result = generatedType.GetMethod("DoSomething").Invoke(obj, new[] { "" });
            Assert.AreEqual("yay", result);
        }

        [Test]
        public void Should_Pass_Method_Arguments()
        {
            Type generatedType = generator.CreateType(type =>
            {
                type.Named("TestType14");
                type.InheritFrom<BaseTypeWithMultipleArgs>();
                type.OverrideMethod<BaseTypeWithMultipleArgs>(baseType => baseType.DoSomething(null, null), method =>
                {
                    method.WithBody(body =>
                    {
                        MethodInfo target = null;

                        var del = body.CreateVariable(typeof(DelegateWithArguments));
                        var args = body.CreateArray(typeof (MethodArgument));

                        args.AssignFrom(body.InstantiateArray<MethodArgument>(2));
                        
                        var arg1 = body.CreateVariable(typeof(MethodArgument));
                        arg1.AssignFrom(body.Instantiate<MethodArgument>());

                        arg1.SetValue<MethodArgument, int>(x => x.Index, 0);
                        arg1.SetValue<MethodArgument, string>(x => x.Name, method.Method.GetParameters()[0].Name);
                        arg1.SetValue<MethodArgument, object>(x => x.Value, new MethodParameter(0));

                        args.SetValueAtIndex(arg1, 0);

                        var arg2 = body.CreateVariable(typeof(MethodArgument));
                        arg2.AssignFrom(body.Instantiate<MethodArgument>());

                        arg2.SetValue<MethodArgument, int>(x => x.Index, 1);
                        arg2.SetValue<MethodArgument, string>(x => x.Name, method.Method.GetParameters()[1].Name);
                        arg2.SetValue<MethodArgument, object>(x => x.Value, new MethodParameter(1));

                        args.SetValueAtIndex(arg2, 1);

                        del.AssignFrom(body.Instantiate<DelegateWithArguments>());
                        var variable = body.CreateLambda(lambda =>
                        {
                            target = lambda.Target<BaseType>(p => p.DoSomething(null));
                        });

                        var func = variable.CreateFunc(target);
                        var returnValue = body.CreateVariable(method.Method.ReturnType);
                        returnValue.AssignFrom(() => del.Invoke<DelegateWithArguments>(d => d.Process(null, null), func, args));

                        body.Return(returnValue);
                    });
                });
            });

            var obj = Activator.CreateInstance(generatedType);
            var result = generatedType.GetMethod("DoSomething").Invoke(obj, new object[] { "hurr", new SampleClass() });
            Assert.AreEqual("yay: hurr, val1, sample", result);
        }

        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            generator.Save();
        }
    }
}