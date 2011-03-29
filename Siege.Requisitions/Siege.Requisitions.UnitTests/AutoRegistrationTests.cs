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
using Siege.Requisitions.Extensions.AutoScanner;
using Siege.Requisitions.Extensions.Conventions;
using Siege.Requisitions.UnitTests.TestNamespace;

namespace Siege.Requisitions.UnitTests
{
    public partial class ServiceLocatorTests
    {
        [Test]
        public void ShouldUseSimpleAutoScanningConventions()
        {
            locator.Register(Using.Convention<TestConvention>());

            Assert.IsInstanceOf<AutoScannedType>(locator.GetInstance<IAutoScannedInterface>());
        }

		[Test]
		public void ShouldUseNamespace()
		{
			locator.Register(Using.Convention<NamespaceTestConvention>());
			Assert.IsInstanceOf<AutoScanningConventionTest>(locator.GetInstance<ITest>());
			Assert.IsFalse(locator.HasTypeRegistered(typeof(IAutoScannedInterface)));
		}

		[Test]
		public void ShouldUseGeneric()
		{
			locator.Register(Using.Convention<NamespaceTestConvention>());
			Assert.IsInstanceOf<GenericTest>(locator.GetInstance<ITest<AutoScannedType>>());
		}
    }

    public class TestConvention : AutoScanningConvention
    {
        public TestConvention()
        {
            FromAssemblyContaining<AutoScannedType>();
            ForTypesImplementing<IAutoScannedInterface>();
        }
    }

    public interface IAutoScannedInterface
    {
    }

    public class AutoScannedType : IAutoScannedInterface
    {
    }

	public class NamespaceTestConvention : AutoScanningConvention
	{
		public NamespaceTestConvention()
		{
			FromAssemblyContaining<AutoScannedType>();
			FromNamespaceOf<ITest>();
		}
	}
	public class GenericTestConvention : AutoScanningConvention
	{
		public GenericTestConvention()
		{
			FromAssemblyContaining<AutoScannedType>();
			FromNamespaceOf<ITest>();
		}
	}
}

namespace Siege.Requisitions.UnitTests.TestNamespace
{
	public interface ITest
	{
	}

	public interface ITest<T>
	{
	}

	public class AutoScanningConventionTest : ITest
	{
	}

	public class GenericTest : ITest<AutoScannedType>
	{
	}
}