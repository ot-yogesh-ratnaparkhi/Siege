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

namespace Siege.Requisitions.UnitTests
{
    public partial class SiegeContainerTests
    {
        [Test]
        public void Should_Use_Simple_Auto_Scanning_Conventions()
        {
            locator.Register(Using.Convention<TestConvention>());

            Assert.IsInstanceOfType(typeof (AutoScannedType), locator.GetInstance<IAutoScannedInterface>());
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
}