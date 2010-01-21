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
using System.Reflection;
using Siege.DynamicTypeGeneration.Actions;

namespace Siege.DynamicTypeGeneration
{
    public class GeneratedField
    {
        private readonly Type type;
        private readonly Func<BuilderBundle> bundle;
        private readonly AddFieldAction action;

        public GeneratedField(Type type, AddFieldAction action)
        {
            this.type = type;
            this.action = action;
        }

        public GeneratedField(Func<BuilderBundle> bundle, AddFieldAction action)
        {
            this.bundle = bundle;
            this.action = action;
        }

        public Func<FieldInfo> Field { get { return () => action.Field; } }
        public Type Type { get { return type ?? bundle().TypeBuilder; } }
    }
}
