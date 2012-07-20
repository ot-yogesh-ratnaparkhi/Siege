//using NUnit.Framework;

//namespace Siege.ServiceLocator.UnitTests
//{
//    [TestFixture]
//    public abstract partial class ServiceLocatorTests
//    {
//        [Test]
//        public void ShouldRegisterMultipleRegistrationsAtOnce()
//        {
//            locator.Register(Given<ISampleInterface>.Then(given =>
//                            {
//                                given.Then<SampleClass>();
//                                given.InitializeWith(test => test.SampleProperty = "lulz");
//                            }));

//            var instance = locator.GetInstance<ISampleInterface>();

//            Assert.IsInstanceOf<SampleClass>(instance);
//            Assert.AreEqual("lulz", instance.SampleProperty);
//        }
//    }

//    public interface ISampleInterface
//    {
//        string SampleProperty { get; set; }
//    }

//    public class SampleClass : ISampleInterface
//    {
//        public string SampleProperty { get; set; }
//    }
//}