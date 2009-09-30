using System;
using NUnit.Framework;
using Siege.ServiceLocation.TypeGeneration;

namespace Siege.Container.UnitTests.TypeGenerationTests
{
    [TestFixture]
    public class TypeGeneratorTests
    {
        [Test]
        public void Should_Generate_Type()
        {
            TypeGenerator generator = new TypeGenerator();

            var result = generator.Generate<TestType>();
            ITestType instance = (ITestType)Activator.CreateInstance(result);
            Assert.IsTrue(instance is TestType);
            Assert.AreNotEqual(typeof(TestType), instance.GetType());

            instance.Test();
        }
    }

    public interface ITestType
    {
        string Test();
    }

    public class TestType : ITestType
    {
        [Sample]
        public virtual string Test()
        {
            return null;
        }
    }
}
