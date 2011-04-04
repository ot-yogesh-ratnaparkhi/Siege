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
using System.Collections.Generic;

namespace Siege.Provisions.UnitOfWork
{
    public interface IUnitOfWorkManager
    {
        IUnitOfWork For<TDatabase>() where TDatabase : IDatabase;
        void Add(IDatabase module);
    }

    public abstract class UnitOfWorkManager : IUnitOfWorkManager
    {
        protected readonly Dictionary<Type, UnitOfWorkBundle> stores = new Dictionary<Type, UnitOfWorkBundle>();

        public IUnitOfWork For<TDatabase>() where TDatabase : IDatabase
		{
            var bundle = stores[typeof(TDatabase)];

            if (bundle.Store.CurrentFor<TDatabase>() == null)
            {
                bundle.Store.SetUnitOfWork<TDatabase>(bundle.Factory.Create());
            }

            return bundle.Store.CurrentFor<TDatabase>();
		}

        public void Add(IDatabase module)
        {
            if (stores.ContainsKey(module.GetType())) return;

            stores.Add(module.GetType(), new UnitOfWorkBundle {Factory = module.Factory, Store = module.Store});
        }
	}

    public class UnitOfWorkBundle
    {
        public IUnitOfWorkFactory Factory { get; set; }
        public IUnitOfWorkStore Store { get; set; }
    }
}