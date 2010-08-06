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

using NUnit.Framework;
using Siege.Requisitions.UnitTests.ContextualTests.Classes;
using Siege.Requisitions.InternalStorage;
using Siege.Requisitions.RegistrationSyntax;

namespace Siege.Requisitions.UnitTests.ContextualTests
{
	[TestFixture]
	public abstract class BaseContextTests
	{
        protected IServiceLocator locator;
		protected abstract IServiceLocatorAdapter GetAdapter();

		[SetUp]
		public virtual void SetUp()
		{
			locator = new ThreadedServiceLocator(GetAdapter());
		}

		[TearDown]
		public void TearDown()
		{
			locator.Dispose();
		}

		[Test]
		public void Should_Choose_Test_Service_2()
		{
			locator.Register(Given<ITestController>.Then<TestController>())
				.Register(Given<ITestRepository>.Then<TestRepository1>())
				.Register(
				Given<IBaseService>.When<ITestCondition>(context => context.TestType == TestTypes.Test1).Then<TestService1>())
				.Register(
				Given<IBaseService>.When<ITestCondition>(context => context.TestType == TestTypes.Test2).Then<TestService2>());

			locator.AddContext(new TestCondition(TestTypes.Test2));

			ITestController controller = locator.GetInstance<ITestController>();
			Assert.IsInstanceOfType(typeof (TestService2), controller.Service);
		}

		[Test]
		public void Should_Choose_Test_Service_1()
		{
			locator.Register(Given<ITestController>.Then<TestController>())
				.Register(Given<ITestRepository>.Then<TestRepository1>())
				.Register(
				Given<IBaseService>.When<ITestCondition>(context => context.TestType == TestTypes.Test2).Then<TestService2>())
				.Register(
				Given<IBaseService>.When<ITestCondition>(context => context.TestType == TestTypes.Test1).Then<TestService1>());
			locator.AddContext(new TestCondition(TestTypes.Test1));

			ITestController controller = locator.GetInstance<ITestController>();
			Assert.IsInstanceOfType(typeof (TestService1), controller.Service);
			Assert.IsInstanceOfType(typeof (TestRepository1), controller.Service.Repository);
		}

		[Test]
		public void Should_Choose_Service1_And_Repository_1()
		{
			locator.Register(Given<ITestController>.Then<TestController>())
				.Register(
				Given<IBaseService>.When<ITestCondition>(context => context.TestType == TestTypes.Test1).Then<TestService1>())
				.Register(
				Given<IBaseService>.When<ITestCondition>(context => context.TestType == TestTypes.Test2).Then<TestService2>())
				.Register(
				Given<ITestRepository>.When<IRepositoryCondition>(context => context.Condition == Conditions.ConditionA).Then
					<TestRepository1>())
				.Register(
				Given<ITestRepository>.When<IRepositoryCondition>(context => context.Condition == Conditions.ConditionB).Then
					<TestRepository2>());

			locator.AddContext(new TestCondition(TestTypes.Test1));
			locator.AddContext(new RepositoryCondition(Conditions.ConditionA));

			ITestController controller = locator.GetInstance<ITestController>();
			Assert.IsInstanceOfType(typeof (TestService1), controller.Service);
			Assert.IsInstanceOfType(typeof (TestRepository1), controller.Service.Repository);
		}

		[Test]
		public void Should_Choose_Service2_And_Repository_2()
		{
			locator.Register(Given<ITestController>.Then<TestController>())
				.Register(
				Given<IBaseService>.When<ITestCondition>(context => context.TestType == TestTypes.Test1).Then<TestService1>())
				.Register(
				Given<IBaseService>.When<ITestCondition>(context => context.TestType == TestTypes.Test2).Then<TestService2>())
				.Register(
				Given<ITestRepository>.When<IRepositoryCondition>(context => context.Condition == Conditions.ConditionA).Then
					<TestRepository1>())
				.Register(
				Given<ITestRepository>.When<IRepositoryCondition>(context => context.Condition == Conditions.ConditionB).Then
					<TestRepository2>());

			locator.AddContext(new TestCondition(TestTypes.Test2));
			locator.AddContext(new RepositoryCondition(Conditions.ConditionB));

			ITestController controller = locator.GetInstance<ITestController>();
			Assert.IsInstanceOfType(typeof (TestService2), controller.Service);
			Assert.IsInstanceOfType(typeof (TestRepository2), controller.Service.Repository);
		}

