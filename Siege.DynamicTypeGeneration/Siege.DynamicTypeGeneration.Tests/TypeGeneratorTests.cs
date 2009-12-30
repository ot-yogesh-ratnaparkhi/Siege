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
using NUnit.Framework;

namespace Siege.DynamicTypeGeneration.Tests
{
    [TestFixture]
    public class TypeGeneratorTests
    {
        [Test]
        public void Should_Create_Empty_Class_Type()
        {
            Type generatedType = new TypeGenerator().CreateType(context => context.Named("TestType"));

            Assert.AreEqual("TestType", generatedType.Name);
            Assert.AreEqual(4, generatedType.GetMethods().Length, "Should only have types from object");
            Assert.AreEqual(0, generatedType.GetProperties().Length, "Should have no properties");
            Assert.AreEqual(0, generatedType.GetFields().Length, "Should have no fields");
            Assert.AreEqual(1, generatedType.GetConstructors().Length, "Should have one constructor");
            Assert.AreEqual(0, generatedType.GetConstructor(new Type[0]).GetParameters().Length, "Should be a default constructor");
        }

        [Test]
        public void Should_Create_From_Base_Type()
        {
            Type generatedType = new TypeGenerator().CreateType(type =>
            {
                type.Named("TestType");
                type.InheritFrom<BaseType>();
            });

            Assert.AreEqual(typeof(BaseType), generatedType.BaseType);
        }

        [Test]
        public void Should_Be_Able_To_Add_A_Method()
        {
            Type generatedType = new TypeGenerator().CreateType(type =>
            {
                type.Named("TestType");
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
            Type generatedType = new TypeGenerator().CreateType(type =>
            {
                type.Named("TestType");
                type.AddMethod(method =>
                {
                    method.Named("TestMethod");
                    method.AddParameterType(typeof(string));
                    method.AddParameterType(typeof(BaseType));
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
            Type generatedType = new TypeGenerator().CreateType(type =>
            {
                type.Named("TestType");
                type.InheritFrom<BaseType>();
                type.OverrideMethod<BaseType>(baseType => baseType.DoSomething(null), method => method.WithBody(body => body.CallBase(method.Method)));
            });

            Assert.IsNotNull(generatedType.GetMethod("DoSomething"));
            Assert.AreEqual(1, generatedType.GetMethod("DoSomething").GetParameters().Length);
            Assert.AreEqual("yay", generatedType.GetMethod("DoSomething").Invoke(Activator.CreateInstance(generatedType), new[] { "yay" }));
        }

        [Test]
        public void Should_Be_Able_To_Add_An_Override_Method_And_Instantiate_A_Type()
        {
            Type generatedType = new TypeGenerator().CreateType(type =>
            {
                type.Named("TestType");
                type.InheritFrom<BaseType>();
                type.OverrideMethod<BaseType>(baseType => baseType.DoSomething(null), method =>
                {
                    method.WithBody(body =>
                    {
                        GeneratedVariable<Processor> variable = body.CreateVariable<Processor>();
                        variable.AssignFrom(body.Instantiate<Processor>());

                        body.CallBase(method.Method);
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
            Type generatedType = new TypeGenerator().CreateType(type =>
            {
                type.Named("TestType");
                type.InheritFrom<BaseType>();
                type.OverrideMethod<BaseType>(baseType => baseType.DoSomething(null), method =>
                {
                    method.WithBody(body =>
                    {
                        GeneratedVariable<Processor> variable = body.CreateVariable<Processor>();

                        variable.AssignFrom(body.Instantiate<Processor>());
                        variable.Invoke(processor => processor.Process());

                        body.CallBase(method.Method);
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
            Type generatedType = new TypeGenerator().CreateType(type =>
            {
                type.Named("TestType");
                type.AddMethod(method =>
                {
                    method.Named("TestMethod");
                    method.Returns(typeof(void));
                    method.AddParameterType(typeof(string));
                    method.WithBody(body =>
                    {
                        GeneratedVariable<Processor> variable = body.CreateVariable<Processor>();

                        variable.AssignFrom(body.Instantiate<Processor>());
                        variable.Invoke(processor => processor.Process());
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
            Type generatedType = new TypeGenerator().CreateType(type =>
            {
                type.Named("TestType");
                type.AddConstructor(constructor =>
                {
                    constructor.CreateArgument<string>();
                    constructor.CreateArgument<BaseType>();
                });
            });

            Assert.AreEqual(1, generatedType.GetConstructors().Length);
            Assert.IsNotNull(generatedType.GetConstructor(new[] { typeof(string), typeof(BaseType)}));
        }

        [Test]
        public void Should_Be_Able_To_Add_Fields()
        {
            Type generatedType = new TypeGenerator().CreateType(type =>
            {
                type.Named("TestType");
                type.AddField<string>("field1");
                type.AddField<BaseType>("field2");
            });

            Assert.AreEqual(2, generatedType.GetFields().Length);
            Assert.AreEqual(typeof(string), generatedType.GetField("field1").FieldType);
            Assert.AreEqual(typeof(BaseType), generatedType.GetField("field2").FieldType);
        }

        [Test]
        public void Should_Be_Able_To_Create_Constructor_And_Initialize_Fields()
        {
            Type generatedType = new TypeGenerator().CreateType(type =>
            {
                type.Named("TestType");
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
    }

    public class Processor
    {
        public void Process()
        {
            
        }
    }

    public class BaseType
    {
        public virtual string DoSomething(string val1)
        {
            return val1;
        }
    }

    public class SubType : BaseType
    {
        private readonly string value;
        private readonly BaseType baseType;

        public SubType(string value, BaseType baseType)
        {
            this.value = value;
            this.baseType = baseType;
        }

        public void TestMethod()
        {
            
        }

        public override string DoSomething(string val1)
        {
            var processor = new Processor();
            processor.Process();
            return base.DoSomething(val1);
        }
    }
}