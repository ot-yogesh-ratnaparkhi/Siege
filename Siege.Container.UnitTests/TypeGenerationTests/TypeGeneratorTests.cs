using System;
using NUnit.Framework;
using Siege.ServiceLocation.TypeGeneration;

namespace Siege.Container.UnitTests.TypeGenerationTests
{
    [TestFixture]
    public class TypeGeneratorTests
    {
        [SetUp]
        public void SetUp()
        {
            TypeGenerator.ServiceLocator = null;
        }

        [Test]
        public void Should_Generate_Type()
        {
            var result = TypeGenerator.Generate<TestType>();
            TestType instance = (TestType)Activator.CreateInstance(result);
            Assert.IsTrue(instance is TestType);
            Assert.AreNotEqual(typeof(TestType), instance.GetType());
        }

        [Test]
        [ExpectedException(typeof(NotImplementedException))]
        public void Should_Throw_Exception_From_Attribute()
        {
            var result = TypeGenerator.Generate<TestType>();
            TestType instance = (TestType)Activator.CreateInstance(result);
            
            instance.Test();
        }
    }

    public class TestType
    {
        [ThrowsException]
        public virtual string Test()
        {
            return null;
        }
    }
}