		[Test]
		public void Should_Choose_DefaultTestService_And_Repository_2()
		{
			locator.Register(Given<ITestController>.Then<TestController>())
				.Register(
				Given<IBaseService>.When<ITestCondition>(context => context.TestType == TestTypes.Test1).Then<TestService1>())
				.Register(
				Given<IBaseService>.When<ITestCondition>(context => context.TestType == TestTypes.Test2).Then<TestService2>())
				.Register(
				Given<ITestRepository>.When<IRepositoryCondition>(context => context.Condition == Conditions.ConditionA).Then
					<TestRepository1>())
				.Register(
				Given<ITestRepository>.When<IRepositoryCondition>(context => context.Condition == Conditions.ConditionB).Then
					<TestRepository2>())
				.Register(Given<IBaseService>.Then<DefaultTestService>());

			locator.AddContext(new TestCondition(TestTypes.Test3));
			locator.AddContext(new RepositoryCondition(Conditions.ConditionB));

			ITestController controller = locator.GetInstance<ITestController>();
			Assert.IsInstanceOfType(typeof (DefaultTestService), controller.Service);
			Assert.IsInstanceOfType(typeof (TestRepository2), controller.Service.Repository);
		}

		[Test]
		public void Should_Choose_Defaults_When_No_Context_Applies()
		{
			locator.Register(Given<ITestController>.Then<TestController>())
				.Register(Given<IBaseService>.Then<DefaultTestService>())
				.Register(Given<ITestRepository>.Then<DefaultTestRepository>())
				.Register(
				Given<IBaseService>.When<ITestCondition>(context => context.TestType == TestTypes.Test1).Then<TestService1>())
				.Register(
				Given<IBaseService>.When<ITestCondition>(context => context.TestType == TestTypes.Test2).Then<TestService2>())
				.Register(
				Given<ITestRepository>.When<IRepositoryCondition>(context => context.Condition == Conditions.ConditionA).Then
					<TestRepository1>())
				.Register(
				Given<ITestRepository>.When<IRepositoryCondition>(context => context.Condition == Conditions.ConditionB).Then
					<TestRepository2>());

			locator.AddContext(new TestCondition(TestTypes.Test3));
			locator.AddContext(new RepositoryCondition(Conditions.ConditionC));

			ITestController controller = locator.GetInstance<ITestController>();
			Assert.IsInstanceOfType(typeof (DefaultTestService), controller.Service);
			Assert.IsInstanceOfType(typeof (DefaultTestRepository), controller.Service.Repository);
		}

		[Test]
		public void Should_Change_Selection_As_Context_Is_Applied()
		{
			Assert.AreEqual(0, locator.Store.ExecutionStore.RequestedTypes.Count);

			locator.Register(Given<ITestController>.Then<TestController>())
				.Register(Given<IBaseService>.Then<DefaultTestService>())
				.Register(Given<ITestRepository>.Then<DefaultTestRepository>())
				.Register(
				Given<IBaseService>.When<ITestCondition>(context => context.TestType == TestTypes.Test1).Then<TestService1>())
				.Register(
				Given<IBaseService>.When<ITestCondition>(context => context.TestType == TestTypes.Test2).Then<TestService2>())
				.Register(
				Given<ITestRepository>.When<IRepositoryCondition>(context => context.Condition == Conditions.ConditionA).Then
					<TestRepository1>())
				.Register(
				Given<ITestRepository>.When<IRepositoryCondition>(context => context.Condition == Conditions.ConditionB).Then
					<TestRepository2>());

			ITestController controller = locator.GetInstance<ITestController>();
			Assert.IsInstanceOfType(typeof (DefaultTestService), controller.Service);
			Assert.IsInstanceOfType(typeof (DefaultTestRepository), controller.Service.Repository);

			locator.AddContext(new TestCondition(TestTypes.Test1));

			controller = locator.GetInstance<ITestController>();
			Assert.IsInstanceOfType(typeof (TestService1), controller.Service);
			Assert.IsInstanceOfType(typeof (DefaultTestRepository), controller.Service.Repository);

			locator.AddContext(new RepositoryCondition(Conditions.ConditionB));

			controller = locator.GetInstance<ITestController>();
			Assert.IsInstanceOfType(typeof (TestService1), controller.Service);
			Assert.IsInstanceOfType(typeof (TestRepository2), controller.Service.Repository);
		}

		[Test]
		public void Should_Choose_Defaults_When_No_Context_Provided()
		{
			locator.Register(Given<ITestController>.Then<TestController>())
				.Register(
				Given<IBaseService>.When<ITestCondition>(context => context.TestType == TestTypes.Test1).Then<TestService1>())
				.Register(
				Given<IBaseService>.When<ITestCondition>(context => context.TestType == TestTypes.Test2).Then<TestService2>())
				.Register(
				Given<ITestRepository>.When<IRepositoryCondition>(context => context.Condition == Conditions.ConditionA).Then
					<TestRepository1>())
				.Register(
				Given<ITestRepository>.When<IRepositoryCondition>(context => context.Condition == Conditions.ConditionB).Then
					<TestRepository2>())
				.Register(Given<IBaseService>.Then<DefaultTestService>())
				.Register(Given<ITestRepository>.Then<DefaultTestRepository>());


			ITestController controller = locator.GetInstance<ITestController>();
			Assert.IsInstanceOfType(typeof (DefaultTestService), controller.Service);
			Assert.IsInstanceOfType(typeof (DefaultTestRepository), controller.Service.Repository);
		}
	}
}