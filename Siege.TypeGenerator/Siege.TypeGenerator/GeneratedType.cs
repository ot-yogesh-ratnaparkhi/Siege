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
using Siege.TypeGenerator.Actions;

namespace Siege.TypeGenerator
{
    public class GeneratedType
    {
        private readonly BuilderBundle bundle;
        private readonly TypeGenerationContext context;

        public GeneratedType(BuilderBundle bundle, TypeGenerationContext context)
        {
            this.bundle = bundle;
            this.context = context;
        }

        public Type Create()
        {
            foreach(ITypeGenerationAction action in context.TypeGenerationActions) 
            {
                action.Execute();
            }

            foreach (Func<BuilderBundle> nestedType in context.TypesToComplete)
            {
                nestedType().TypeBuilder.CreateType();
            }

            return bundle.TypeBuilder.CreateType();
        }
    }
}
