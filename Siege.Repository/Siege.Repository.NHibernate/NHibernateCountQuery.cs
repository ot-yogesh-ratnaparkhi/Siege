﻿/*   Copyright 2009 - 2010 Marcus Bratton

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

using NHibernate;
using NHibernate.Criterion;
using Siege.Repository.Finders;

namespace Siege.Repository.NHibernate
{
	public class NHibernateCountQuery : ICountQuery
	{
        private readonly ICriteria executableCriteria;

		public NHibernateCountQuery(DetachedCriteria criteria, ISession session)
		{
            executableCriteria = criteria.GetExecutableCriteria(session);
		}

        public NHibernateCountQuery(ICriteria executableCriteria)
        {
            this.executableCriteria = executableCriteria;
        }

		public int Count()
		{
		    var result = executableCriteria.SetProjection(Projections.RowCount()).UniqueResult();
		    return result != null ? (int) result : 0;
		}
	}
}