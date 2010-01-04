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

namespace Siege.DynamicTypeGeneration.Actions
{
    internal class FieldAssignmentAction : ITypeGenerationAction
    {
        private readonly Func<Func<ILGenerator>> bundle;
        private readonly IGeneratedParameter source;
        private Func<FieldInfo> target;

        public FieldAssignmentAction(Func<Func<ILGenerator>> bundle, IGeneratedParameter source)
        {
            this.bundle = bundle;
            this.source = source;
        }

        public void Execute()
        {
            if(target() != null)
            {
                var methodBuilder = this.bundle()();

                methodBuilder.Emit(OpCodes.Ldarg_0);
                methodBuilder.Emit(OpCodes.Ldarg, source.LocalIndex());
                methodBuilder.Emit(OpCodes.Stfld, target());
            }
        }

        public void To(Func<FieldInfo> field)
        {
            target = field;
        }
    }
}
