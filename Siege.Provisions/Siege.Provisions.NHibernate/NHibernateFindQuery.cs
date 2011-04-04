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

using System.Collections.Generic;
using NHibernate;
using NHibernate.Criterion;
using Siege.Provisions.Finders;

namespace Siege.Provisions.NHibernate
{
	public class NHibernateFindQuery : IFindQuery
	{
	    private readonly ICriteria executableCriteria;

		public NHibernateFindQuery(DetachedCriteria criteria, ISession session)
		{
            executableCriteria = criteria.GetExecutableCriteria(session);
		}

        public NHibernateFindQuery(ICriteria executableCriteria)
        {
            this.executableCriteria = executableCriteria;
        }

		public IList<T> Find<T>()
		{
            return executableCriteria.List<T>();
		}
	}
}