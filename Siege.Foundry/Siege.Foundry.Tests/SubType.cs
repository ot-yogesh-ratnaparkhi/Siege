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

namespace Siege.Foundry.Tests
{
	public class SubType : BaseType
	{
		private BaseType value;

		public BaseType Property
		{
			get { return value; }
			set { this.value = value; }
		}

		public SubType(BaseType baseType)
		{
			value = baseType;
		}

		public void TestMethod(string type)
		{

		}

		public override string DoSomething(string val1)
		{
			Processor processor = new Processor();

			var str = processor.Process(val1, new BaseType());

			return str;
		}
	}
}