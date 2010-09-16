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
using System.Threading;

namespace Siege.Provisions
{
    public class ThreadedUnitOfWorkStore : IUnitOfWorkStore
    {
        private static object lockObject = new object();
        private static ThreadLocal<Dictionary<Type, IUnitOfWork>> currentUnitOfWork = new ThreadLocal<Dictionary<Type, IUnitOfWork>>();

        static ThreadedUnitOfWorkStore()
        {
            Create();
        }

        private static void Create()
        {
            if (!currentUnitOfWork.IsValueCreated)
            {
                lock (lockObject)
                {
                    if (!currentUnitOfWork.IsValueCreated)
                    {
                        currentUnitOfWork.Value = new Dictionary<Type, IUnitOfWork>();
                    }
                }
            }
        }

        public IUnitOfWork CurrentFor<TPersistenceModule>() where TPersistenceModule : IPersistenceModule
        {
            Create();
            if (!currentUnitOfWork.Value.ContainsKey(typeof(TPersistenceModule))) return null;

            return currentUnitOfWork.Value[typeof(TPersistenceModule)]; 
        }

        public void SetUnitOfWork<TPersistenceModule>(IUnitOfWork unitOfWork) where TPersistenceModule : IPersistenceModule
        {
            Create();
            if (!currentUnitOfWork.Value.ContainsKey(typeof(TPersistenceModule))) currentUnitOfWork.Value.Add(typeof(TPersistenceModule), unitOfWork);
            currentUnitOfWork.Value[typeof(TPersistenceModule)] = unitOfWork;
        }

        public void Dispose()
        {
            if (currentUnitOfWork.Value != null)
            {
                var keysToRemove = new List<Type>();
                foreach(Type key in currentUnitOfWork.Value.Keys)
                {
                    currentUnitOfWork.Value[key].Dispose();
                    keysToRemove.Add(key);
                }

                foreach(Type key in keysToRemove)
                {
                    currentUnitOfWork.Value.Remove(key);
                }
            }
        }
    }
}