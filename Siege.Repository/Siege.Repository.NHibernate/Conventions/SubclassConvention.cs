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

using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace Siege.Repository.NHibernate.Conventions
{
    public abstract class SubclassConvention : IJoinedSubclassConvention
    {
        public void Apply(IJoinedSubclassInstance instance)
        {
            instance.Schema(Schema);
            instance.Table(instance.EntityType.Name + ConventionConstants.TableSuffix);
            instance.Key.Column(instance.EntityType.BaseType.Name + ConventionConstants.Id);
        }

        protected abstract string Schema { get; }
    }
}