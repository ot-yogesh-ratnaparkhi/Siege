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
using System.Reflection.Emit;

namespace Siege.TypeGenerator.Actions
{
    internal class DefineNestedTypeAction : DefineTypeAction
    {
        internal BuilderBundle LocalBuilder { get; set; }
        public DefineNestedTypeAction(BuilderBundle bundle, Func<string> typeName, Func<Type> baseType) : base(bundle, typeName, baseType)
        {
            LocalBuilder = new BuilderBundle {ModuleBuilder = bundle.ModuleBuilder};
        }

        protected override TypeBuilder DefineType()
        {
            return LocalBuilder.TypeBuilder = bundle.TypeBuilder.DefineNestedType("NestedTypeIn" + bundle.TypeBuilder.Name + Guid.NewGuid().ToString().Replace("-", ""),
                                                                  TypeAttributes.Sealed |
                                                                  TypeAttributes.NestedPrivate |
                                                                  TypeAttributes.BeforeFieldInit);
        }
    }
}
