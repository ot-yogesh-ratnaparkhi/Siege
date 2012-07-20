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

using System;
using System.Collections.Generic;
using Siege.TypeGenerator.Actions;

namespace Siege.TypeGenerator
{
    public class NestedTypeGenerationContext : BaseTypeGenerationContext
    {
        private readonly DefineNestedTypeAction nestedType;
        internal Func<MethodBuilderBundle> EntryPoint { get; set; }

        public NestedTypeGenerationContext(TypeGenerator generator, Func<BuilderBundle> bundle, Action<BaseTypeGenerationContext> nestedClosure, IList<ITypeGenerationAction> actions) : base(generator)
        {
            BaseType = typeof(object);
            TypeGenerationActions = actions;

            nestedType = new DefineNestedTypeAction(bundle(), () => TypeName, () => BaseType);
            TypeGenerationActions.Add(nestedType);

            Builder = () => nestedType.LocalBuilder;

            nestedClosure(this);
            if(!ConstructorAdded) AddDefaultConstructor();
        }
    }
}
