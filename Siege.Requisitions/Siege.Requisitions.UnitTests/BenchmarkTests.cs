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
using Siege.Requisitions.InternalStorage;
using Siege.Requisitions.UnitTests.ContextualTests.Classes;
using Siege.Requisitions.Extensions.ExtendedRegistrationSyntax;

namespace Siege.Requisitions.UnitTests
{
	public abstract partial class ServiceLocatorTests
	{
		const double iterations = 10000;

        [Test]
		[Category("Load")]
		public virtual void LoadPerformanceTest()
        {
            var totalTime = new TimeSpan();
            DateTime start = DateTime.Now;

			locator
				.Register(Given<ITestController>.Then<TestController>())
				.Register(Given<IBaseService>.Then<DefaultTestService>())
				.Register(Given<ITestRepository>.Then<DefaultTestRepository>())
				.Register(Given<IBaseService>
							.When<ITestCondition>(context => context.TestType == TestTypes.Test1)
							.Then<TestService1>())
				.Register(Given<IBaseService>
							.When<ITestCondition>(context => context.TestType == TestTypes.Test2)
							.Then<TestService2>())
				.Register(Given<ITestRepository>
							.When<IRepositoryCondition>(context => context.Condition == Conditions.ConditionA)
							.Then<TestRepository1>())
				.Register(Given<ITestRepository>
							.When<IRepositoryCondition>(context => context.Condition == Conditions.ConditionB)
							.Then<TestRepository2>());

			locator.GetInstance<ITestController>();

            DateTime end = DateTime.Now;
            totalTime += end - start;


            Console.WriteLine("Total Registration Time (in milliseconds): " + totalTime.TotalMilliseconds);
            Console.WriteLine("Average Registration Time (in milliseconds): " + totalTime.TotalMilliseconds / iterations);

			for (int i = 0; i < iterations; i++)
            {
                start = DateTime.Now;
				
                var controller = locator.GetInstance<ITestController>();
                Assert.IsInstanceOf<DefaultTestService>(controller.Service);
				Assert.IsInstanceOf<DefaultTestRepository>(controller.Service.Repository);

				locator.AddContext(new TestCondition(TestTypes.Test1));

				controller = locator.GetInstance<ITestController>();
				Assert.IsInstanceOf<TestService1>(controller.Service);
				Assert.IsInstanceOf<DefaultTestRepository>(controller.Service.Repository);

				locator.AddContext(new RepositoryCondition(Conditions.ConditionB));

				controller = locator.GetInstance<ITestController>();
				Assert.IsInstanceOf<TestService1>(controller.Service);
				Assert.IsInstanceOf<TestRepository2>(controller.Service.Repository);

                locator.Store.Get<IContextStore>().Clear();
                end = DateTime.Now;

                totalTime += end - start;
			}

			Console.WriteLine("Total Execution Time (in milliseconds): " + totalTime.TotalMilliseconds);
			Console.WriteLine("Average Execution Time (in milliseconds): " + totalTime.TotalMilliseconds / iterations);
		}


        [Test]
		[Category("With Siege")]
		public virtual void WithSiege()
        {
            var totalTime = new TimeSpan();
            DateTime start = DateTime.Now;

			locator
				.Register(Given<ITestController>.Then<TestController>())
				.Register(Given<IBaseService>.Then<DefaultTestService>())
				.Register(Given<ITestRepository>.Then<DefaultTestRepository>());

            DateTime end = DateTime.Now;
            totalTime += end - start;

            Console.WriteLine("Total Registration Time (in milliseconds): " + totalTime.TotalMilliseconds);
            Console.WriteLine("Average Registration Time (in milliseconds): " + totalTime.TotalMilliseconds / iterations);

            for (int i = 0; i < iterations; i++)
            {
                start = DateTime.Now;
                locator.GetInstance<ITestController>();
                end = DateTime.Now;
                totalTime += end - start;
			}

			Console.WriteLine("Total Execution Time (with Siege in milliseconds): " + totalTime.TotalMilliseconds);
			Console.WriteLine("Average Execution Time (with Siege in milliseconds): " + totalTime.TotalMilliseconds / iterations);
		}

        [Test]
		[Category("Without Siege")]
		public virtual void WithoutSiege()
        {
            var totalTime = new TimeSpan();

            DateTime start = DateTime.Now;

			RegisterWithoutSiege<ITestController, TestController>();
			RegisterWithoutSiege<IBaseService, DefaultTestService>();
			RegisterWithoutSiege<ITestRepository, DefaultTestRepository>();

            DateTime end = DateTime.Now;
            totalTime += end - start;

            Console.WriteLine("Total Registration Time (in milliseconds): " + totalTime.TotalMilliseconds);
            Console.WriteLine("Average Registration Time (in milliseconds): " + totalTime.TotalMilliseconds / iterations);
            
            for (int i = 0; i < iterations; i++)
            {
                start = DateTime.Now;
                ResolveWithoutSiege<ITestController>();
                end = DateTime.Now;

                totalTime += end - start;
			}


			Console.WriteLine("Total Execution Time (without Siege in milliseconds): " + totalTime.TotalMilliseconds);
			Console.WriteLine("Average Execution Time (without Siege in milliseconds): " + totalTime.TotalMilliseconds / iterations);
		}
	}
}