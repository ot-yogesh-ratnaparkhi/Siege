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
using Rhino.Mocks;
using Siege.Provisions.Finders;

namespace Siege.Provisions.Tests
{
	[TestFixture]
	public class FinderTests
	{
		private MockRepository mocks;
		private TestFinder finder;
		private ICountQuery countQuery;
		private IFindQuery findQuery;

		[SetUp]
		public void SetUp()
		{
			mocks = new MockRepository();
			countQuery = mocks.DynamicMock<ICountQuery>();
			findQuery = mocks.DynamicMock<IFindQuery>();
			finder = new TestFinder(findQuery, countQuery);
		}

		[Test]
		public void FindShouldCallUnitOfWorkManagerCurrentSession()
		{
			using (mocks.Record())
			{
				findQuery.Expect(f => f.Find<object>()).Return(null);
			}
			using (mocks.Playback())
			{
				finder.Find();
			}
		}

		[Test]
		public void CountShouldCallUnitOfWorkManagerCurrentSession()
		{
			using (mocks.Record())
			{
				countQuery.Expect(c => c.Count()).Return(1);
			}
			using (mocks.Playback())
			{
				finder.Count();
			}
		}

		[Test]
		[ExpectedException(typeof (NotImplementedException))]
		public void ShouldThrowExceptionWhenGetCountCriteriaNotOverriden()
		{
			new FaultedFinder().Count();
		}
	}

	public class FaultedFinder : Finder<object>
	{
		protected override IFindQuery CreateFindQuery()
		{
			return null;
		}
	}

	public class TestFinder : Finder<object>
	{
		private readonly IFindQuery findQuery;
		private readonly ICountQuery countQuery;

		public TestFinder(IFindQuery findQuery, ICountQuery countQuery)
		{
			this.findQuery = findQuery;
			this.countQuery = countQuery;
		}

		protected override IFindQuery CreateFindQuery()
		{
			return this.findQuery;
		}

		protected override ICountQuery CreateCountQuery()
		{
			return this.countQuery;
		}
	}
}